namespace Graph
{
    public interface IGraph
    {
        // добавление вершины
        public void AddNode(string name);

        // добавление/обновление ребра
        public void AddOrUpdateEdge(string fromNode, string toNode, int value);

        // удаление вершины
        public void DeleteNode(string name);

        // запись графа в файл
        public void RecordFile(string outputFile);

        // вывод графа на консоль
        public void Print();

        // проверить, существует ли вершина
        public bool ContainsKey(string name);

        // вернуть кол-во элементов
        public int Count();

        // удаление ребра
        public bool RemoveEdge(string fromNode, string toNode);
    }
}
