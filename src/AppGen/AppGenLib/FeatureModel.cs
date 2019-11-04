using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGen.AppGenLib
{
    public class FeatureModel
    {
        public readonly Dictionary<string, bool> Features = new Dictionary<string, bool>();

        public FeatureModel()
        {
        }
        
        public FeatureModel LoadFeatureModelByRepoModel(Repo.IModel repoModel, Action<string> writeToConsole)
        {
            List<Repo.INode> nodeList = repoModel.Nodes.ToList();
            List<Repo.IEdge> edgeList = repoModel.Edges.ToList();

            Repo.INode mainNode = nodeList.Find(x => x.Name == "ManagementSystem");
            try
            {
                this.RecursiveIteration(mainNode, nodeList, edgeList);
            }
            catch (Exception e)
            {
                if (nodeList.Count != 6)
                {
                    writeToConsole(e.Message);
                    throw new Exception("Not all of the features readed");
                }
                writeToConsole(e.Message);
            }

            return this;
        }

        private int RecursiveIteration(Repo.INode currNode, List<Repo.INode> nodeList, List<Repo.IEdge> edgeList)
        {
            List<Repo.IEdge> currEdgeList = edgeList.FindAll(x => x.From == currNode);

            if (currEdgeList.Count == 0)
            {
                Repo.IAttribute isAbstractAsAFeature = currNode.Attributes.ToList().Find(x => x.Name == "isAbstractAsAFeature");
                if (Boolean.Parse(isAbstractAsAFeature.StringValue))
                {
                    return 0;
                }

                Repo.IAttribute selectedAttr = currNode.Attributes.ToList().Find(x => x.Name == "selected");
                bool isSelected = Boolean.Parse(selectedAttr.StringValue);
                Features.Add(currNode.Name, isSelected);
                return isSelected ? 1 : 0;
            }
            if (currNode.Name.Equals("xor") || currNode.Name.Equals("or") || currNode.Name.Equals("and"))
            {
                int trueCount = 0;
                foreach (Repo.IEdge edge in currEdgeList)
                {
                    Repo.INode nextNode = edge.To as Repo.INode;
                    int selectedCount = this.RecursiveIteration(nextNode, nodeList, edgeList) > 0 ? 1 : 0;
                    trueCount += selectedCount;
                    if (trueCount > 1 && currNode.Name.Equals("xor"))
                    {
                        throw new Exception("Only one attribute in XOR operation should be chosen.");
                    }
                }
                if (trueCount < currEdgeList.Count && currNode.Name.Equals("and"))
                {
                    throw new Exception("All nodes in AND operation should be chosen.");
                }
                if (trueCount == 0 && (currNode.Name.Equals("xor") || currNode.Name.Equals("and")))
                {
                    throw new Exception("At least on node in XOR operations should be chosen.");
                }
                return trueCount;
            }
            else
            {
                int selectedCount = 0;
                foreach (Repo.IEdge edge in currEdgeList)
                {
                    Repo.INode nextNode = edge.To as Repo.INode;
                    selectedCount += this.RecursiveIteration(nextNode, nodeList, edgeList) > 0 ? 1 : 0;
                }
                return selectedCount;
            }
            return 100500;
        }
    }
}
