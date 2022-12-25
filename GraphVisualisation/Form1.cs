using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace GraphVisualisation
{
	public partial class GraphVisul : Form
	{
		int nodeCount;
		private List<Button> nodes;
		MyGraph.Graph graph;
		bool isDirected;
		bool isWeighed;
		bool addNode;
		bool removeNode;
		bool isNewGraph;
		bool addEdge;
		string edgeToAdd;
		readonly Point nodeSize;


		// ����������� �����
		public GraphVisul()
		{
			graph = new(false);
			nodes = new();
			InitializeComponent();
			nodeSize = new(35, 35);

			typeof(Panel).InvokeMember("DoubleBuffered",
			BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
			null, graphSpace, new object[] { true });
		}


		// ��������� ����� ����� �� ���� ��������� ����� 
		private void graphSpace_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				// ���� ���������� ������ � ����� ����� � ���� ���������
				if (addNode)
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
			else if (e.Button == MouseButtons.Right)
			{

			}
		}


		// ��������� ������� ����� ���� �� �������
		private void node_MouseClick(object? sender, EventArgs e)
		{
			if (removeNode)
			{
				Button buttonToRemove = nodes.Find(x => x == sender);

				graph.DeleteNode(buttonToRemove.Name);
				nodes.Remove(buttonToRemove);
				graphSpace.Controls.Remove(buttonToRemove);

				graphSpace.Refresh();
			}
			else if (addEdge)
			{
				if (edgeToAdd == null)
				{
					edgeToAdd = ((Button)sender).Name;
					infoBox.Text = "������ ������� ������ �������";
				}
				else
				{
					graph.AddOrUpdateEdge(edgeToAdd, ((Button)sender).Name, 1);
					graphSpace.Refresh();
					edgeToAdd = null;
					infoBox.Text = "������� ������ �������";
				}
			}
		}


		// ����������� ������� ������� ����� ������� ����
		private void node_MouseMove(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && addNode)
			{
				Button buttonToMove = nodes.Find(x => x == sender);

				Point mousePos = buttonToMove.PointToClient(Cursor.Position);

				int newX = buttonToMove.Left + mousePos.X - (buttonToMove.Width / 2);
				int newY = buttonToMove.Top + mousePos.Y - (buttonToMove.Height / 2);

				buttonToMove.Location = new Point(newX, newY);

				graphSpace.Refresh();
			}
		}


		// ����� ��������� �����
		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			graphSpace.Size = new(ClientRectangle.Width, ClientRectangle.Height - 50);
		}


		// ������� �� ��������� ����������������� ����� 
		private void isDirectedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			isDirected = ((CheckBox)sender).Checked;
		}


		// ������� �� ��������� ������������ ����� 
		private void isWeighedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			isWeighed = ((CheckBox)sender).Checked;
		}


		// ��������� ������� �� ������ "�������� �������"
		private void addNodeBtn_Click(object sender, EventArgs e)
		{
			addNode = true;
			removeNode = false;
			isDirectedCheckBox.Enabled = false;
			isWeighedCheckBox.Enabled = false;

			addEdge = false;
			edgeToAdd = null;
			infoBox.Text = null;
		}


		// ��������� ������� ������ ������� ���� �� ������������ �������
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


		// ��������� ������� �� ������ "����� ����"
		private void newGraphBtn_Click(object sender, EventArgs e)
		{
			isDirected = false;
			isWeighed = false;
			isDirectedCheckBox.Enabled = true;
			isWeighedCheckBox.Enabled = true;
			isDirectedCheckBox.Checked = false;
			isWeighedCheckBox.Checked = false;
			nodeCount = 0;

			isNewGraph = true;
			addNode = false;
			removeNode = false;
			addEdge = false;
			edgeToAdd = null;
			infoBox.Text = null;

			foreach (var item in nodes)
			{
				graphSpace.Controls.Remove(item);
			}
			nodes.Clear();

			graphSpace.Refresh();
		}


		// ��������� ������� �� ������ "������� �������"
		private void deleteNodeBtn_Click(object sender, EventArgs e)
		{
			removeNode = true;
			addNode = false;
			addEdge = false;
			edgeToAdd = null;
			infoBox.Text = null;
		}


		// ��������� ������ ����� ���������
		private void graphSpace_Paint(object sender, PaintEventArgs e)
		{
			if (nodes.Count > 1)
			{
				int t1 = nodeSize.X / 2;
				int t2 = nodeSize.Y / 2;

				var g = e.Graphics;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

				var pen = new Pen(Color.Black, 3);

				foreach (var v1 in graph)
				{
					foreach (var v2 in v1.Value)
					{
						Button node1 = nodes.Find(v => v.Name == v1.Key);
						Button node2 = nodes.Find(v => v.Name == v2.Key);

						var startPoint = new Point(node1.Location.X + t1, node1.Location.Y + t2);
						var endPoint = new Point(node2.Location.X + t1, node2.Location.Y + t2);

						if (isDirected)
						{
							pen = new Pen(new System.Drawing.Drawing2D.LinearGradientBrush(startPoint, endPoint, Color.MediumVioletRed, Color.GhostWhite)) { Width = 3 };
						}

						g.DrawLine(pen, startPoint, endPoint);
					}
				}
			}
		}


		// ��������� ������� �� ������ "�������� �����"
		private void addEdgeBtn_Click(object sender, EventArgs e)
		{
			addNode = false;
			removeNode = false;
			addEdge = true;

			infoBox.Text = "������� ������ �������";
		}
	}
}