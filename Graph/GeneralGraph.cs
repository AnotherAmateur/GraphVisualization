using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace Graph
{
    // Базовый класс
    abstract public class GeneralGraph : IGraph
    {
        // коллекция вершин/ребер
        public SortedDictionary<string, SortedDictionary<string, int>> graphDict;
        public bool isWeighed;


        // рекурсивный обход графа в глубину
        public void dfs(string v, List<string> order, List<string> used)
        {
            // вершина помечается просмотренной
            used.Add(v);

            // проход по еще непросмотренным вершинам в цикле 
            foreach (string item in graphDict[v].Keys)
            {
                if (!used.Contains(item))
                {
                    dfs(item, order, used);
                }
            }

            // сохранение порядка просмотра вершин
            order.Add(v);
        }


		// рекурсивный обход транспонированного графа в глубину
		public void dfsT(SortedDictionary<string, SortedDictionary<string, int>> graphT, 
                         string v, List<string> component, List<string> used)
        {
			// вершина помечается просмотренной
			used.Add(v);
			// вершина помечается просмотренной
			component.Add(v);

            foreach (string item in graphT[v].Keys)
            {
                if (!used.Contains(item))
                {
                    dfsT(graphT, item, component, used);
                }
            }
        }


        // проверить, существует ли вершина
        public bool ContainsKey(string name)
        {
            return graphDict.ContainsKey(name);
        }


        // вернуть кол-во элементов
        public int Count()
        {
            return graphDict.Count();
        }


        // инициализация из файла списком смежности
        public void InitializeFromFile(string inputPath)
        {
            graphDict = new SortedDictionary<string, SortedDictionary<string, int>>();

            using (StreamReader sr = new StreamReader(inputPath))
            {
                int count = int.Parse(sr.ReadLine());

                for (int i = 0; i < count; i++)
                {
                    string[] str = sr.ReadLine().Split('/', StringSplitOptions.RemoveEmptyEntries);
                    graphDict.Add(str[0], new SortedDictionary<string, int>());
               
                    for (int j = 1; j < str.Length; j++)
                    {
                        string[] node = str[j].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        graphDict[str[0]].Add(node[0], int.Parse(node[1]));
                    }
                }
            }
        }


        // добавление вершины
        public void AddNode(string name)
        {
            graphDict.Add(name, new());
        }


        // добавление ребра
        abstract public void AddOrUpdateEdge(string fromNode, string toNode, int value);


        // удаление ребра
        abstract public bool RemoveEdge(string fromNode, string toNode);


        // удаление вершины
        public void DeleteNode(string name)
        {
            graphDict.Remove(name);

            foreach (var item in graphDict)
            {
                if (graphDict[item.Key].ContainsKey(name))
                {
                    graphDict[item.Key].Remove(name);
                }
            }
        }


        // запись графа в файл
        public void RecordFile(string outputFile)
        {
            using (StreamWriter sr = new StreamWriter(outputFile))
            {
                sr.WriteLine(graphDict.Count);

                if (isWeighed)
                {
                    foreach (var item in graphDict)
                    {
                        sr.Write(item.Key);

                        foreach (var node in item.Value)
                        {
                            sr.Write("/" + node.Key + " " + node.Value);
                        }

                        sr.WriteLine();
                    }
                }
                else
                {
                    foreach (var item in graphDict)
                    {
                        sr.Write(item.Key);

                        foreach (var node in item.Value)
                        {
                            sr.Write("/" + node.Key);
                        }

                        sr.WriteLine();
                    }
                }
            }
        }


        // вывод графа на консоль
        public void Print()
        {
            Console.WriteLine();
            int i = 1;

            if (isWeighed)
            {
                foreach (var item in graphDict)
                {
                    Console.Write(i++ + ". " + item.Key);

                    foreach (var node in item.Value)
                    {
                        Console.Write("/" + node.Key + " " + node.Value);
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                foreach (var item in graphDict)
                {
                    Console.Write(i++ + ". " + item.Key);

                    foreach (var node in item.Value)
                    {
                        Console.Write("/" + node.Key);
                    }

                    Console.WriteLine();
                }
            }
        }


        // перегрузка индексатора
        public SortedDictionary<string, int> this[string name]
        {
            get => new(graphDict[name]);
        }
    }
}
