using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AppGen.AppGenLib
{
    public static class ModelLoaderViaXML
    {
        public static Repo.IModel LoadModel(Repo.IModel repoModel, string modelFileName, Action<string> writeToConsole)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(modelFileName);
                XmlElement xmlModel = xDoc.DocumentElement;

                Repo.IModel metamodel = repoModel.Metamodel;

                Repo.INode mainNode = repoModel.Nodes.FirstOrDefault();
                mainNode.Name = "ManagementSystem";
                mainNode.AddAttribute("required", Repo.AttributeKind.Boolean, true.ToString());

                foreach (XmlNode abstractNode in xmlModel)
                {
                    List<Tuple<object, object>> abstractNodeList = null;
                    string abstractAttributeName = abstractNode.Name;
                    if (abstractAttributeName.Equals("xor") || abstractAttributeName.Equals("or") || abstractAttributeName.Equals("and"))
                    {
                        abstractNodeList =
                            ModelLoaderViaXML.FillModelWithGivenConcreteNodes(abstractNode.ChildNodes, repoModel, metamodel, mainNode, abstractAttributeName, isAbstractLink: true);
                    }
                    else
                    {
                        XmlNode attr = abstractNode.Attributes.GetNamedItem("name");

                        Repo.IElement abstractNodeType = metamodel.FindElement("DiagramAbstractNode");
                        Repo.INode diagramAbstractNode = repoModel.CreateElement(abstractNodeType) as Repo.INode;
                        diagramAbstractNode.Name = attr?.Value;

                        Repo.IEdge linkBetweenRootAndAbstractNodes = repoModel.CreateElement("Link") as Repo.IEdge;
                        linkBetweenRootAndAbstractNodes.From = mainNode;
                        linkBetweenRootAndAbstractNodes.To = diagramAbstractNode;

                        var tuple = new Tuple<object, object>(abstractNode, diagramAbstractNode);
                        abstractNodeList = new List<Tuple<object, object>>();
                        abstractNodeList.Add(tuple);
                    }

                    foreach (Tuple<object, object> t in abstractNodeList)
                    {
                        XmlNode abstractNodeFromList = t.Item1 as XmlNode;
                        Repo.INode diagramAbstractNode = t.Item2 as Repo.INode;
                        foreach (XmlNode concreteNodes in abstractNodeFromList.ChildNodes)
                        {
                            foreach (XmlNode concreteNode in concreteNodes.ChildNodes)
                            {
                                string attributeName = concreteNode.Name;
                                if (attributeName.Equals("xor") || attributeName.Equals("or") || attributeName.Equals("and"))
                                {
                                    ModelLoaderViaXML.FillModelWithGivenConcreteNodes(concreteNode.ChildNodes, repoModel, metamodel, diagramAbstractNode, attributeName);
                                    continue;
                                }
                                String concreteNodeName = concreteNode.Attributes.GetNamedItem("name").Value;

                                Repo.IElement concreteNodeType = metamodel.FindElement("DiagramConcreteNode");
                                Repo.INode diagramConcreteNode = repoModel.CreateElement(concreteNodeType) as Repo.INode;
                                diagramConcreteNode.Name = concreteNodeName;

                                Repo.IElement linkType = metamodel.FindElement("Link");
                                Repo.IEdge linkBetweenAbstractAndConcreteNodes = repoModel.CreateElement(linkType) as Repo.IEdge;
                                linkBetweenAbstractAndConcreteNodes.From = diagramAbstractNode;
                                linkBetweenAbstractAndConcreteNodes.To = diagramConcreteNode;

                                foreach (XmlNode concreteNodeItem in concreteNode.ChildNodes)
                                {
                                    if (concreteNodeItem.Name.Equals("selected"))
                                    {
                                        bool value = bool.Parse(concreteNodeItem.InnerText);
                                        diagramConcreteNode.AddAttribute("selected", Repo.AttributeKind.Boolean, value.ToString());
                                    }
                                    if (concreteNodeItem.Name.Equals("required"))
                                    {
                                        bool value = bool.Parse(concreteNodeItem.InnerText);
                                        diagramConcreteNode.AddAttribute("required", Repo.AttributeKind.Boolean, value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                writeToConsole(e.Message);
            }
            return repoModel;
        }

        private static List<Tuple<object, object>> FillModelWithGivenConcreteNodes(
            XmlNodeList logicalOperationConcreteNodes, Repo.IModel repoModel, Repo.IModel metamodel, Repo.INode diagramAbstractNode, 
            string logicalOperationType, bool isAbstractLink = false
            )
        {
            Repo.IElement logicalNodeType = null;
            if (logicalOperationType.Equals("xor"))
            {
                logicalNodeType = metamodel.FindElement("XORConcreteNode");
            }
            else if (logicalOperationType.Equals("or"))
            {
                logicalNodeType = metamodel.FindElement("ORConcreteNode");
            }
            else if (logicalOperationType.Equals("and"))
            {
                logicalNodeType = metamodel.FindElement("ANDConcreteNode");
            }
            else
            {
                throw new Exception("Unknown logical type in XML feature model");
            }

            Repo.INode diagramLogicalNode = repoModel.CreateElement(logicalNodeType) as Repo.INode;
            diagramLogicalNode.Name = logicalOperationType;

            Repo.IElement linkType = metamodel.FindElement("Link");
            Repo.IEdge linkBetweenAbstractAndLogicalNodes = repoModel.CreateElement(linkType) as Repo.IEdge;
            linkBetweenAbstractAndLogicalNodes.From = diagramAbstractNode;
            linkBetweenAbstractAndLogicalNodes.To = diagramLogicalNode;

            var listForReturn = new List<Tuple<object, object>>();
            
            foreach (XmlNode concreteNode in logicalOperationConcreteNodes)
            {
                String concreteNodeName = concreteNode.Attributes.GetNamedItem("name").Value;
                Repo.IElement concreteNodeType = null;

                if (isAbstractLink)
                {
                    concreteNodeType = metamodel.FindElement("DiagramAbstractNode");
                }
                else
                {
                    concreteNodeType = metamodel.FindElement("DiagramConcreteNode");
                }
                Repo.INode diagramConcreteNode = repoModel.CreateElement(concreteNodeType) as Repo.INode;
                diagramConcreteNode.Name = concreteNodeName;

                Repo.IEdge linkBetweenLogicalAndConcreteNodes = repoModel.CreateElement(linkType) as Repo.IEdge;
                linkBetweenLogicalAndConcreteNodes.From = diagramLogicalNode;
                linkBetweenLogicalAndConcreteNodes.To = diagramConcreteNode;
                listForReturn.Add(new Tuple<object, object>(concreteNode, diagramConcreteNode));

                if (!isAbstractLink)
                {
                    foreach (XmlNode concreteNodeItem in concreteNode.ChildNodes)
                    {
                        if (concreteNodeItem.Name.Equals("selected"))
                        {
                            bool value = bool.Parse(concreteNodeItem.InnerText);
                            diagramConcreteNode.AddAttribute("selected", Repo.AttributeKind.Boolean, value.ToString());
                        }
                        if (concreteNodeItem.Name.Equals("required"))
                        {
                            bool value = bool.Parse(concreteNodeItem.InnerText);
                            diagramConcreteNode.AddAttribute("required", Repo.AttributeKind.Boolean, value.ToString());
                        }
                    }
                }
                diagramConcreteNode.AddAttribute(logicalOperationType, Repo.AttributeKind.Boolean, logicalOperationType.ToString());
            }
            return listForReturn;
        }
    }
}
