namespace Graph
{
    public class DirectedGraph : GeneralGraph
    {
        // базовый конструктор
        public DirectedGraph()
        {
            graphDict = new();
        }


        // конструктор-копия
        public DirectedGraph(DirectedGraph graphToCpy)
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
                return;
            }

            this.graphDict[fromNode].Add(toNode, value);
        }


        // удаление ребра
        public override bool RemoveEdge(string fromNode, string toNode)
        {
                return graphDict[fromNode].Remove(toNode);
        }
    }
}
