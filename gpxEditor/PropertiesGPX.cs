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
    public partial class PropertiesGPX : Form
    {
        public string fileName = "";
        public string len = "";
        public string timeSpan = "";

        public PropertiesGPX()
        {
            InitializeComponent();

        }

        private void PropertiesGPX_Load(object sender, EventArgs e)
        {
            txtFileName.Text = fileName;
            txtLen.Text = len;
            txtTimeSpan.Text = timeSpan;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {

        }
    }
}
