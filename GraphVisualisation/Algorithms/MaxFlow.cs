using MyGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;

namespace GraphVisualisation.Algorithms
{
	public static class MaxFlow
	{
		static public GraphVisul form { get; set; }
		static public MyGraph.Graph graph { get; set; }
		static public List<Button> nodes { get; set; }
		static public Pen pen { get; set; }
		static public Panel graphSpace { get; set; }
		static public Point nodeSize { get; set; }

		private static Graphics g;
		private static int delay;

		static int t1;
		static int t2;


		private static void SetLocCorrection()
		{
			t1 = nodeSize.X / 2;
			t2 = nodeSize.Y / 2;
		}


		private static void DrawLine(string v1, string v2)
		{
			Button node1 = nodes.Find(v => v.Name == v1);
			Button node2 = nodes.Find(v => v.Name == v2);

			var startPoint = new Point(node1.Location.X + t1, node1.Location.Y + t2);
			var endPoint = new Point(node2.Location.X + t1, node2.Location.Y + t2);

			g.DrawLine(pen, startPoint, endPoint);
		}


		private static (string, string, bool) GetSourceEffluent()
		{
			if (graph != null)
			{
				// изначально считаем, что все вершины могут быть стоками и истоками
				var potentialSources = graph.graph.graphDict.Keys.ToList();
				var potentialEffluents = graph.graph.graphDict.Keys.ToList();
				// перебор всех дуг, исключение из списков неподходящих вершин
				foreach (string v1 in graph.graph.graphDict.Keys)
				{
					foreach (string v2 in graph[v1].Keys)
					{
						if (graph[v2].ContainsKey(v1))
						{
							return (null, null, false);
						}

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
			}

			return (null, null, false);
		}


		private static int Dfs(string curV, string effluent, MyGraph.Graph net, Stack<string> stack, int minValue, ref int maxFlow, MyGraph.Graph netReversed)
		{
			// обновляем минимальное значение потока
			if (stack.Count > 0)
			{
				DrawLine(stack.Peek(), curV);

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
					Thread.Sleep(delay);

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
					Thread.Sleep(delay);

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


		public static int MaxFlowStart(int delayParam)
		{
			var tmp = GetSourceEffluent();
			string source = tmp.Item1;
			string effluent = tmp.Item2;
			// граф должен быть ориентированный, взвешенный, иметь по одному стоку и истоку
			if (form.IsWeighed && graph.graph is DirectedGraph && tmp.Item3)
			{
				// используем 2 орграфа - копию данного и обратный орграф
				// первый используем для хода в направлении потока,
				// второй - в противоложном
				MyGraph.Graph net = new(graph);
				MyGraph.Graph netReversed = ReverseGraph(net);
				// в стеке запоминаем очередной путь потока
				Stack<string> stack = new();
				int maxFlow = 0;

				SetLocCorrection();
				g = graphSpace.CreateGraphics();
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				delay = delayParam;

				graphSpace_Paint(net, netReversed);
				PrintFlow(0);

				// запускаем модифицированный обход в глубину из истока, пока он не вернет 0
				while (Dfs(source, effluent, net, stack, int.MaxValue, ref maxFlow, netReversed) != 0)
				{
					Thread.Sleep(delay);
					graphSpace.Refresh();
					graphSpace_Paint(net, netReversed);
					PrintFlow(maxFlow);
					stack = new();
				}

				return maxFlow;
			}

			form.DrawEdges = true;
			graphSpace.Refresh();
			return -1;
		}


		// Метод возвращает транспонированную копию графа
		private static MyGraph.Graph ReverseGraph(MyGraph.Graph graph)
		{
			// инициализация нового графа
			MyGraph.Graph graphReversed = new MyGraph.Graph(true);
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


		// Отрисовка связей между вершинами
		private static void graphSpace_Paint(MyGraph.Graph net, MyGraph.Graph netReversed)
		{
			var pen = new Pen(Color.Black, 2);

			foreach (var v1 in graph)
			{
				foreach (var v2 in v1.Value)
				{
					Button node1 = nodes.Find(v => v.Name == v1.Key);
					Button node2 = nodes.Find(v => v.Name == v2.Key);

					var startPoint = new Point(node1.Location.X + t1, node1.Location.Y + t2);
					var endPoint = new Point(node2.Location.X + t1, node2.Location.Y + t2);

					int weightStraight = net[v1.Key][v2.Key];
					int weightGay = netReversed[v2.Key][v1.Key];

					g.DrawLine(pen, startPoint, endPoint);
					g.DrawString($"[{weightStraight.ToString()}, {weightGay.ToString()}]", new Font("Arial", 12), Brushes.DarkRed,
						new Point((int)((startPoint.X + endPoint.X) / 2), (int)((startPoint.Y + endPoint.Y)) / 2));
				}
			}
		}


		// Вывести значение текущего потока
		private static void PrintFlow(int flow)
		{
			g.DrawString($"Max flow: {flow}", new Font("Arial", 11), Brushes.Black,
				new Point(80, 2));
		}


		//private static void DrawString((string, string) v, (int, int) weights)
		//{
		//	Button node1 = nodes.Find(x => x.Name == v.Item1);
		//	Button node2 = nodes.Find(x => x.Name == v.Item2);

		//	var startPoint = new Point(node1.Location.X + t1, node1.Location.Y + t2);
		//	var endPoint = new Point(node2.Location.X + t1, node2.Location.Y + t2);

		//	g.DrawString($"[{weights.Item1.ToString()}, {weights.Item2.ToString()} ]", new Font("Arial", 12), Brushes.DarkRed,
		//		new Point((int)((startPoint.X + endPoint.X) / 2), (int)((startPoint.Y + endPoint.Y)) / 2));
		//}
	}
}
