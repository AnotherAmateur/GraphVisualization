using MyGraph;
using GraphVisualisation;
using System.Drawing;
using System.Net;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace GraphVisualisation.Algorithms
{
	public static class BFS
	{
		static public GraphVisul form { get; set; }
		static public MyGraph.Graph graph { get; set; }
		static public List<Button> nodes { get; set; }
		static public Pen pen { get; set; }
		static public Graphics g { get; set; }
		static public Point nodeSize { get; set; }

		static int t1;
		static int t2;


		private static void SetLocCorrection()
		{
			t1 = nodeSize.X / 2;
			t2 = nodeSize.Y / 2;
		}

		private async static void DrawLine(string v1, string v2, int ik)
		{
			Thread.Sleep(1200 * k);

			Button node1 = nodes.Find(v => v.Name == v1);
			Button node2 = nodes.Find(v => v.Name == v2);

			var startPoint = new Point(node1.Location.X + t1, node1.Location.Y + t2);
			var endPoint = new Point(node2.Location.X + t1, node2.Location.Y + t2);

			g.DrawLine(pen, startPoint, endPoint);
		}

		private static void DrawString(string v, int number)
		{
			Button node = nodes.Find(x => x.Name == v);

			g.DrawString("(" + number.ToString() + ")", new Font("Arial", 12), Brushes.Blue,
				new Point(node.Location.X, node.Location.Y + nodeSize.Y));
		}

		public static List<string> StartBFS(string v)
		{
			SetLocCorrection();

			List<string> result = new();
			List<string> used = new();
			Queue<string> queue = new();

			used.Add(v);
			queue.Enqueue(v);
			DrawString(v, 0);

			int k = 1;
			for (int i = 0; i < graph.Count(); i++)
			{
				Queue<string> queueTmp = new();
				foreach (var v1 in queue)
				{
					foreach (var v2 in graph[v1])
					{
						if (used.Contains(v2.Key) is false)
						{
							DrawLine(v1, v2.Key, k);
							DrawString(v2.Key, k);
							used.Add(v2.Key);
							result.Add(v2.Key);
						}

						queueTmp.Enqueue(v2.Key);
					}
				}

				if (queueTmp.Count == 0)
					break;

				queue = queueTmp;
			}

			return result;
		}
	}
}
