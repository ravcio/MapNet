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
    public partial class frmTileDownloader : Form
    {
        public bool cancel = false;
        public frmTileDownloader()
        {
            InitializeComponent();
        }

        internal void Update(string message, int progress)
        {
            progressBar1.Value = progress;
            label1.Text = message;
            Application.DoEvents(); 
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }
    }
}
