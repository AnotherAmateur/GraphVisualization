namespace Graph
{
    public class UnDirectedGraph : GeneralGraph
    {
        // базовый конструктор
        public UnDirectedGraph()
        {
            graphDict = new();
        }


        // конструктор-копия
        public UnDirectedGraph(UnDirectedGraph graphToCpy)
        {
            graphDict = new();

            foreach (var item in graphToCpy.graphDict)
            {
                graphDict.Add(item.Key, new(item.Value));
            }
        }


        // добавление ребра
        public override void AddOrUpdateEdge(string fromNode, string toNode, int value)
        {
            if (this.graphDict[fromNode].ContainsKey(toNode))
            {
                this.graphDict[fromNode][toNode] = value;
                this.graphDict[toNode][fromNode] = value;
                return;
            }

            graphDict[fromNode].Add(toNode, value);
            if (fromNode != toNode)
            {
                graphDict[toNode].Add(fromNode, value);
            }
        }


        // удаление ребра
        public override bool RemoveEdge(string fromNode, string toNode)
        {
            if (graphDict[fromNode].ContainsKey(toNode) || graphDict[toNode].ContainsKey(fromNode))
            {
                graphDict[fromNode].Remove(toNode);
                graphDict[toNode].Remove(fromNode);

                return true;
            }

            return false;
        }
    }
}
