using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using RazorLight;


namespace AppGen.AppGenLib
{
    class CopyFeatureFilesToEnviroment
    {
        public static void CopyAppDirectoryToEnvironment(string sourceDirectory, string targetDirectory, FeatureModel model)
        {
            DirectoryInfo dirSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo dirTarget =
                new DirectoryInfo(Path.Combine(targetDirectory, sourceDirectory.Split(new[] { "\\" }, StringSplitOptions.None).Last()));
            
            FindFeatureFilesThatShouldNotBeLoad(sourceDirectory, model);

            CopyAll(dirSource, dirTarget, model);

        }

        private static readonly List<string> shouldNotBeLoadedFileList = new List<string>();
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target, FeatureModel model)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fileInfo in source.GetFiles())
            {
                if (shouldNotBeLoadedFileList.Contains(fileInfo.Name))
                {
                    continue;
                }

                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fileInfo.Name);
                fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
            }

            foreach (DirectoryInfo dirInfoSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(dirInfoSourceSubDir.Name);
                CopyAll(dirInfoSourceSubDir, nextTargetSubDir, model);
            }
        }

        private static void FindFeatureFilesThatShouldNotBeLoad(string path, FeatureModel model)
        {
            string[] filePaths = Directory.GetFiles(path, "*.java", SearchOption.AllDirectories);
            string patternFeature = @"\s*@AnnotationList.Feature\w*";
            string patternAnyway = @"\s*@AnnotationList.NeededAnywayFeatureFile\w*";
            //            string patternFeatureConnected = @"\s*@AnnotationList.ConnectedToFeature\w*";

            foreach (var filePath in filePaths)
            {
                if (filePath.Contains("AnnotationList.java"))
                    continue;

                using (var reader = new StreamReader(filePath))
                {
                    string featureName = null;
                    string neededAnyway = null;
                    string fileName = filePath.Split(new[] { "\\" }, StringSplitOptions.None).Last();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (Regex.Match(line, patternFeature).Success)
                        {
                            featureName = line.Split(new[] { "Feature" }, StringSplitOptions.None)[1].Split(new[] { "\"" }, StringSplitOptions.None)[1];
                            if (!model.Features[featureName])
                            {
                                shouldNotBeLoadedFileList.Add(fileName);
                            }
                        }

                        if (Regex.Match(line, patternAnyway).Success)
                        {
                            shouldNotBeLoadedFileList.Remove(fileName);
                        }
                    }
                }
            }
        }
    }
}
