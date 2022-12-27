namespace GraphVisualisation
{
	public partial class EdgeEditBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.setWeightBox = new System.Windows.Forms.TextBox();
			this.delEdgeBtn = new System.Windows.Forms.Button();
			this.addEdgeBtn = new System.Windows.Forms.Button();
			this.edEdgeBoxInfo = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// setWeightBox
			// 
			this.setWeightBox.Location = new System.Drawing.Point(170, 25);
			this.setWeightBox.Name = "setWeightBox";
			this.setWeightBox.Size = new System.Drawing.Size(75, 23);
			this.setWeightBox.TabIndex = 0;
			this.setWeightBox.Text = "0";
			this.setWeightBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.setWeightBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.setWeightBox_KeyDown);
			// 
			// delEdgeBtn
			// 
			this.delEdgeBtn.Location = new System.Drawing.Point(12, 82);
			this.delEdgeBtn.Name = "delEdgeBtn";
			this.delEdgeBtn.Size = new System.Drawing.Size(75, 23);
			this.delEdgeBtn.TabIndex = 1;
			this.delEdgeBtn.Text = "Удалить";
			this.delEdgeBtn.UseVisualStyleBackColor = true;
			this.delEdgeBtn.Click += new System.EventHandler(this.delEdgeBtn_Click);
			// 
			// addEdgeBtn
			// 
			this.addEdgeBtn.Location = new System.Drawing.Point(170, 82);
			this.addEdgeBtn.Name = "addEdgeBtn";
			this.addEdgeBtn.Size = new System.Drawing.Size(75, 23);
			this.addEdgeBtn.TabIndex = 2;
			this.addEdgeBtn.Text = "Добавить";
			this.addEdgeBtn.UseVisualStyleBackColor = true;
			this.addEdgeBtn.Click += new System.EventHandler(this.addEdgeBtn_Click);
			// 
			// edEdgeBoxInfo
			// 
			this.edEdgeBoxInfo.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.edEdgeBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.edEdgeBoxInfo.Location = new System.Drawing.Point(12, 28);
			this.edEdgeBoxInfo.Name = "edEdgeBoxInfo";
			this.edEdgeBoxInfo.Size = new System.Drawing.Size(75, 16);
			this.edEdgeBoxInfo.TabIndex = 3;
			this.edEdgeBoxInfo.Text = "Укажите вес";
			// 
			// EdgeEditBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(257, 117);
			this.Controls.Add(this.edEdgeBoxInfo);
			this.Controls.Add(this.addEdgeBtn);
			this.Controls.Add(this.delEdgeBtn);
			this.Controls.Add(this.setWeightBox);
			this.Location = new System.Drawing.Point(150, 150);
			this.MaximumSize = new System.Drawing.Size(273, 156);
			this.MinimumSize = new System.Drawing.Size(273, 156);
			this.Name = "EdgeEditBox";
			this.Text = "EdgeEditBox";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public TextBox setWeightBox;
		private Button delEdgeBtn;
		private Button addEdgeBtn;
		private TextBox edEdgeBoxInfo;
	}
}