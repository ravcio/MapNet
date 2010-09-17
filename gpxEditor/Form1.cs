using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gpxEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timeSlide1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            timeSlide1.PlotValuesBlack.Clear();

            for (int i = 0; i < 100; i++)
            {
                timeSlide1.PlotValuesBlack.Add(new KeyValuePair<DateTime, double>(time, (1 + Math.Sin(i / 4.0)) * 24.0));
                time = time.AddMinutes(5);
            }

            timeSlide1.ValueStart = DateTime.Now;
            //timeSlide1.Refresh();

            timeSlide1.ValueMarker = DateTime.Now + TimeSpan.FromMinutes (120);
        }
    }
}
