using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AppGen.AppGenLib
{
    public static class XmlModelGenerator
    {
        private static Tuple<List<IGrouping<string, FeatureWithParameters>>, string>
            FindLogicalAbstractGroupIfItExist(List<FeatureWithParameters> featureListForModelGeneration)
        {
            var groupedByXorAbstract = featureListForModelGeneration.GroupBy(x => x.xorAbstractGroup).ToList();
            var groupedByOrAbstract = featureListForModelGeneration.GroupBy(x => x.orAbstractGroup).ToList();
            var groupedByAndAbstract = featureListForModelGeneration.GroupBy(x => x.andAbstractGroup).ToList();

            if (IsLogicalGroupExist(groupedByXorAbstract))
                return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(groupedByXorAbstract, "xor");
            if (IsLogicalGroupExist(groupedByOrAbstract))
                return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(groupedByOrAbstract, "or");
            if (IsLogicalGroupExist(groupedByAndAbstract))
                return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(groupedByAndAbstract, "and");
            return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(null, "No result");

        }

        private static Tuple<List<IGrouping<string, FeatureWithParameters>>, string>
            FindLogicalGroupIfItExist(List<FeatureWithParameters> featureListForModelGeneration)
        {
            var groupedByXor = featureListForModelGeneration.GroupBy(x => x.xorGroup).ToList();
            var groupedByOr = featureListForModelGeneration.GroupBy(x => x.orGroup).ToList();
            var groupedByAnd = featureListForModelGeneration.GroupBy(x => x.andGroup).ToList();

            if (IsLogicalGroupExist(groupedByXor))
                return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(groupedByXor, "xor");
            if (IsLogicalGroupExist(groupedByOr))
                return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(groupedByOr, "or");
            if (IsLogicalGroupExist(groupedByAnd))
                return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(groupedByAnd, "and");
            return new Tuple<List<IGrouping<string, FeatureWithParameters>>, string>(null, "No result");

        }

        private static bool IsLogicalGroupExist(List<IGrouping<string, FeatureWithParameters>> logicalGroup)
        {
            if (logicalGroup.Count > 1 || logicalGroup.Count == 1 && logicalGroup[0].Key != null)
            {
                return true;
            }
            return false;
        }

        public static void GenerateXmlModel(List<FeatureWithParameters> featureListForModelGeneration, string generatedModelFileName)
        {
            XDocument xdoc = new XDocument();

            XElement model = new XElement("model");
            model.Add(new XAttribute("name", "generated_model"));

            var resultOfLogicalAbstractGroupSearch = FindLogicalAbstractGroupIfItExist(featureListForModelGeneration);
            var groupedByLogicalOperationAbstract = resultOfLogicalAbstractGroupSearch.Item1;
            string operationName = resultOfLogicalAbstractGroupSearch.Item2;

            if (groupedByLogicalOperationAbstract != null)
            {
                XElement logicalOperationAbstract = new XElement(operationName);
                foreach (var group in groupedByLogicalOperationAbstract)
                {
                    var groupedByAbstractFeatures = group.GroupBy(x => x.abstractFeatureName);
                    List<XElement> abstractNodeList = GroupBySimpleAbstractFeatureParsing(groupedByAbstractFeatures);
                    if (group.Key == null)
                    {
                        abstractNodeList.ForEach(x => model.Add(x));
                    }
                    else
                    {
                        abstractNodeList.ForEach(x => logicalOperationAbstract.Add(x));
                        model.Add(logicalOperationAbstract);
                    }
                }
            }
            else
            {
                var groupedByAbstractFeatures = featureListForModelGeneration.GroupBy(x => x.abstractFeatureName);
                List<XElement> abstractNodeList = GroupBySimpleAbstractFeatureParsing(groupedByAbstractFeatures);
                abstractNodeList.ForEach(x => model.Add(x));
            }

            xdoc.Add(model);
            xdoc.Save(generatedModelFileName);

        }

        private static List<XElement> GroupBySimpleAbstractFeatureParsing(
            IEnumerable<IGrouping<string, FeatureWithParameters>> groupedByAbstractFeatures)
        {
            List<XElement> abstractNodeList = new List<XElement>();
            foreach (var group in groupedByAbstractFeatures)
            {
                string abstractFeatureName = group.ToList()[0].abstractFeatureName;

                var resultOfLogicalGroupSearch = FindLogicalGroupIfItExist(group.ToList());
                var groupedByLogicalOperation = resultOfLogicalGroupSearch.Item1;
                string operationName = resultOfLogicalGroupSearch.Item2;

                if (groupedByLogicalOperation != null)
                {
                    XElement logicalOperation = new XElement(operationName);
                    foreach (var groupOr in groupedByLogicalOperation)
                    {
                        XElement abstractNode = new XElement("abstract_node");
                        abstractNode.Add(new XAttribute("name", abstractFeatureName));

                        XElement concreteNodes = new XElement("concrete_nodes");
                        List<XElement> concreteNodeList = GroupBySimpleConcreteFeatureParsing(group);

                        if (groupOr.Key == null)
                        {
                            concreteNodeList.ForEach(concreteNode => concreteNodes.Add(concreteNode));
                        }
                        else
                        {
                            logicalOperation.Add(concreteNodeList);
                            concreteNodes.Add(logicalOperation);
                        }
                        abstractNode.Add(concreteNodes);
                        abstractNodeList.Add(abstractNode);
                    }
                }
                else
                {
                    XElement concreteNodes = new XElement("concrete_nodes");

                    List<XElement> concreteNodeList = GroupBySimpleConcreteFeatureParsing(group);
                    concreteNodeList.ForEach(concreteNode => concreteNodes.Add(concreteNode));
                    XElement abstractNode = new XElement("abstract_node");
                    abstractNode.Add(new XAttribute("name", abstractFeatureName));
                    abstractNode.Add(concreteNodes);
                    abstractNodeList.Add(abstractNode);
                }
            }

            return abstractNodeList;
        }

        private static List<XElement> GroupBySimpleConcreteFeatureParsing(IGrouping<string, FeatureWithParameters> group)
        {
            List<XElement> concreteNodeList = new List<XElement>();
            foreach (var featureWithParams in group)
            {

                XElement concreteNode = new XElement("concrete_node");
                concreteNode.Add(new XAttribute("name", featureWithParams.featureName));
                concreteNode.Add(new XElement("selected", false));

                if (featureWithParams.clickTO != null)
                    concreteNode.Add(new XElement("clickTO", featureWithParams.clickTO));

                if (featureWithParams.clickFROM != null)
                    concreteNode.Add(new XElement("clickFROM", featureWithParams.clickFROM));

                if (featureWithParams.longClickTO != null)
                    concreteNode.Add(new XElement("longClickTO", featureWithParams.longClickTO));

                if (featureWithParams.longClickFROM != null)
                    concreteNode.Add(new XElement("longClickFROM", featureWithParams.longClickFROM));

                if (featureWithParams.swipeRightTO != null)
                    concreteNode.Add(new XElement("swipeRightTO", featureWithParams.swipeRightTO));

                if (featureWithParams.onButtonClickFromArgTO != null)
                    concreteNode.Add(new XElement("onButtonClickFromArgTO",
                        featureWithParams.onButtonClickFromArgTO));

                concreteNode.Add(new XElement("required", featureWithParams.required));
                concreteNodeList.Add(concreteNode);

            }
            return concreteNodeList;
        }


    }
}