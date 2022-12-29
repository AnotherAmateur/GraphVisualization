using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using GraphVisualisation.Algorithms;

namespace GraphVisualisation
{
	public partial class GraphVisul : Form
	{
		int nodeCount;
		private List<Button> nodes;
		MyGraph.Graph? graph;
		bool isDirected;
		bool isWeighed;
		bool addNodeSelected;
		bool delNodeSelected;
		bool isNewGraph;
		bool addEdgeSelected;
		bool BFSSelected;
		public bool DrawEdges { get; set; }
		string? edgeToAddFirst;
		readonly Point nodeSize;
		int delay;
		public string InfoBox
		{
			get { return infoBox.Text; }
			set { infoBox.Text = value; }
		}
		public bool IsWeighed
		{
			get { return isWeighed; }
		}

		EdgeEditBox edgeEditBoxForm;

		// Конструктор формы
		public GraphVisul(EdgeEditBox edgeEditBox)
		{
			nodes = new();
			isNewGraph = true;
			DrawEdges = true;

			InitializeComponent();

			nodeSize = new(35, 35);
			edgeEditBoxForm = edgeEditBox;
			edgeEditBoxForm.StartPosition = FormStartPosition.Manual;
			delay = 1250;

			typeof(Panel).InvokeMember("DoubleBuffered",
			BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
			null, graphSpace, new object[] { true });
		}


		// Обработка клика мышью на поле отрисовки графа 
		private void graphSpace_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// блок добавления вершин в класс графа и поле отрисовки
				if (addNodeSelected)
				{
					++nodeCount;

					var clickLocation = e.Location;
					var newNode = new Button();
					newNode.Location = new Point(clickLocation.X - nodeSize.X / 2, clickLocation.Y - nodeSize.Y / 2);
					newNode.Size = new(nodeSize.X, nodeSize.Y);
					newNode.Text = nodeCount.ToString();
					newNode.BackColor = Color.MediumVioletRed;
					newNode.Font = new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Regular);
					newNode.Name = new string(nodeCount.ToString());
					graphSpace.Controls.Add(newNode);
					newNode.MouseDown += new MouseEventHandler(onNodeBtn_MouseDown);
					newNode.MouseMove += new MouseEventHandler(node_MouseMove);
					newNode.Click += new EventHandler(node_MouseClick);

					if (isNewGraph)
					{
						graph = new(isDirected);
						graph.IsWeighed = isWeighed;

						isNewGraph = false;
					}

					graph.AddNode(newNode.Name);
					nodes.Add(newNode);
				}
			}
		}


		// Обработка левого клика по вершине
		private void node_MouseClick(object? sender, EventArgs e)
		{
			// удаление вершины
			if (delNodeSelected)
			{
				Button buttonToRemove = nodes.Find(x => x == sender);

				graph.DeleteNode(buttonToRemove.Name);
				nodes.Remove(buttonToRemove);
				graphSpace.Controls.Remove(buttonToRemove);

				graphSpace.Refresh();
			}
			// редактирование ребер/дуг
			else if (addEdgeSelected)
			{
				if (edgeToAddFirst == null)
				{
					edgeToAddFirst = ((Button)sender).Name;
					InfoBox = "Теперь укажите вторую вершину";
				}
				else
				{
					string edgeToAddSecond = ((Button)sender).Name;

					int weight = (isWeighed && graph[edgeToAddFirst].ContainsKey(edgeToAddSecond)) ? graph[edgeToAddFirst][edgeToAddSecond] : 0;

					edgeEditBoxForm.Weight = weight;
					edgeEditBoxForm.InfoBox = "Укажите вес";


					edgeEditBoxForm.Location = new Point(this.Location.X + (int)(this.Width / 2 - edgeEditBoxForm.Width / 2), this.Location.Y + (int)(this.Height / 2 - edgeEditBoxForm.Height / 2));
					edgeEditBoxForm.ShowDialog();

					weight = edgeEditBoxForm.Weight;

					if (edgeEditBoxForm.EdgeAdded)
					{
						graph.AddOrUpdateEdge(edgeToAddFirst, edgeToAddSecond, weight);
					}
					else
					{
						graph.DeleteEdge(edgeToAddFirst, edgeToAddSecond);
					}

					graphSpace.Refresh();
					edgeToAddFirst = null;
					InfoBox = "Укажите первую вершину";
				}
			}
			// Визуализация обхода в ширину
			else if (BFSSelected)
			{
				InfoBox = "Выполняется...";
				Refresh();

				Graphics g = this.graphSpace.CreateGraphics();
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				string vertex = ((Button)sender).Name;

				BFS.graph = graph;
				BFS.pen = new Pen(Color.MediumVioletRed, 4);
				BFS.g = g;
				BFS.nodes = nodes;
				BFS.nodeSize = nodeSize;
				List<string> result = BFS.StartBFS(vertex, delay);

				var tmp = new List<string>() { vertex };
				tmp.AddRange(result);
				InfoBox = $"Порядок обхода: {string.Join(", ", tmp)}"; ;
			}
		}


		// Перемещение вершины зажатой левой кнопкой мыши
		private void node_MouseMove(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && addNodeSelected)
			{
				Button buttonToMove = nodes.Find(x => x == sender);

				Point mousePos = buttonToMove.PointToClient(Cursor.Position);

				int newX = buttonToMove.Left + mousePos.X - (buttonToMove.Width / 2);
				int newY = buttonToMove.Top + mousePos.Y - (buttonToMove.Height / 2);

				buttonToMove.Location = new Point(newX, newY);

				graphSpace.Refresh();
			}
		}


		// Метод отрисовки формы
		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			graphSpace.Size = new(ClientRectangle.Width, ClientRectangle.Height - 50);
		}


		// Реакция на изменение ориентированности графа 
		private void isDirectedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			isDirected = ((CheckBox)sender).Checked;
		}


		// Реакция на изменение взвешенности графа 
		private void isWeighedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			isWeighed = ((CheckBox)sender).Checked;
		}


		// Обработка нажатия на кнопку "Добавить вершины"
		private void addNodeBtn_Click(object sender, EventArgs e)
		{
			AdjustPanel((Button)sender);

			isDirectedCheckBox.Enabled = false;
			isWeighedCheckBox.Enabled = false;

			edgeToAddFirst = null;
			InfoBox = null;
		}


		// Обработка нажатия правой кнопкой мыши на существующую вершину
		private void onNodeBtn_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				//Button nodePressed = nodes.Find(x => x == sender);

				//TextBox textBox = new TextBox();
				//textBox.Text = nodePressed.Text;

				//textBox.Location = nodePressed.Location;
				//textBox.Size = nodePressed.Size * 2;

				//graphSpace.Controls.Add(textBox);

				//textBox.Focus();
			}
		}


		// Обработка нажатия на кнопку "Новый граф"
		private void newGraphBtn_Click(object sender, EventArgs e)
		{
			isWeighed = false;
			isDirected = false;
			isDirectedCheckBox.Enabled = true;
			isWeighedCheckBox.Enabled = true;
			isDirectedCheckBox.Checked = false;
			isWeighedCheckBox.Checked = false;
			nodeCount = 0;

			isNewGraph = true;

			foreach (var item in nodes)
			{
				graphSpace.Controls.Remove(item);
			}
			nodes.Clear();

			edgeToAddFirst = null;
			InfoBox = null;

			AdjustPanel(null);
		}


		// Обработка нажатия на кнопку "Удалить вершины"
		private void deleteNodeBtn_Click(object sender, EventArgs e)
		{
			AdjustPanel((Button)sender);

			edgeToAddFirst = null;
			InfoBox = null;
		}


		// Отрисовка связей между вершинами
		private void graphSpace_Paint(object sender, PaintEventArgs e)
		{
			if (DrawEdges && nodes.Count > 0)
			{
				int t1 = nodeSize.X / 2;
				int t2 = nodeSize.Y / 2;

				var g = e.Graphics;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

				var pen = new Pen(Color.Black, 2);

				List<string> used = new();
				foreach (var v1 in graph)
				{
					foreach (var v2 in v1.Value)
					{
						if (v1.Key == v2.Key)
						{
							Button node = nodes.Find(v => v.Name == v1.Key);
							int circleWidth = 45;
							int circleHeight = 45;
							var centerPoint = new Point(node.Location.X + t1 - circleWidth, node.Location.Y + t2 - circleHeight);

							g.DrawEllipse(pen, centerPoint.X, centerPoint.Y, circleWidth, circleHeight);

							if (isWeighed)
							{
								g.DrawString("[" + graph[v1.Key][v2.Key].ToString() + "]", new Font("Arial", 12), Brushes.DarkRed, new Point(centerPoint.X, centerPoint.Y - (int)circleHeight / 2));
							}
						}
						else
						{
							Button node1 = nodes.Find(v => v.Name == v1.Key);
							Button node2 = nodes.Find(v => v.Name == v2.Key);

							var startPoint = new Point(node1.Location.X + t1, node1.Location.Y + t2);
							var endPoint = new Point(node2.Location.X + t1, node2.Location.Y + t2);

							if (isWeighed)
							{
								if (graph[v2.Key].ContainsKey(v1.Key))
								{
									if (used.Contains(v2.Key) is false)
									{
										g.DrawString($"[{graph[v1.Key][v2.Key].ToString()}, {graph[v2.Key][v1.Key].ToString()}]", new Font("Arial", 12), Brushes.DarkRed,
											new Point((int)((startPoint.X + endPoint.X) / 2), (int)((startPoint.Y + endPoint.Y)) / 2));
									}
								}
								else
								{
									g.DrawString("[" + graph[v1.Key][v2.Key].ToString() + "]", new Font("Arial", 12), Brushes.DarkRed,
											new Point((int)((startPoint.X + endPoint.X) / 2), (int)((startPoint.Y + endPoint.Y)) / 2));
								}
							}

							if (isDirected && graph[v2.Key].ContainsKey(v1.Key) is false)
							{
								pen = new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(
									startPoint, endPoint, Color.Black, Color.GhostWhite))
								{ Width = 2 };
							}

							g.DrawLine(pen, startPoint, endPoint);
						}
					}

					used.Add(v1.Key);
				}
			}
		}


		// Обработка нажатия на кнопку "Добавить свзяь"
		private void addEdgeBtn_Click(object sender, EventArgs e)
		{
			AdjustPanel((Button)sender);

			InfoBox = "Укажите первую вершину";
		}


		// Визуализация обхода в ширину
		private void BFSBtn_Click(object sender, EventArgs e)
		{
			AdjustPanel(null);
			BFSSelected = true;

			InfoBox = "Укажите начальную вершину";
		}


		// Установка активной функции
		private void AdjustPanel(Button button)
		{
			graphSpace.Refresh();

			addNodeBtn.ForeColor = Color.Black;
			addEdgeBtn.ForeColor = Color.Black;
			deleteNodeBtn.ForeColor = Color.Black;
			addNodeSelected = false;
			delNodeSelected = false;
			addEdgeSelected = false;
			BFSSelected = false;

			if (button == addNodeBtn)
			{
				addNodeBtn.ForeColor = Color.White;
				addNodeSelected = true;
			}
			else if (button == addEdgeBtn)
			{
				addEdgeBtn.ForeColor = Color.White;
				addEdgeSelected = true;
			}
			else if (button == deleteNodeBtn)
			{
				deleteNodeBtn.ForeColor = Color.White;
				delNodeSelected = true;
			}
		}


		// Визуализация поиска максимального потока
		private void MaxFlowBtn_Cllck(object sender, EventArgs e)
		{
			InfoBox = null;

			AdjustPanel(null);
			DrawEdges = false;
			Refresh();

			MaxFlow.form = this;
			MaxFlow.graph = graph;
			MaxFlow.pen = new Pen(Color.MediumVioletRed, 4);
			MaxFlow.graphSpace = graphSpace;
			MaxFlow.nodes = nodes;
			MaxFlow.nodeSize = nodeSize;

			int result = MaxFlow.MaxFlowStart(delay);
			InfoBox = (graph == null || graph.Count() < 2 || result == -1) ? InfoBox = "Структура графа непригодна для этого алгоритма" : $"Максимальный поток = {result}";

			DrawEdges = true;
		}
	}
}