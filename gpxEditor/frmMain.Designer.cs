namespace gpxEditor
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inverseSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.cropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stripTimeStampToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.tileMapTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleSatelliteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bingMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openStreetMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedDistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altitudeDistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.speedTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altitudeTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanceTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.tileDownloaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Open = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Pan = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Arrow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_info = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_GM = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_GS = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_BM = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_OSM = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_UMP = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelection = new System.Windows.Forms.ToolStripStatusLabel();
            this.gpxScrollBar = new System.Windows.Forms.HScrollBar();
            this.timeSlide1 = new TimeSlideControl.TimeSlide();
            this.mapControl1 = new hiMapNet.MapControl();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(813, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.openMoreToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem5,
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem9,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openMoreToolStripMenuItem
            // 
            this.openMoreToolStripMenuItem.Name = "openMoreToolStripMenuItem";
            this.openMoreToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.openMoreToolStripMenuItem.Text = "Open more...";
            this.openMoreToolStripMenuItem.Click += new System.EventHandler(this.openMoreToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(190, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(190, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem2,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem3,
            this.selectAllToolStripMenuItem,
            this.selectMoreToolStripMenuItem,
            this.inverseSelectionToolStripMenuItem,
            this.toolStripMenuItem4,
            this.cropToolStripMenuItem,
            this.stripTimeStampToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(189, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(189, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectMoreToolStripMenuItem
            // 
            this.selectMoreToolStripMenuItem.Name = "selectMoreToolStripMenuItem";
            this.selectMoreToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.selectMoreToolStripMenuItem.Text = "Select more...";
            // 
            // inverseSelectionToolStripMenuItem
            // 
            this.inverseSelectionToolStripMenuItem.Name = "inverseSelectionToolStripMenuItem";
            this.inverseSelectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.inverseSelectionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.inverseSelectionToolStripMenuItem.Text = "Inverse selection";
            this.inverseSelectionToolStripMenuItem.Click += new System.EventHandler(this.inverseSelectionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(189, 6);
            // 
            // cropToolStripMenuItem
            // 
            this.cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            this.cropToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.cropToolStripMenuItem.Text = "Crop selection";
            // 
            // stripTimeStampToolStripMenuItem
            // 
            this.stripTimeStampToolStripMenuItem.Name = "stripTimeStampToolStripMenuItem";
            this.stripTimeStampToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.stripTimeStampToolStripMenuItem.Text = "Strip time stamp";
            this.stripTimeStampToolStripMenuItem.Click += new System.EventHandler(this.stripTimeStampToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomOutToolStripMenuItem,
            this.zoomInToolStripMenuItem,
            this.toolStripMenuItem7,
            this.tileMapTypeToolStripMenuItem,
            this.trackViewToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(137, 6);
            // 
            // tileMapTypeToolStripMenuItem
            // 
            this.tileMapTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.googleMapsToolStripMenuItem,
            this.googleSatelliteToolStripMenuItem,
            this.bingMapsToolStripMenuItem,
            this.openStreetMapToolStripMenuItem});
            this.tileMapTypeToolStripMenuItem.Name = "tileMapTypeToolStripMenuItem";
            this.tileMapTypeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.tileMapTypeToolStripMenuItem.Text = "Tile Map Type";
            // 
            // googleMapsToolStripMenuItem
            // 
            this.googleMapsToolStripMenuItem.Name = "googleMapsToolStripMenuItem";
            this.googleMapsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.googleMapsToolStripMenuItem.Text = "Google Maps";
            // 
            // googleSatelliteToolStripMenuItem
            // 
            this.googleSatelliteToolStripMenuItem.Name = "googleSatelliteToolStripMenuItem";
            this.googleSatelliteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.googleSatelliteToolStripMenuItem.Text = "Google Satellite";
            // 
            // bingMapsToolStripMenuItem
            // 
            this.bingMapsToolStripMenuItem.Name = "bingMapsToolStripMenuItem";
            this.bingMapsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.bingMapsToolStripMenuItem.Text = "Bing Satellite";
            // 
            // openStreetMapToolStripMenuItem
            // 
            this.openStreetMapToolStripMenuItem.Name = "openStreetMapToolStripMenuItem";
            this.openStreetMapToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openStreetMapToolStripMenuItem.Text = "Open Street Map";
            // 
            // trackViewToolStripMenuItem
            // 
            this.trackViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speedDistanceToolStripMenuItem,
            this.altitudeDistanceToolStripMenuItem,
            this.toolStripMenuItem6,
            this.speedTimeToolStripMenuItem,
            this.altitudeTimeToolStripMenuItem,
            this.distanceTimeToolStripMenuItem});
            this.trackViewToolStripMenuItem.Name = "trackViewToolStripMenuItem";
            this.trackViewToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.trackViewToolStripMenuItem.Text = "Track view";
            // 
            // speedDistanceToolStripMenuItem
            // 
            this.speedDistanceToolStripMenuItem.Name = "speedDistanceToolStripMenuItem";
            this.speedDistanceToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.speedDistanceToolStripMenuItem.Text = "Speed / Distance";
            // 
            // altitudeDistanceToolStripMenuItem
            // 
            this.altitudeDistanceToolStripMenuItem.Name = "altitudeDistanceToolStripMenuItem";
            this.altitudeDistanceToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.altitudeDistanceToolStripMenuItem.Text = "Altitude / Distance";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(159, 6);
            // 
            // speedTimeToolStripMenuItem
            // 
            this.speedTimeToolStripMenuItem.Name = "speedTimeToolStripMenuItem";
            this.speedTimeToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.speedTimeToolStripMenuItem.Text = "Speed / Time";
            // 
            // altitudeTimeToolStripMenuItem
            // 
            this.altitudeTimeToolStripMenuItem.Name = "altitudeTimeToolStripMenuItem";
            this.altitudeTimeToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.altitudeTimeToolStripMenuItem.Text = "Altitude / Time";
            // 
            // distanceTimeToolStripMenuItem
            // 
            this.distanceTimeToolStripMenuItem.Name = "distanceTimeToolStripMenuItem";
            this.distanceTimeToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.distanceTimeToolStripMenuItem.Text = "Distance / Time";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolStripMenuItem8,
            this.tileDownloaderToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(147, 6);
            // 
            // tileDownloaderToolStripMenuItem
            // 
            this.tileDownloaderToolStripMenuItem.Name = "tileDownloaderToolStripMenuItem";
            this.tileDownloaderToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.tileDownloaderToolStripMenuItem.Text = "Tile Downloader";
            this.tileDownloaderToolStripMenuItem.Click += new System.EventHandler(this.tileDownloaderToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 52);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mapControl1);
            this.splitContainer1.Size = new System.Drawing.Size(813, 294);
            this.splitContainer1.SplitterDistance = 148;
            this.splitContainer1.TabIndex = 14;
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(148, 294);
            this.treeView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Open,
            this.toolStripSeparator1,
            this.toolStripButton_ZoomIn,
            this.toolStripButton_ZoomOut,
            this.toolStripButton_Pan,
            this.toolStripSeparator4,
            this.toolStripButton_Arrow,
            this.toolStripButton1,
            this.toolStripButton_info,
            this.toolStripSeparator2,
            this.toolStripButton_GM,
            this.toolStripButton_GS,
            this.toolStripButton_BM,
            this.toolStripButton_OSM,
            this.toolStripButton_UMP});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(813, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Open
            // 
            this.toolStripButton_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Open.Image = global::gpxEditor.Properties.Resources.open;
            this.toolStripButton_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Open.Name = "toolStripButton_Open";
            this.toolStripButton_Open.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Open.Text = "Open GPX";
            this.toolStripButton_Open.Click += new System.EventHandler(this.toolStripButton_Open_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ZoomIn
            // 
            this.toolStripButton_ZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ZoomIn.Image = global::gpxEditor.Properties.Resources.zoomin;
            this.toolStripButton_ZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ZoomIn.Name = "toolStripButton_ZoomIn";
            this.toolStripButton_ZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_ZoomIn.Text = "Zoom In";
            this.toolStripButton_ZoomIn.Click += new System.EventHandler(this.toolStripButton_ZoomIn_Click);
            // 
            // toolStripButton_ZoomOut
            // 
            this.toolStripButton_ZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ZoomOut.Image = global::gpxEditor.Properties.Resources.zoomout;
            this.toolStripButton_ZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ZoomOut.Name = "toolStripButton_ZoomOut";
            this.toolStripButton_ZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_ZoomOut.Text = "Zoom Out";
            this.toolStripButton_ZoomOut.Click += new System.EventHandler(this.toolStripButton_ZoomOut_Click);
            // 
            // toolStripButton_Pan
            // 
            this.toolStripButton_Pan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Pan.Image = global::gpxEditor.Properties.Resources.pan;
            this.toolStripButton_Pan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Pan.Name = "toolStripButton_Pan";
            this.toolStripButton_Pan.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Pan.Text = "Pan";
            this.toolStripButton_Pan.Click += new System.EventHandler(this.toolStripButton_Pan_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Arrow
            // 
            this.toolStripButton_Arrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Arrow.Image = global::gpxEditor.Properties.Resources.arrow;
            this.toolStripButton_Arrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Arrow.Name = "toolStripButton_Arrow";
            this.toolStripButton_Arrow.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Arrow.Text = "Edit / Select";
            this.toolStripButton_Arrow.Click += new System.EventHandler(this.toolStripButton_Arrow_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::gpxEditor.Properties.Resources.add;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Add";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton_info
            // 
            this.toolStripButton_info.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_info.Image = global::gpxEditor.Properties.Resources.info;
            this.toolStripButton_info.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_info.Name = "toolStripButton_info";
            this.toolStripButton_info.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_info.Text = "Info";
            this.toolStripButton_info.Click += new System.EventHandler(this.toolStripButton_info_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_GM
            // 
            this.toolStripButton_GM.Checked = true;
            this.toolStripButton_GM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_GM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_GM.Image = global::gpxEditor.Properties.Resources.favicon_google;
            this.toolStripButton_GM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_GM.Name = "toolStripButton_GM";
            this.toolStripButton_GM.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_GM.Text = "Google Maps";
            this.toolStripButton_GM.Click += new System.EventHandler(this.toolStripButton_GM_Click);
            // 
            // toolStripButton_GS
            // 
            this.toolStripButton_GS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_GS.Image = global::gpxEditor.Properties.Resources.favicon_google;
            this.toolStripButton_GS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_GS.Name = "toolStripButton_GS";
            this.toolStripButton_GS.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_GS.Text = "Google Satellite View";
            this.toolStripButton_GS.Click += new System.EventHandler(this.toolStripButton_GS_Click);
            // 
            // toolStripButton_BM
            // 
            this.toolStripButton_BM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_BM.Image = global::gpxEditor.Properties.Resources.favicon_bing;
            this.toolStripButton_BM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_BM.Name = "toolStripButton_BM";
            this.toolStripButton_BM.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_BM.Text = "Bing Satellite View";
            this.toolStripButton_BM.Click += new System.EventHandler(this.toolStripButton_BM_Click);
            // 
            // toolStripButton_OSM
            // 
            this.toolStripButton_OSM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_OSM.Image = global::gpxEditor.Properties.Resources.favicon_osm;
            this.toolStripButton_OSM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_OSM.Name = "toolStripButton_OSM";
            this.toolStripButton_OSM.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_OSM.Text = "Open Street Maps";
            this.toolStripButton_OSM.Click += new System.EventHandler(this.toolStripButton_OSM_Click);
            // 
            // toolStripButton_UMP
            // 
            this.toolStripButton_UMP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_UMP.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_UMP.Image")));
            this.toolStripButton_UMP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_UMP.Name = "toolStripButton_UMP";
            this.toolStripButton_UMP.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_UMP.Text = "UMP Maps";
            this.toolStripButton_UMP.Click += new System.EventHandler(this.toolStripButton_UMP_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInfo,
            this.lblSelection});
            this.statusStrip1.Location = new System.Drawing.Point(0, 445);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(813, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(19, 17);
            this.lblInfo.Text = "---";
            // 
            // lblSelection
            // 
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(0, 17);
            // 
            // gpxScrollBar
            // 
            this.gpxScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gpxScrollBar.Location = new System.Drawing.Point(0, 416);
            this.gpxScrollBar.Name = "gpxScrollBar";
            this.gpxScrollBar.Size = new System.Drawing.Size(813, 21);
            this.gpxScrollBar.TabIndex = 18;
            // 
            // timeSlide1
            // 
            this.timeSlide1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeSlide1.BackColor = System.Drawing.SystemColors.Control;
            this.timeSlide1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.timeSlide1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.timeSlide1.Location = new System.Drawing.Point(0, 352);
            this.timeSlide1.Name = "timeSlide1";
            this.timeSlide1.Size = new System.Drawing.Size(813, 61);
            this.timeSlide1.TabIndex = 19;
            this.timeSlide1.ValueMarker = new System.DateTime(1, 1, 1, 0, 5, 0, 0);
            this.timeSlide1.ValueStart = new System.DateTime(((long)(0)));
            // 
            // mapControl1
            // 
            this.mapControl1.BackColor = System.Drawing.Color.White;
            this.mapControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapControl1.CurrentTool = hiMapNet.MapControl.ToolConst.CustomToolProcessor;
            this.mapControl1.CurrentToolProcessor = null;
            this.mapControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl1.Location = new System.Drawing.Point(0, 0);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.ResizeScaleMode = hiMapNet.MapControl.ResizeScaleConst.NoChange;
            this.mapControl1.Size = new System.Drawing.Size(661, 294);
            this.mapControl1.TabIndex = 0;
            this.mapControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapControl1_MouseMove);
            this.mapControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapControl1_MouseClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 467);
            this.Controls.Add(this.timeSlide1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gpxScrollBar);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(714, 318);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GPS Track analyser";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileMapTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleSatelliteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bingMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openStreetMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem cropToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem trackViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem speedDistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altitudeDistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem speedTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altitudeTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem distanceTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private hiMapNet.MapControl mapControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Pan;
        private System.Windows.Forms.ToolStripButton toolStripButton_ZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButton_ZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButton_GM;
        private System.Windows.Forms.ToolStripButton toolStripButton_GS;
        private System.Windows.Forms.ToolStripButton toolStripButton_BM;
        private System.Windows.Forms.ToolStripButton toolStripButton_OSM;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_Arrow;
        private System.Windows.Forms.ToolStripButton toolStripButton_info;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem stripTimeStampToolStripMenuItem;
        private System.Windows.Forms.HScrollBar gpxScrollBar;
        private System.Windows.Forms.ToolStripStatusLabel lblSelection;
        private System.Windows.Forms.ToolStripMenuItem selectMoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inverseSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_UMP;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private TimeSlideControl.TimeSlide timeSlide1;
        private System.Windows.Forms.ToolStripMenuItem tileDownloaderToolStripMenuItem;
    }
}

