namespace MyGraph
{
    public class UserInterface
    {
        static public void IsWeighed(Graph graph)
        {
            Console.WriteLine("Граф взешенный? Y/N: ");
            graph.IsWeighed = char.ToUpper(Convert.ToChar(Console.ReadLine())) == 'Y' ? true : false;
        }


        static public void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine(" 1. Создать пустой ориентированный граф");
            Console.WriteLine(" 2. Создать пустой неориентированный граф");
            Console.WriteLine(" 3. Создать ориентированный граф из файла");
            Console.WriteLine(" 4. Создать неориентированный граф из файла");
            Console.WriteLine(" 5. Добавить ребро");
            Console.WriteLine(" 6. Удалить ребро");
            Console.WriteLine(" 7. Добавить вершину");
            Console.WriteLine(" 8. Удалить вершину");
            Console.WriteLine(" 9. Вывести граф в консоль");
            Console.WriteLine(" 0. Вывести граф в файл");
            Console.WriteLine(" 22. Показать доступные действия");
            Console.WriteLine(" 11. Завершить");
            Console.WriteLine();
        }


        static public void StartUserInterface(Graph graph, bool oughtToBeOriented)
        {
            string inputFile = "D:/VisualProj/Graph/input.txt";
            string outputFile = "D:/VisualProj/Graph/output.txt";

            //Graph graph = new();
            bool isInitialized = true;
            bool isDirected = oughtToBeOriented;

            PrintInfo();
            while (true)
            {
                int inputStr = int.Parse(Console.ReadLine());
                switch (inputStr)
                {
                    case 1:
                        graph = new(true);
                        isInitialized = true;
                        IsWeighed(graph);
                        break;
                    case 2:
                        if (oughtToBeOriented)
                        {
                            Console.WriteLine("Граф должен быть оринтированным");
                            break;
                        }
                        graph = new();
                        isInitialized = true;
                        IsWeighed(graph);
                        break;
                    case 3:
                        graph = new(inputFile, true);
                        isInitialized = true;
                        IsWeighed(graph);
                        break;
                    case 4:
                        if (oughtToBeOriented)
                        {
                            Console.WriteLine("Граф должен быть оринтированным");
                            break;
                        }
                        graph = new(inputFile);
                        isInitialized = true;
                        IsWeighed(graph);
                        break;
                    case 5:
                        if (isInitialized)
                        {
                            if (graph.Count() == 0)
                            {
                                Console.WriteLine("\tНедостаточно вершин в графе");
                            }
                            else
                            {
                                if (graph.IsWeighed)
                                {
                                    Console.Write("\tВершина 1, вершина 2, значение: ");
                                    string[] tmp = Console.ReadLine().Split(' ');
                                    if (tmp.Length != 3)
                                    {
                                        Console.Write("\tНеверный формат ");
                                    }
                                    else
                                    {
                                        graph.AddOrUpdateEdge(tmp[0], tmp[1], int.Parse(tmp[2]));
                                    }
                                }
                                else
                                {
                                    Console.Write("\tВершина 1, вершина 2: ");
                                    string[] tmp = Console.ReadLine().Split(' ');
                                    if (tmp.Length != 2)
                                    {
                                        Console.Write("\tНеверный формат ");
                                    }
                                    else
                                    {
                                        graph.AddOrUpdateEdge(tmp[0], tmp[1], 1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\tГраф не создан");
                        }
                        break;
                    case 6:
                        if (isInitialized)
                        {
                            if (graph.Count() == 0)
                            {
                                Console.WriteLine("\tНет ребер");
                            }
                            else
                            {
                                Console.Write("\tВершина 1, вершина 2: ");
                                string[] tmp = Console.ReadLine().Split(' ');
                                graph.DeleteEdge(tmp[0], tmp[1]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\tГраф не создан");
                        }
                        break;
                    case 7:
                        if (isInitialized)
                        {
                            Console.Write("\tВершина 1: ");
                            string tmp = Console.ReadLine();
                            graph.AddNode(tmp);
                        }
                        else
                        {
                            Console.WriteLine("\tГраф не создан");
                        }
                        break;
                    case 8:
                        if (isInitialized)
                        {
                            if (graph.Count() == 0)
                            {
                                Console.WriteLine("\tГраф пустой");
                            }
                            else
                            {
                                Console.Write("\tВершина 1: ");
                                string tmp = Console.ReadLine();
                                graph.DeleteNode(tmp);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\tГраф не создан");
                        }
                        break;
                    case 9:
                        if (isInitialized)
                        {
                            graph.PrintGraph();
                        }
                        else
                        {
                            Console.WriteLine("\tГраф не создан");
                        }
                        break;
                    case 0:
                        if (isInitialized)
                        {
                            graph.RecordFile(outputFile);
                        }
                        else
                        {
                            Console.WriteLine("\tГраф не создан");
                        }
                        break;
                    case 22:
                        PrintInfo();
                        break;
                    case 11:
                        return;
                    default:
                        Console.WriteLine("\tНичё не понятно");
                        break;
                }
            }
        }
    }
}
