namespace GraphVisualisation
{
	partial class GraphVisul
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.graphSpace = new System.Windows.Forms.Panel();
			this.isWeighedCheckBox = new System.Windows.Forms.CheckBox();
			this.isDirectedCheckBox = new System.Windows.Forms.CheckBox();
			this.addNodeBtn = new System.Windows.Forms.Button();
			this.newGraphBtn = new System.Windows.Forms.Button();
			this.deleteNodeBtn = new System.Windows.Forms.Button();
			this.addEdgeBtn = new System.Windows.Forms.Button();
			this.infoBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// graphSpace
			// 
			this.graphSpace.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.graphSpace.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.graphSpace.Cursor = System.Windows.Forms.Cursors.Cross;
			this.graphSpace.ImeMode = System.Windows.Forms.ImeMode.On;
			this.graphSpace.Location = new System.Drawing.Point(-2, 58);
			this.graphSpace.Name = "graphSpace";
			this.graphSpace.Size = new System.Drawing.Size(856, 393);
			this.graphSpace.TabIndex = 0;
			this.graphSpace.Paint += new System.Windows.Forms.PaintEventHandler(this.graphSpace_Paint);
			this.graphSpace.MouseClick += new System.Windows.Forms.MouseEventHandler(this.graphSpace_MouseClick);
			// 
			// isWeighedCheckBox
			// 
			this.isWeighedCheckBox.AutoSize = true;
			this.isWeighedCheckBox.Location = new System.Drawing.Point(12, 12);
			this.isWeighedCheckBox.Name = "isWeighedCheckBox";
			this.isWeighedCheckBox.Size = new System.Drawing.Size(97, 19);
			this.isWeighedCheckBox.TabIndex = 1;
			this.isWeighedCheckBox.Text = "Взвешенный";
			this.isWeighedCheckBox.UseVisualStyleBackColor = true;
			this.isWeighedCheckBox.CheckedChanged += new System.EventHandler(this.isWeighedCheckBox_CheckedChanged);
			// 
			// isDirectedCheckBox
			// 
			this.isDirectedCheckBox.AutoSize = true;
			this.isDirectedCheckBox.Location = new System.Drawing.Point(12, 37);
			this.isDirectedCheckBox.Name = "isDirectedCheckBox";
			this.isDirectedCheckBox.Size = new System.Drawing.Size(130, 19);
			this.isDirectedCheckBox.TabIndex = 2;
			this.isDirectedCheckBox.Text = "Ориентированный";
			this.isDirectedCheckBox.UseVisualStyleBackColor = true;
			this.isDirectedCheckBox.CheckedChanged += new System.EventHandler(this.isDirectedCheckBox_CheckedChanged);
			// 
			// addNodeBtn
			// 
			this.addNodeBtn.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.addNodeBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
			this.addNodeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addNodeBtn.ForeColor = System.Drawing.Color.Black;
			this.addNodeBtn.Location = new System.Drawing.Point(193, 8);
			this.addNodeBtn.Name = "addNodeBtn";
			this.addNodeBtn.Size = new System.Drawing.Size(146, 44);
			this.addNodeBtn.TabIndex = 3;
			this.addNodeBtn.Text = "Добавить/переместить вершины";
			this.addNodeBtn.UseVisualStyleBackColor = false;
			this.addNodeBtn.Click += new System.EventHandler(this.addNodeBtn_Click);
			// 
			// newGraphBtn
			// 
			this.newGraphBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.newGraphBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
			this.newGraphBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.newGraphBtn.Location = new System.Drawing.Point(762, 8);
			this.newGraphBtn.Name = "newGraphBtn";
			this.newGraphBtn.Size = new System.Drawing.Size(75, 44);
			this.newGraphBtn.TabIndex = 4;
			this.newGraphBtn.Text = "Новый граф";
			this.newGraphBtn.UseVisualStyleBackColor = true;
			this.newGraphBtn.Click += new System.EventHandler(this.newGraphBtn_Click);
			// 
			// deleteNodeBtn
			// 
			this.deleteNodeBtn.AutoEllipsis = true;
			this.deleteNodeBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
			this.deleteNodeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.deleteNodeBtn.Location = new System.Drawing.Point(449, 8);
			this.deleteNodeBtn.Name = "deleteNodeBtn";
			this.deleteNodeBtn.Size = new System.Drawing.Size(75, 44);
			this.deleteNodeBtn.TabIndex = 5;
			this.deleteNodeBtn.Text = "Удалить вершины";
			this.deleteNodeBtn.UseVisualStyleBackColor = true;
			this.deleteNodeBtn.Click += new System.EventHandler(this.deleteNodeBtn_Click);
			// 
			// addEdgeBtn
			// 
			this.addEdgeBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
			this.addEdgeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addEdgeBtn.Location = new System.Drawing.Point(345, 8);
			this.addEdgeBtn.Name = "addEdgeBtn";
			this.addEdgeBtn.Size = new System.Drawing.Size(98, 44);
			this.addEdgeBtn.TabIndex = 6;
			this.addEdgeBtn.Text = "Редактировать связи";
			this.addEdgeBtn.UseVisualStyleBackColor = true;
			this.addEdgeBtn.Click += new System.EventHandler(this.addEdgeBtn_Click);
			// 
			// infoBox
			// 
			this.infoBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.infoBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.infoBox.Location = new System.Drawing.Point(556, 23);
			this.infoBox.Name = "infoBox";
			this.infoBox.Size = new System.Drawing.Size(200, 16);
			this.infoBox.TabIndex = 7;
			// 
			// GraphVisul
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(849, 450);
			this.Controls.Add(this.infoBox);
			this.Controls.Add(this.addEdgeBtn);
			this.Controls.Add(this.deleteNodeBtn);
			this.Controls.Add(this.newGraphBtn);
			this.Controls.Add(this.addNodeBtn);
			this.Controls.Add(this.isDirectedCheckBox);
			this.Controls.Add(this.isWeighedCheckBox);
			this.Controls.Add(this.graphSpace);
			this.MinimumSize = new System.Drawing.Size(865, 100);
			this.Name = "GraphVisul";
			this.Text = "GraphVisul";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Panel graphSpace;
		private CheckBox isWeighedCheckBox;
		private CheckBox isDirectedCheckBox;
		private Button addNodeBtn;
		private Button newGraphBtn;
		private Button deleteNodeBtn;
		private Button addEdgeBtn;
		private TextBox infoBox;
	}
}