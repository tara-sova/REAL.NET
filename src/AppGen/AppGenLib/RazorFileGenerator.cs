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
    class RazorFileGenerator
    {
        public static async Task GenerateMainFile(FeatureModel model, string classTemplate,
         string classForGeneration, string pathToDirectoryWithFeatures)
        {
            Console.WriteLine("Start generate file");

            var engine = new RazorLightEngineBuilder();

            var engineUFSP = engine.UseFilesystemProject(AppDomain.CurrentDomain.BaseDirectory + "..\\..");
            var engineUMCP = engineUFSP.UseMemoryCachingProvider();
            var engineBuid = engineUMCP.Build();

            var comppilied = engineBuid.CompileRenderAsync("LectureListActivityTemplate.cshtml", model);
            var result = comppilied.Result;
            File.WriteAllText(Path.Combine(pathToDirectoryWithFeatures, classForGeneration), result);
            Console.WriteLine("File generated");
        }
    }
}
