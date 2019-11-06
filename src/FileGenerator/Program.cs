using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RazorLight;
using System.IO;

namespace FileGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Dictionary<string, bool> features = new Dictionary<string, bool>();
            var featureConfig = args[0];
            var splittedArgs = featureConfig.Split(";");
            foreach (var arg in splittedArgs)
            {
                if (arg == "")
                {
                    continue;
                }
                var keyValue = arg.Split(":");
                features.Add(keyValue[0], keyValue[1].ToLower().Equals("true"));
            }

            // src\FileGenerator\bin\release\netcoreapp2.0\win-x64\publish\FileGenerator.exe
            string pathFromExeDirectoryToSource = "..\\..\\..\\..\\..\\..\\";
            string pathToSlnFolder = AppDomain.CurrentDomain.BaseDirectory + pathFromExeDirectoryToSource + args[5] + "\\";

            string classForGenerationName = args[1];
            string classTemplateName = args[2];
            string pathToDirectoryWithFeatures = pathToSlnFolder + args[3];
            string pathToTemplateDirectory = pathToSlnFolder + args[4];
            Model model = new Model(features);

            await GenerateMainFile(model, classTemplateName, classForGenerationName, pathToDirectoryWithFeatures, pathToTemplateDirectory);
        }

        public static async Task GenerateMainFile(Model model, string classTemplateName,
         string classForGenerationName, string pathToDirectoryWithFeatures, string pathToTemplateDirectory)
        {
            Console.WriteLine("Start generate file");

            var engine = new RazorLightEngineBuilder();
            var engineUFSP = engine.UseFilesystemProject(pathToTemplateDirectory);
            var engineUMCP = engineUFSP.UseMemoryCachingProvider();
            var engineBuid = engineUMCP.Build();

            var result = engineBuid.CompileRenderAsync(classTemplateName, model).Result;
            File.WriteAllText(Path.Combine(pathToDirectoryWithFeatures, classForGenerationName), result);

            Console.WriteLine("File generated");
        }
    }
}
