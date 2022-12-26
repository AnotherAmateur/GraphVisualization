﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualisation
{
	public partial class EdgeEditBox : Form
	{
		private int weight;

		public int Weight
		{
			get { return int.Parse(setWeightBox.Text); }
			set { setWeightBox.Text = value.ToString(); }
		}


		public string InfoBox
		{
			get { return edEdgeBoxInfo.Text; }
			set { edEdgeBoxInfo.Text = value; }
		}

		public bool EdgeAdded { get; set; }

		public EdgeEditBox()
		{
			InitializeComponent();
		}


		private void delEdgeBtn_Click(object sender, EventArgs e)
		{
			EdgeAdded = false;
			this.Hide();
		}


		private void addEdgeBtn_Click(object sender, EventArgs e)
		{
			if (int.TryParse(setWeightBox.Text, out weight) is false)
			{
				edEdgeBoxInfo.Text = "Неверный формат";
			}

			Weight = weight;
			EdgeAdded = true;
			this.Hide();
		}
	}
}
