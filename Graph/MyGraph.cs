using Graph;
using System.Collections;
using System.Collections.Generic;

namespace MyGraph
{
	public class Graph : IEnumerable
	{
		// экземпляр базового класса для ориентированного и неориентированного графов
		public GeneralGraph graph;


		// базовый конструктор
		public Graph(bool isDirected = false)
		{
			if (isDirected)
			{
				graph = new DirectedGraph();
			}
			else
			{
				graph = new UnDirectedGraph();
			}
		}


		// инициализация из файла
		public Graph(string inputPath, bool isDirected = false)
		{
			if (isDirected)
			{
				graph = new DirectedGraph();
			}
			else
			{
				graph = new UnDirectedGraph();
			}

			graph.InitializeFromFile(inputPath);
		}


		// создании копии
		public Graph(Graph graphToCpy)
		{
			if (graphToCpy.graph is DirectedGraph)
			{
				this.graph = new DirectedGraph((DirectedGraph)graphToCpy.graph);
			}
			else
			{
				this.graph = new UnDirectedGraph((UnDirectedGraph)graphToCpy.graph);
			}

			this.graph.isWeighed = graphToCpy.IsWeighed;
		}


		// перегрузка индексатора
		public SortedDictionary<string, int> this[string name]
		{
			get => graph[name];
		}


		// получить enumerator
		IEnumerator IEnumerable.GetEnumerator()
		{
			return graph.graphDict.GetEnumerator();
		}

		public SortedDictionary<string, SortedDictionary<string, int>>.Enumerator GetEnumerator()
		{
			return graph.graphDict.GetEnumerator();
		}


		// добавление вершины
		public void AddNode(string name)
		{
			if (graph.ContainsKey(name))
			{
				Console.WriteLine("Такая вершина уже существует.");
			}
			else
			{
				graph.AddNode(name);
			}
		}


		// реализация метода проверки наличия вершины в Graph
		public bool ContainsKey(string name)
		{
			return graph.ContainsKey(name);
		}


		// добавление/обновление значения ребра, если оно уже существует
		public void AddOrUpdateEdge(string fromNode, string toNode, int value)
		{
			this.graph.AddOrUpdateEdge(fromNode, toNode, value);
		}


		// удаление вершины
		public void DeleteNode(string name)
		{
			if (graph.ContainsKey(name))
			{
				graph.DeleteNode(name);
			}
			else
			{
				Console.WriteLine("\tВершина не найдена");
			}
		}


		// удаление ребра
		public void DeleteEdge(string fromNode, string toNode)
		{
			// проверка наличия данных вершин в графе
			if (graph.ContainsKey(fromNode) && graph.ContainsKey(toNode))
			{
				// метод RemoveEdge базовго класса вернет true, если ребро успешно удалено
				if (graph.RemoveEdge(fromNode, toNode) is false)
				{
					Console.WriteLine("\tРебро не найдено");
				}
			}
			else
			{
				Console.WriteLine("\tВершина(ы) не найдена(ы)");
			}
		}


		// запись графа в файл
		public void RecordFile(string outputFile)
		{
			graph.RecordFile(outputFile);
		}


		// вывод графа на консоль
		public void PrintGraph()
		{
			graph.Print();
		}


		// вернуть кол-во элементов в графе
		public int Count()
		{
			return graph.Count();
		}


		// Свойство, представляющее взвешенность графа
		public bool IsWeighed
		{
			get { return graph.isWeighed; }
			set { graph.isWeighed = value; }
		}


		// Метод возвращает транспонированную копию графа
		private Graph ReverseGraph(Graph graph)
		{
			// инициализация нового графа
			Graph graphReversed = new Graph(true);
			graphReversed.IsWeighed = graph.IsWeighed;

			// заполнение вершинами переданного графа
			foreach (var v in graph)
			{
				graphReversed.AddNode(v.Key);
			}
			//  добавление дуг исходного графа, направленных в обратную сторону
			foreach (var item in graph)
			{
				foreach (var v in item.Value)
				{
					graphReversed.AddOrUpdateEdge(v.Key, item.Key, 0);
				}
			}

			return graphReversed;
		}


		// Генерация полного графа
		private Graph GetCompleteGraph(Graph graph)
		{
			// копируем переданный граф
			Graph result = new(graph);
			result.IsWeighed = true;
			int infinity = int.MaxValue;
			// для каждой вершины добавляются ребра к несмежным вершинам с весом = бесконечность
			foreach (var v1 in result)
			{
				foreach (var vName in graph.graph.graphDict.Keys)
				{
					if (v1.Key != vName && v1.Value.ContainsKey(vName) is false)
					{
						result.AddOrUpdateEdge(v1.Key, vName, infinity);
					}
				}
			}

			return result;
		}


		// Проверить, можно ли из графа 1 получить граф 2 удалением вершин
		public bool ContainsGraph(Graph graph2)
		{
			// в графе 1 вершин должно быть не меньше, чем в 2
			if (graph.Count() < graph2.Count())
				return false;

			// удаление из копии графа 1 всех вершин, не содержащихся в графе 2
			Graph graphCpy = new(this);
			foreach (string v in graph.graphDict.Keys)
			{
				if (graph2.ContainsKey(v))
					continue;
				graphCpy.DeleteNode(v);
			}

			// если полученный граф не содержит какую-то из вершин графа 2 или их ребра отличаются,
			// то утверждение ложно, иначе истинно
			foreach (string v in graph2.graph.graphDict.Keys)
			{
				if (graphCpy.ContainsKey(v) is false || graphCpy[v].SequenceEqual(graph2[v]) is false)
					return false;
			}

			return true;
		}


		// Поиск компонент сильной связности
		public List<List<string>> GetComponenets()
		{
			// список просмотренных вершин
			List<string> used = new();
			// список для хранения порядка просмотра вершин
			List<string> order = new();
			// список найденных компонент сильной связности
			List<List<string>> components = new();

			// запуск рекурсивного обхода в глубиу для каждой пока не просмотренной вершины
			foreach (string v in graph.graphDict.Keys)
			{
				if (!used.Contains(v))
				{
					graph.dfs(v, order, used);
				}
			}

			used = new();
			// инициализация транспонированного графа
			Graph graphT = ReverseGraph(this);
			// серия обходов в глубину для обратного графа из вершин с конца списка order
			for (int i = 0; i < graph.Count(); i++)
			{
				// список вершин одной компоненты
				List<string> component = new();
				string v = order.Last();
				order.Remove(v);

				if (!used.Contains(v))
				{
					// в результате обхода будет получен список вершин одной компоненты сильной связности
					graph.dfsT(graphT.graph.graphDict, v, component, used);
					components.Add(component);
				}
			}

			return components;
		}


		// Нерекурсивный поиск в ширину в радиусе k дуг из данной начальной
		public List<string> BFS(string v, int k)
		{
			// список достигнутых вершин
			List<string> result = new();

			if (graph.ContainsKey(v) is false)
			{
				Console.WriteLine("Вершина не существует");
			}
			else
			{
				// список просмотренных вершин
				List<string> used = new();
				used.Add(v);
				// в очередь складываются вершины, смежные просмотренным, для следующей итерации
				Queue<string> queue = new();
				queue.Enqueue(v);
				for (int i = 0; i < k; i++)
				{
					// временная очередь 2, в которую кладем смежные вершины
					Queue<string> queueTmp = new();
					// проход по всем ребрам в очереди 1
					foreach (var item1 in queue)
					{
						foreach (var item2 in graph[item1])
						{
							// все просмотренные вершины пропускаем 
							if (used.Contains(item2.Key) is false)
							{
								used.Add(item2.Key);
								result.Add(item2.Key);
							}

							queueTmp.Enqueue(item2.Key);
						}
					}

					// конец, если непросмотренных вершин, доступных из начальной, не осталось
					if (queueTmp.Count == 0)
						break;

					queue = queueTmp;
				}
			}

			return result;
		}


		// Проверка графа на соответствие требованиям для поиска остова
		private bool CheckTermsCarcass()
		{
			bool isConnected = true;
			// если вес ребра равен 0, то ребра как бы нет и граф не является связным
			foreach (var item in this)
			{
				if (item.Value.Count == 0)
				{
					isConnected = false;
					break;
				}
			}

			// граф должен быть взвешенным, неориентированным, связным
			return graph.isWeighed && graph is UnDirectedGraph && isConnected;
		}

		// Алгоритм Прима
		public void GetCarcass()
		{
			// проверка графа на соответствие требованиям для поиска остова
			if (CheckTermsCarcass() is false)
			{
				Console.WriteLine("Невозможно найти остов в данном графе");
				return;
			}

			// новый пустой граф, который в итоге должен будет являться остовом
			Graph carcass = new();
			carcass.IsWeighed = true;
			// добавляем к остову случайную вершину исходного графа
			string randomV = this.graph.graphDict.Keys.First();
			carcass.AddNode(randomV);

			// переменная хранит общий вес остовного дерева
			int weight = 0;
			// выполняем цикл, пока в остове не будут добавлены все вершины исходного графа
			while (carcass.Count() < this.Count())
			{
				// перебор всех вершин, смежных с любой вершиной остова
				var minEdge = ("none", "none", int.MaxValue);
				foreach (var v1 in carcass)
				{
					foreach (var v2 in this[v1.Key])
					{
						// ищем ребро с минимальным весом
						if (carcass.ContainsKey(v2.Key) is false && minEdge.Item3 > v2.Value)
						{
							minEdge = (v1.Key, v2.Key, v2.Value);
						}
					}
				}

				// в остов добавляется вершина исходного графа с минимальной стоимостью пути из остова
				carcass.AddNode(minEdge.Item2);
				carcass.AddOrUpdateEdge(minEdge.Item1, minEdge.Item2, minEdge.Item3);
				weight += minEdge.Item3;
			}

			carcass.PrintGraph();
			Console.WriteLine("Вес остовного дерева: " + weight);
		}


		// Алгоритм Флойда
		public List<string> GetFloyd(string[] vertexes)
		{
			// коллекция вершин для восстановления минимального пути
			Dictionary<string, Dictionary<string, string>> ancestors = new();
			// полный граф из исходного,
			// содержащий в итоге стоимости кратчайших путей между всеми парами вершин
			Graph completeGraph = GetCompleteGraph(this);
			completeGraph.IsWeighed = true;

			// инициализация списка предков, изначально считаем, что предков нет
			foreach (var item in this.graph.graphDict.Keys)
			{
				ancestors.Add(item, new());
				foreach (var item2 in this.graph.graphDict.Keys)
				{
					ancestors[item].Add(item2, null);
				}
			}

			// для каждой вершины графа перебор всех пар вершин графа
			// совпадающие вершины не рассматриваются
			foreach (var v1 in completeGraph)
			{
				foreach (var v2 in completeGraph)
				{
					if (v1.Key == v2.Key)
						continue;

					foreach (var v3 in completeGraph)
					{
						if (v2.Key == v3.Key || v1.Key == v3.Key)
							continue;

						// если стоимости ребер предполагаемого кратчайшего пути не равны "бесконечности"
						// и стоимость текущего из v2 в v3 пути больше стоимости пути, проходящего через вершину v1,
						// стоимость пути из v2 в v3 считаем как v2 + v1 + v3
						if (completeGraph[v2.Key][v1.Key] < int.MaxValue && completeGraph[v1.Key][v3.Key] < int.MaxValue &&
							completeGraph[v2.Key][v3.Key] > completeGraph[v2.Key][v1.Key] + completeGraph[v1.Key][v3.Key])
						{
							completeGraph.AddOrUpdateEdge(v2.Key, v3.Key, completeGraph[v2.Key][v1.Key] + completeGraph[v1.Key][v3.Key]);
							ancestors[v2.Key][v3.Key] = v1.Key;
						}
					}
				}
			}

			// лист хранит строку, содержащую кратчайший путь и стоимость этого пути
			List<string> result = new();
			for (int i = 1; i < vertexes.Length; i++)
			{
				// восстановление кратчайшего пути
				Queue<string> way = new();
				way.Enqueue(vertexes[0]);
				WayFloyd(vertexes[0], vertexes[i], ancestors, way);
				way.Enqueue(vertexes[i]);

				// если начальная и конечная вершины совпадают, то считаем, что пути нет
				if (vertexes[0] == vertexes[i])
					result.Add(string.Join(" - ", way.ToArray()) + " : ни путю");
				else
				{
					int dist = completeGraph[vertexes[0]][vertexes[i]];
					result.Add(string.Join(" - ", way.ToArray()) + " : " + (dist < int.MaxValue ? dist : "ни путю"));
				}
			}

			return result;
		}


		// рекурсивный алгоритм восстановления пути между парой вершин в алгоритме Флойда
		public void WayFloyd(string u, string v, Dictionary<string, Dictionary<string, string>> ancestors, Queue<string> way)
		{
			// получаем "среднюю" вершину для u и v
			string tmp = ancestors[u][v];

			// восстанавливаем путь от u до "средней", затем от "средней" до v,
			// сохраняя пройденные вершины в очередь
			if (tmp != null)
			{
				WayFloyd(u, tmp, ancestors, way);
				way.Enqueue(tmp);
				WayFloyd(tmp, v, ancestors, way);
			}
		}


		// Алгоритм Дейкстры
		public List<(List<string>, int)> GetDijkstra(int n)
		{
			// полный граф из исходного
			Graph completeGraph = GetCompleteGraph(this);
			double eps = 1e-10;

			foreach (string s in this.graph.graphDict.Keys)
			{
				// коллекция для расстояний от начальной вершины до всех остальных
				Dictionary<string, int> dist = new();
				// коллекция для сохранения кратчайших путей
				Dictionary<string, string> path = new();
				List<string> notUsed = new();

				// инициализация начальных параметров
				foreach (string v in this.graph.graphDict.Keys)
				{
					if (v != s)
					{
						dist.Add(v, completeGraph[s][v]);
						notUsed.Add(v);
						path[v] = s;
					}
				}

				// нахождение минимальных путей
				for (int i = 1; i < this.Count(); i++)
				{
					string v = notUsed.MinBy(v => dist[v]);
					notUsed.Remove(v);

					foreach (string w in notUsed)
					{
						if (dist[w] > dist[v] + completeGraph[v][w] + eps)
						{
							dist[w] = dist[v] + completeGraph[v][w];
							path[w] = v;
						}
					}
				}

				// проверка условий задачи
				if (dist.Max(x => x.Value) <= n)
				{
					List<(List<string>, int)> res = new();
					foreach (string v in this.graph.graphDict.Keys)
					{
						if (v != s)
						{
							List<string> tmp = new();
							string w = v;
							do
							{
								tmp.Add(w);
								w = path[w];
							} while (s != w);

							tmp.Add(s);
							tmp.Reverse();
							res.Add((tmp, dist[v]));
						}
					}

					return res;
				}
			}

			return null;
		}


		// Алгоритм Белмана-Форда возвращает коллекцию пар [город, расстояние до него из S]
		public Dictionary<string, string> BelFord(string s)
		{
			// dist - коллекция для хранения минимальных расстояний от начальной вершины до всех остальных
			Dictionary<string, int> dist = new();
			int infinityPos = int.MaxValue;
			int infinityNeg = int.MinValue;
			// изначально расстояние до начальной вершины = 0, до остальных = бесконечности
			foreach (var item in this.graph.graphDict.Keys)
			{
				dist.Add(item, infinityPos);
			}
			dist[s] = 0;

			// просматриваем список ребер n-1 раз,
			// после чего будут найдены минимальные расстояния (при условии отсутствия циклов отрицательного веса)
			for (int i = 1; i < graph.Count(); i++)
			{
				foreach (string v in this.graph.graphDict.Keys)
				{
					foreach (var edge in this[v])
					{
						// исключаем рассмотрение бесконечно больших новых расстояний
						if (dist[v] != infinityPos && edge.Value != infinityPos)
						{
							// обновляем расстояние до праавой вершины ребра в коллекции dist,
							// если оно больше расстония до левой вершины ребра + вес ребра
							if (dist[edge.Key] > dist[v] + edge.Value)
							{
								dist[edge.Key] = dist[v] + edge.Value;
							}
						}
					}
				}
			}
			// совершаем еще один проход по списку ребер
			foreach (string v in this.graph.graphDict.Keys)
			{
				foreach (var edge in this[v])
				{
					if (dist[edge.Key] != infinityPos && dist[v] != infinityPos && edge.Value != infinityPos)
					{
						// если расстояние возможно уменьшить еще больше, обнаружен цикл на пути, помечаем его
						if (dist[edge.Key] > dist[v] + edge.Value)
						{
							dist[edge.Key] = infinityNeg;
						}
					}
				}
			}

			// приводим результаты к презентабельному виду
			Dictionary<string, string> result = new();
			foreach (var item in dist)
			{
				if (item.Value == infinityNeg)
				{
					result.Add(item.Key, "Infinity");
				}
				else if (item.Value == infinityPos)
				{
					result.Add(item.Key, "—");
				}
				else
				{
					result.Add(item.Key, item.Value.ToString());
				}
			}

			return result;
		}


		// Поиск стока-истока
		private (string, string, bool) GetSourceEffluent()
		{
			// изначально считаем, что все вершины могут быть стоками и истоками
			var potentialSources = graph.graphDict.Keys.ToList();
			var potentialEffluents = graph.graphDict.Keys.ToList();
			// перебор всех дуг, исключение из списков неподходящих вершин
			foreach (string v1 in graph.graphDict.Keys)
			{
				foreach (string v2 in graph[v1].Keys)
				{
					potentialEffluents.Remove(v1);

					if (potentialSources.Contains(v2))
					{
						potentialSources.Remove(v2);
					}
				}
			}
			// возвращаем вершины, если найдено по 1 стоку и истоку
			if (potentialSources.Count == 1 && potentialEffluents.Count == 1)
			{
				return (potentialSources[0], potentialEffluents[0], true);
			}

			return (null, null, false);
		}

		// модифицированный обход в глубину
		private int Dfs(string curV, string effluent, Graph net, Stack<string> stack, int minValue, ref int maxFlow, Graph netReversed)
		{
			// обновляем минимальное значение потока
			if (stack.Count > 0)
			{
				if (net[stack.Peek()].ContainsKey(curV))
				{
					minValue = Math.Min(minValue, net[stack.Peek()][curV]);
				}
				else
				{
					minValue = Math.Min(minValue, netReversed[stack.Peek()][curV]);
				}
			}

			// проверка достижения стока
			if (curV == effluent)
			{
				maxFlow += minValue;
				return minValue;
			}

			stack.Push(curV);

			int maxLocalFlow;
			string maxLocalFlowVertex;
			int edgesCount = net[curV].Count();

			// поиск пути в направлении потока, перебор смежных вершин
			for (int i = 0; i < edgesCount; i++)
			{
				maxLocalFlow = 0;
				maxLocalFlowVertex = null;
				// поиск дуги максимального веса
				foreach (string v in net[curV].Keys)
				{
					if (net[curV][v] > maxLocalFlow && stack.Contains(v) is false)
					{
						maxLocalFlow = net[curV][v];
						maxLocalFlowVertex = v;
					}
				}
				if (maxLocalFlow > 0)
				{
					// найдена ненасыщенная дуга, рекурсивно вызываем обход в глубину
					int tmp = Dfs(maxLocalFlowVertex, effluent, net, stack, minValue, ref maxFlow, netReversed);
					if (tmp > 0)
					{
						// если был возвращен не 0, значит, сток был достигнут, поэтому обновляем метки на дугах
						net.AddOrUpdateEdge(curV, maxLocalFlowVertex, net[curV][maxLocalFlowVertex] - tmp);
						netReversed.AddOrUpdateEdge(maxLocalFlowVertex, curV, netReversed[maxLocalFlowVertex][curV] + tmp);
						stack.Pop();
						return tmp;
					}

					continue;
				}

				break;
			}

			// поиск пути в направлении, противоположному потоку, перебор смежных вершин для обратного графа
			edgesCount = netReversed[curV].Count();
			for (int i = 0; i < edgesCount; i++)
			{
				maxLocalFlow = 0;
				maxLocalFlowVertex = null;
				// поиск обратной дуги максимального веса
				foreach (string v in netReversed[curV].Keys)
				{
					if (netReversed[curV][v] > maxLocalFlow && stack.Contains(v) is false)
					{
						maxLocalFlow = netReversed[curV][v];
						maxLocalFlowVertex = v;
					}
				}
				if (maxLocalFlow > 0)
				{
					// найдена ненасыщенная дуга, рекурсивно вызываем обход в глубину
					int tmp = Dfs(maxLocalFlowVertex, effluent, net, stack, minValue, ref maxFlow, netReversed);
					if (tmp > 0)
					{
						// если был возвращен не 0, значит, сток был достигнут, поэтому обновляем метки на дугах
						net.AddOrUpdateEdge(maxLocalFlowVertex, curV, net[maxLocalFlowVertex][curV] + tmp);
						netReversed.AddOrUpdateEdge(curV, maxLocalFlowVertex, netReversed[curV][maxLocalFlowVertex] - tmp);
						stack.Pop();
						return tmp;
					}

					continue;
				}

				break;
			}

			// если мы здесь, значит, из текущей вершины невозможно попасть в сток,
			// поэтому исключаем ее из рассмотрения
			net.DeleteNode(curV);
			netReversed.DeleteNode(curV);
			stack.Pop();

			return 0;
		}

		// Поиск максимального потка - алгоритм Форда-Фалкерсона
		// алгоритм вернет -1, если граф имеет неверный формат,
		// иначе вернет величину максимального потока
		public int GetMaxFlow()
		{
			var tmp = GetSourceEffluent();
			string source = tmp.Item1;
			string effluent = tmp.Item2;
			// граф должен быть ориентированный, взвешенный, иметь по одному стоку и истоку
			if (graph.isWeighed && graph is DirectedGraph && tmp.Item3)
			{
				// используем 2 орграфа - копию данного и обратный орграф
				// первый используем для хода в направлении потока,
				// второй - в противоложном
				Graph net = new(this);
				Graph netReversed = ReverseGraph(net);
				// в стеке запоминаем очередной путь потока
				Stack<string> stack = new();
				int maxFlow = 0;

				// запускаем модифицированный обход в глубину из истока, пока он не вернет 0
				while (Dfs(source, effluent, net, stack, int.MaxValue, ref maxFlow, netReversed) != 0)
				{
					stack = new();
				}

				return maxFlow;
			}

			return -1;
		}
	}
}
