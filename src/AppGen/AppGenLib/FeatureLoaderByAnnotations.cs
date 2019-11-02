using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;



namespace AppGen.AppGenLib
{
    public static class FeatureLoaderByAnnotations
    {
        public static List<FeatureWithParameters> LoadFeaturesWithParams(string path)
        {
            List<FeatureWithParameters> featureListForModelGeneration = new List<FeatureWithParameters>();

            string[] filePaths = Directory.GetFiles(path, "*.java", SearchOption.AllDirectories);
            string annotationPattern = @"\s*@AnnotationList.\w*";
            string patternFeature = @"\s*@AnnotationList.Feature\w*";
            string patternAbstract = @"\s*@AnnotationList.AbstractFeature\w*";
            string patternRequired = @"\s*@AnnotationList.RequiredFeature\w*";
            string patternOnItemClickTO = @"\s*@AnnotationList.OnItemClickTO\w*";
            string patternOnItemClickFROM = @"\s*@AnnotationList.OnItemClickFROM\w*";
            string patternOnLongItemClickTO = @"\s*@AnnotationList.OnLongItemClickTO\w*";
            string patternOnLongItemClickFROM = @"\s*@AnnotationList.OnLongItemClickFROM\w*";
            string patternOnSwipeRightTO = @"\s*@AnnotationList.OnSwipeRightTO\w*";
            string patternOnButtonClickFromArgTO = @"\s*@AnnotationList.OnButtonClickFromArgTO\w*";

            string patternXorGroup = @"\s*@AnnotationList.XorGroup\w*";
            string patternOrGroup = @"\s*@AnnotationList.OrGroup\w*";
            string patternAndGroup = @"\s*@AnnotationList.AndGroup\w*";

            string patternXorAbstractGroup = @"\s*@AnnotationList.XorAbstractGroup\w*";
            string patternOrAbstractGroup = @"\s*@AnnotationList.OrAbstractGroup\w*";
            string patternAndAbstractGroup = @"\s*@AnnotationList.AndAbstractGroup\w*";


            foreach (var filePath in filePaths)
            {
                if (filePath.Contains("AnnotationList.java"))
                    continue;

                using (var reader = new StreamReader(filePath))
                {
                    string featureName = null;
                    string abstractFeatureName = null;
                    bool requiredFeature = false;
                    string longClickTO = null;
                    string longClickFROM = null;
                    string clickTO = null;
                    string clickFROM = null;

                    string xorGroup = null;
                    string orGroup = null;
                    string andGroup = null;

                    string xorAbstractGroup = null;
                    string orAbstractGroup = null;
                    string andAbstractGroup = null;

                    string swipeRightTO = null;
                    string onButtonClickFromArgTO = null;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        // check start of the new record lines
                        if (Regex.Match(line, annotationPattern).Success)
                        {
                            if (Regex.Match(line, patternOnItemClickTO).Success)
                                clickTO = line.Split(new[] { "OnItemClickTO" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternOnItemClickFROM).Success)
                                clickFROM =  line.Split(new[] { "OnItemClickFROM" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternOnLongItemClickTO).Success)
                                longClickTO = line.Split(new[] { "OnLongItemClickTO" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternOnLongItemClickFROM).Success)
                                longClickFROM = line.Split(new[] { "OnLongItemClickFROM" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternFeature).Success)
                                featureName = line.Split(new[] { "Feature" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternAbstract).Success)
                                abstractFeatureName = line.Split(new[] { "AbstractFeature" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternRequired).Success)
                                requiredFeature = true;

                            if (Regex.Match(line, patternOnSwipeRightTO).Success)
                                swipeRightTO = line.Split(new[] { "SwipeRightTO" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternOnButtonClickFromArgTO).Success)
                                onButtonClickFromArgTO = line.Split(new[] { "OnButtonClickFromArgTO" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternXorGroup).Success)
                                xorGroup = line.Split(new[] { "XorGroup" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternOrGroup).Success)
                                orGroup = line.Split(new[] { "OrGroup" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternAndGroup).Success)
                                andGroup = line.Split(new[] { "AndGroup" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternXorAbstractGroup).Success)
                                xorAbstractGroup = line.Split(new[] { "XorAbstractGroup" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternOrAbstractGroup).Success)
                                orAbstractGroup =  line.Split(new[] { "OrAbstractGroup" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];

                            if (Regex.Match(line, patternAndAbstractGroup).Success)
                                andAbstractGroup =  line.Split(new[] { "AndAbstractGroup" }, StringSplitOptions.None)[1]
                                    .Split(new[] { "\"" }, StringSplitOptions.None)[1];
                        }
                    }
                    if (featureName != null)
                        featureListForModelGeneration.Add(
                        new FeatureWithParameters(
                            featureName, abstractFeatureName, requiredFeature,
                            longClickTO, longClickFROM, clickTO, clickFROM,
                            xorGroup, orGroup, andGroup,
                            xorAbstractGroup, orAbstractGroup, andAbstractGroup,
                            swipeRightTO, onButtonClickFromArgTO));
                }
            }

            return featureListForModelGeneration;
        }
    }
}
