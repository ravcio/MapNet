using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace gpxEditor
{
    public class GPXViewTree : IGPXView
    {
        TreeView treeView1;

        private System.Windows.Forms.ContextMenuStrip mnuTextFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelect;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;

        public GPXViewTree(TreeView treeView)
        {
            treeView1 = treeView;

            treeView1.MouseUp += new MouseEventHandler(treeView1_MouseUp);
            treeView1.AfterCheck += new TreeViewEventHandler(treeView1_AfterCheck);

            mnuTextFile = new System.Windows.Forms.ContextMenuStrip();
            toolStripMenuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemSelect.Text = "Select";
            toolStripMenuItemSelect.Click += new EventHandler(toolStripMenuItemSelect_Click);
            toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemDelete.Text = "Delete";
            toolStripMenuItemDelete.Click += new EventHandler(toolStripMenuItemDelete_Click);

            this.mnuTextFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelect,
            this.toolStripMenuItemDelete});
        }

        void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            bool modified = false;

            // change selection
            TreeView treeView = sender as TreeView;
            foreach (TreeNode trkNode in treeView.Nodes)
            {
                GPXTrk trk = trkNode.Tag as GPXTrk;
                Debug.Assert(trk != null);
                if (trk.selected != trkNode.Checked)
                {
                    trk.selected = trkNode.Checked;
                    modified = true;
                }
                foreach (TreeNode segNode in trkNode.Nodes)
                {
                    GPXTrkSeg trkseg = segNode.Tag as GPXTrkSeg;
                    Debug.Assert(trkseg != null);
                    foreach (GpxWpt wpt in trkseg.wpts)
                    {
                        if (wpt.selected != segNode.Checked)
                        {
                            wpt.selected = segNode.Checked;
                            modified = true;
                        }
                    }
                    if (trkseg.selected != segNode.Checked)
                    {
                        trkseg.selected = segNode.Checked;
                        modified = true;
                    }
                }
            }
            if (modified)
            {
                if (ChangedData != null)
                {
                    ChangedData(this, new EventArgs());
                }
            }
        }

        void toolStripMenuItemSelect_Click(object sender, EventArgs e)
        {
            bool modified = false;

            GPXTrk trk = treeView1.SelectedNode.Tag as GPXTrk;
            if (trk != null)
            {
                modified = true;
                selectTrk(trk);
            }

            GPXTrkSeg seg = treeView1.SelectedNode.Tag as GPXTrkSeg;
            if (seg != null)
            {
                modified = true;
                selectSeg(seg);
            }

            if (modified)
            {
                if (ChangedData != null)
                {
                    Repaint();
                    ChangedData(this, new EventArgs());
                }
            }
        }

        void selectTrk(GPXTrk trk)
        {
            foreach (GPXTrkSeg seg in trk.trkSeg)
            {
                selectSeg(seg);
            }
        }

        void selectSeg(GPXTrkSeg seg)
        {
            seg.selected = true;
            foreach (GpxWpt wpt in seg.wpts)
            {
                wpt.selected = true;
            }
        }




        void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            bool modified = false;

            // what item was clicked?
            GPXTrk trk = treeView1.SelectedNode.Tag as GPXTrk;
            if (trk != null)
            {
                modified = true;
                gpxFile.trks.Remove(trk);
            }

            GPXTrkSeg seg = treeView1.SelectedNode.Tag as GPXTrkSeg;
            if (seg != null)
            {
                foreach (GPXTrk trk1 in gpxFile.trks)
                {
                    trk1.trkSeg.Remove(seg);
                }
                modified = true;
            }

            if (modified)
            {
                if (ChangedData != null)
                {
                    Repaint();
                    ChangedData(this, new EventArgs());
                }
            }

            //throw new NotImplementedException();
        }


        void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {

                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null)
                {
                    treeView1.SelectedNode = node;

                    // Find the appropriate ContextMenu depending on the selected node.
                    mnuTextFile.Show(treeView1, p);
                }
            }
        }


        #region IGPXView Members

        GPXFile gpxFile;
        public void Bind(GPXFile gpxFile)
        {
            this.gpxFile = gpxFile;
        }

        public void Repaint()
        {
            fillTreeWithGPX(gpxFile, treeView1);
        }

        public void NewLocation()
        {
            // select respective node
            GpxWpt wptFind = gpxFile.location;
            if (wptFind != null)
            {
                foreach (TreeNode tn_trk in treeView1.Nodes)
                {
                    foreach (TreeNode tn_seg in tn_trk.Nodes)
                    {
                        GPXTrkSeg seg = tn_seg.Tag as GPXTrkSeg;
                        Debug.Assert(seg != null);
                        {
                            foreach (GpxWpt wpt in seg.wpts)
                            {
                                if (wpt == wptFind)
                                {
                                    treeView1.SelectedNode = tn_seg;
                                }
                            }
                        }
                    }
                 }
            }
        }

        public event EventHandler ChangedData;
        public event EventHandler ChangedLocation;

        #endregion

        private void fillTreeWithGPX(GPXFile gpxFile, TreeView treeView1)
        {
            treeView1.Visible = false; // hack to prevent flickering
            treeView1.Nodes.Clear();
            if (gpxFile == null) return;

            foreach (GPXTrk trk in gpxFile.trks)
            {
                TreeNode nodeTrk = new TreeNode("Track: " + trk.name);
                treeView1.Nodes.Add(nodeTrk);
                nodeTrk.Tag = trk;
                nodeTrk.Checked = trk.selected;

                foreach (GPXTrkSeg trkseg in trk.trkSeg)
                {
                    string name = "Segment with " + trkseg.wpts.Count + " points.";
                    TreeNode nodeSeg = new TreeNode(name);
                    nodeSeg.Tag = trkseg;
                    nodeSeg.Checked = trkseg.selected;

                    nodeTrk.Nodes.Add(nodeSeg);
                }
                nodeTrk.Expand();
            }
            treeView1.Visible = true;
        }
    }
}
