using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hiMapNet;
using GMap.NET;
using System.IO.Ports;
using System.Globalization;
using System.Reflection;
using System.Resources;
using gpxEditor.Properties;

namespace gpxEditor
{
    public partial class frmMain : Form
    {
        // current GPX filename
        string m_currentGPXfileName = "";

        // gps reception
        NmeaInterpreter GPS = new NmeaInterpreter();
        NMEA2OSG OSGconv = new NMEA2OSG();
        private SerialPort comport = new SerialPort("COM3", 115000, Parity.None, 8, StopBits.One);
        public string instring;
        public string[] gpsString;

        CoordConverter oCCGPS;

        LayerAbstract tileLayer = null;

        // Views
        GPXViewMap gpxMapView = null;
        GPXViewTree gpxViewTree = null;
        GPXViewScrollbar gpxViewScrollbar = null;
        GPXViewTimeSlide gpxViewTimeSlide = null;

        GPXPresenter gpxPresenter;


        public frmMain()
        {
            InitializeComponent();
        }

        private void InitMapAndPresenter()
        {
            // set Display coordsys
            mapControl1.Map.DisplayCoordSys = CoordSysFactory.CreateCoordSys("Mercator Datum(WGS84)");

            // make Map Layer
            mapControl1.Map.Layers.Clear();
            mapControl1.ResizeScaleMode = MapControl.ResizeScaleConst.NoChange;

            // One Tile Layer
            tileLayer = new LayerTilesAsynch(); // mercator (datum(wgs84))
            mapControl1.Map.Layers.Add(tileLayer);

            // MVC ------------
            // bind view to presentation surface (control)

            gpxMapView = new GPXViewMap(mapControl1);
            gpxViewTree = new GPXViewTree(treeView1);
            gpxViewScrollbar = new GPXViewScrollbar(gpxScrollBar);
            gpxViewTimeSlide = new GPXViewTimeSlide(timeSlide1);

            gpxPresenter = new GPXPresenter();
            gpxPresenter.registerView(gpxMapView);
            gpxPresenter.registerView(gpxViewTree);
            gpxPresenter.registerView(gpxViewScrollbar);
            gpxPresenter.registerView(gpxViewTimeSlide);

            mapControl1.Map.InsertionLayer = gpxMapView.o_LayerGPXPolylines;

            // 
            CoordSys oCSWGS84 = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            oCCGPS = new CoordConverter();
            oCCGPS.Init(gpxMapView.o_LayerGPXPolylines.LayerCoordSys, mapControl1.Map.DisplayCoordSys);

            // show zoom=0 (whole world) in scale 1px screen = 1px layer
            Datum datumWGS84 = CoordSysFactory.CreateDatum(DatumID.WGS84);
            double r = datumWGS84.SemiMajorAxis;
            double zoom = mapControl1.Bounds.Width / 256.0 * 2 * Math.PI * r;
            //mapControl1.SetCenterZoom(0.0, 0.0, zoom, mapControl1.Bounds);

            /*
            if (gpxFile.getWptCount() > 0)
            {
                GpxWpt wpt = gpxFile.getWpt(0);
                oCCGPS.Convert(wpt.lon, wpt.lat);
                mapControl1.SetCenterZoom(oCCGPS.X, -oCCGPS.Y, zoom / 256.0, mapControl1.Bounds); // center poland
            }
            else
            {*/
            oCCGPS.Convert(21.0, 52.0);
            mapControl1.SetCenterZoom(oCCGPS.X, -oCCGPS.Y, zoom / 256.0, mapControl1.Bounds); // center poland
            //}
        }



        private void mapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            DPoint pt;
            mapControl1.Map.DisplayTransform.FromDisplay(new System.Drawing.Point(e.X, e.Y), out pt);

            // convert mercator to wgs84
            CoordConverter occ = new CoordConverter();
            CoordSys oCSMercator = CoordSysFactory.CreateCoordSys(CoordSysType.Mercator, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            CoordSys oCSWGS84 = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            occ.Init(oCSMercator, oCSWGS84);
            occ.Convert(pt.X, pt.Y);

            // calculate zoom
            CoordSys layerCoordsys = tileLayer.LayerCoordSys;

            CoordConverter oCC = new CoordConverter();
            oCC.Init(layerCoordsys, mapControl1.Map.DisplayCoordSys);

            // this atPan converts DisplayCoordSys into Screen CoordSys[px]
            // DisplayCoordSys has Y axis up (unless its AT does not change it)
            // Screen Y axis is down
            AffineTransform atPan = new AffineTransform();
            atPan.OffsetInPlace((double)mapControl1.Map.MapOffsetX, (double)mapControl1.Map.MapOffsetY);
            atPan.MultiplyInPlace(mapControl1.Map.MapScale, -mapControl1.Map.MapScale);

            // add screen scale and offset transformation
            oCC.atMaster = oCC.atMaster.Compose(atPan);

            double zoomElevateUpscale = 1024 * 8; //1024;
            double scale = oCC.atMaster.A;

            int zoom = (int)Math.Log(scale * zoomElevateUpscale, 2);

            lblInfo.Text = string.Format("x={0:0.00000}, y={1:0.00000} Mercator: x={2:0}, y={3:0}, zoom={4}", occ.X, occ.Y, pt.X, pt.Y, zoom);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitMapAndPresenter();

            //GPXFile gpxFile = new GPXFile();

            // try to load last GPX file
            String fileName = Properties.Settings.Default.RecentGPX;
            if (fileName != "")
            {
                try
                {
                    gpxPresenter.LoadAndAppend(fileName);
                    //GPXUtils.AppendGPX(gpxFile, fileName);
                }
                catch (Exception) { }
            }
            setCurrentGPX(gpxPresenter.lastGpxFilename);


            //fillTreeWithGPX(gpxFile, treeView1);

            // set pan tool
            mapControl1.CurrentTool = MapControl.ToolConst.PanTool;
        }

        /*
        /// <summary>
        /// Draws bottom chart
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="gpxPoints"></param>
        private void DrawChart(Map map, GPXFile gpxPoints)
        {
            map.Layers.Clear();

            LayerVectors oLayerGPX = new LayerVectors();
            oLayerGPX.LayerCoordSys = CoordSysFactory.CreateCoordSys(CoordSysType.NonEarth, null, new AffineTransform());
            map.Layers.Add(oLayerGPX);

            List<DPoint> oPoints1 = new List<DPoint>();
            for (int i = 0; i < gpxPoints.Count; i++)
            {
                double x = i;
                double y = gpxPoints.wpts[i].speed;
                y = (i % 2 == 0) ? 0 : 90;

                oPoints1.Add(new DPoint(x, y));
            }
            PolylineFeature oPolyline1 = FeatureFactory.CreatePolyline(oPoints1, new Style(Color.Red));
            oLayerGPX.FeaturesInsert(oPolyline1);

            oPoints1 = new List<DPoint>();
            for (int i = 0; i < gpxPoints.Count; i++)
            {
                double x = i;
                double y = gpxPoints.wpts[i].speed / 10;
                if (y < 0) y = 0;
                if (y > 90) y = 90;

                oPoints1.Add(new DPoint(x, y));
            }
            oPolyline1 = FeatureFactory.CreatePolyline(oPoints1, new Style(Color.Blue));
            oLayerGPX.FeaturesInsert(oPolyline1);
        }*/
        /*
        private void DrawChartTeeChart(GPXFile gpxPoints)
        {
            if (gpxPoints == null) return;

            tChart1.Axes.Left.Automatic = false;
            tChart1.Axes.Left.AutomaticMaximum = true;
            tChart1.Axes.Left.AutomaticMinimum = false;
            tChart1.Axes.Left.Minimum = 0;

            tChart1.Series.Clear();

            // speed line
            Line line1 = new Line();
            line1.Brush.Color = Color.Blue;
            line1.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            line1.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            line1.Marks.Callout.ArrowHeadSize = 8;
            line1.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            line1.Marks.Callout.Distance = 0;
            line1.Marks.Callout.Draw3D = false;
            line1.Marks.Callout.Length = 10;
            line1.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            line1.Marks.Font.Shadow.Visible = false;
            line1.Marks.Font.Unit = System.Drawing.GraphicsUnit.World;
            line1.Pointer.Brush.Color = System.Drawing.Color.Red;
            line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            line1.Title = "line1";
            line1.XValues.DataMember = "X";
            line1.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            line1.YValues.DataMember = "Y";
            tChart1.Series.Add(line1);

            Series series = line1;
            series.Clear();

            for (int i = 0; i < gpxPoints.Count; i++)
            {
                double x = i;
                double y = gpxPoints.wpts[i].speed;

                series.Add(x, y);
            }

            // altitude line
            Line line2 = new Line();
            line2.Brush.Color = Color.Black;
            line2.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            line2.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            line2.Marks.Callout.ArrowHeadSize = 8;
            line2.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            line2.Marks.Callout.Distance = 0;
            line2.Marks.Callout.Draw3D = false;
            line2.Marks.Callout.Length = 10;
            line2.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            line2.Marks.Font.Shadow.Visible = false;
            line2.Marks.Font.Unit = System.Drawing.GraphicsUnit.World;
            line2.Pointer.Brush.Color = System.Drawing.Color.Red;
            line2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            line2.Title = "line2";
            line2.XValues.DataMember = "X";
            line2.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            line2.YValues.DataMember = "Y";
            tChart1.Series.Add(line2);

            series = line2;
            series.Clear();

            for (int i = 0; i < gpxPoints.Count; i++)
            {
                double x = i;
                double y = gpxPoints.wpts[i].ele / 10.0;

                series.Add(x, y);
            }


            // set the scale

            hScrollBar1.LargeChange = gpxPoints.Count / 20;
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = gpxPoints.Count;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Axis axis = tChart1.Axes.Bottom;
            axis.Automatic = false;
            axis.Minimum = e.NewValue;
            axis.Maximum = e.NewValue + hScrollBar1.LargeChange;
        }*/

        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            openGPX();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openGPX();
        }

        private void openGPX()
        {
            String fileName = @"d:\rav\Private\rv130_gps_tracks\Data_slady_gps_Logger\sandiego2\2010-04-17_sd.gpx";
            openFileDialog1.FileName = fileName;
            openFileDialog1.Filter = "GPX track files (*.gpx)|*.gpx|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel)
                return;

            fileName = openFileDialog1.FileName;

            gpxPresenter.ClearAll();
            try
            {
                gpxPresenter.LoadAndAppend(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load gpx file. " + e.Message);
            }

            setCurrentGPX(gpxPresenter.lastGpxFilename);

        }



        /// <summary>
        /// Call this after GPX was openned
        /// </summary>
        private void setCurrentGPX(String fileName)
        {
            m_currentGPXfileName = fileName;

            if (fileName == "")
            {
                this.Text = AssemblyTitle;
            }
            else
            {
                this.Text = AssemblyTitle + " [" + fileName + " ]";
            }
            Settings.Default.RecentGPX = fileName;
            Settings.Default.Save();
        }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        private void toolStripButton_ZoomIn_Click(object sender, EventArgs e)
        {
            mapControl1.SetCenterZoom(mapControl1.CenterX, mapControl1.CenterY, mapControl1.Zoom / 2, mapControl1.Bounds);
        }

        private void toolStripButton_ZoomOut_Click(object sender, EventArgs e)
        {
            mapControl1.SetCenterZoom(mapControl1.CenterX, mapControl1.CenterY, mapControl1.Zoom * 2, mapControl1.Bounds);
        }

        private void toolStripButton_GM_Click(object sender, EventArgs e)
        {
            toolStripButton_GM.Checked = true;
            toolStripButton_GS.Checked = false;
            toolStripButton_BM.Checked = false;
            toolStripButton_OSM.Checked = false;
            toolStripButton_UMP.Checked = false;

            if (tileLayer is LayerTiles) ((LayerTiles)tileLayer).MapType = MapType.GoogleMap; /* todo: wymaga gmap */
            if (tileLayer is LayerTilesAsynch) ((LayerTilesAsynch)tileLayer).MapType = MapType.GoogleMap; /* todo: wymaga gmap */
            mapControl1.InvalidateMap();
        }

        private void toolStripButton_GS_Click(object sender, EventArgs e)
        {
            toolStripButton_GM.Checked = false;
            toolStripButton_GS.Checked = true;
            toolStripButton_BM.Checked = false;
            toolStripButton_OSM.Checked = false;
            toolStripButton_UMP.Checked = false;

            if (tileLayer is LayerTiles) ((LayerTiles)tileLayer).MapType = MapType.GoogleSatellite; /* todo: wymaga gmap */
            if (tileLayer is LayerTilesAsynch) ((LayerTilesAsynch)tileLayer).MapType = MapType.GoogleSatellite; /* todo: wymaga gmap */
            mapControl1.InvalidateMap();
        }

        private void toolStripButton_BM_Click(object sender, EventArgs e)
        {
            toolStripButton_GM.Checked = false;
            toolStripButton_GS.Checked = false;
            toolStripButton_BM.Checked = true;
            toolStripButton_OSM.Checked = false;
            toolStripButton_UMP.Checked = false;

            if (tileLayer is LayerTiles) ((LayerTiles)tileLayer).MapType = MapType.BingSatellite; /* todo: wymaga gmap */
            if (tileLayer is LayerTilesAsynch) ((LayerTilesAsynch)tileLayer).MapType = MapType.BingSatellite; /* todo: wymaga gmap */
            mapControl1.InvalidateMap();
        }

        private void toolStripButton_OSM_Click(object sender, EventArgs e)
        {
            toolStripButton_GM.Checked = false;
            toolStripButton_GS.Checked = false;
            toolStripButton_BM.Checked = false;
            toolStripButton_OSM.Checked = true;
            toolStripButton_UMP.Checked = false;

            if (tileLayer is LayerTiles) ((LayerTiles)tileLayer).MapType = MapType.OpenStreetMap; /* todo: wymaga gmap */
            if (tileLayer is LayerTilesAsynch) ((LayerTilesAsynch)tileLayer).MapType = MapType.OpenStreetMap; /* todo: wymaga gmap */
            mapControl1.InvalidateMap();
        }

        private void toolStripButton_UMP_Click(object sender, EventArgs e)
        {
            toolStripButton_GM.Checked = false;
            toolStripButton_GS.Checked = false;
            toolStripButton_BM.Checked = false;
            toolStripButton_OSM.Checked = false;
            toolStripButton_UMP.Checked = true;

            if (tileLayer is LayerTiles) ((LayerTiles)tileLayer).MapType = MapType.UMP; /* todo: wymaga gmap */
            if (tileLayer is LayerTilesAsynch) ((LayerTilesAsynch)tileLayer).MapType = MapType.UMP; /* todo: wymaga gmap */
            mapControl1.InvalidateMap();
        }
        /*
        private void toolStripButton_GPS_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                ToolStripButton checkbox = sender as ToolStripButton;
                if (checkbox.Checked)
                {
                    //checkbox.Text = "Checked";
                    comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                    GPS.PositionReceived += new NmeaInterpreter.PositionReceivedEventHandler(GPS_PositionReceived);

                    // Open the port
                    comport.Open();
                }
                else
                {
                    //checkbox.Text = "UnChecked";
                    comport.Close();
                }
            }
        }
        */
        private void toolStripButton_Arrow_Click(object sender, EventArgs e)
        {
            mapControl1.CurrentTool = MapControl.ToolConst.CustomToolProcessor;
            mapControl1.CurrentToolProcessor = gpxMapView.toolSelectMove;

            MapToolSelectMove mt = (MapToolSelectMove)mapControl1.CurrentToolProcessor;
            mt.PolylineLayer = gpxMapView.o_LayerGPXPolylines;
            mt.SymbolLayer = gpxMapView.o_LayerGPXSymbols;

            mapControl1.CurrentToolProcessor.ToolDragShape = MapControl.ToolDragShapeConst.Rectangle;
            mapControl1.Cursor = Cursors.Arrow;
        }

        private void toolStripButton_Pan_Click(object sender, EventArgs e)
        {
            mapControl1.CurrentTool = MapControl.ToolConst.PanTool;
        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {/*
            // read position on the chart
            int dx = (int)tChart1.Series[0].XScreenToValue(e.X); // wsp. X
            int pos1 = tChart1.Series[0].CalcXPosValue(dx);

            // position the map
            double x = track.wpts[dx].lon;
            double y = track.wpts[dx].lat;

            oCCGPS.Convert(x, y);
            mapControl1.SetCenterZoom(oCCGPS.X, -oCCGPS.Y, mapControl1.Zoom, mapControl1.Bounds); // center poland
          */
        }

        private void tChart1_MouseMove(object sender, MouseEventArgs e)
        {/*
            if (e.Button == MouseButtons.Left)
            {
                // read position on the chart
                int dx = (int)tChart1.Series[0].XScreenToValue(e.X); // wsp. X
                int pos1 = tChart1.Series[0].CalcXPosValue(dx);

                // position the map
                if (track.wpts.Count > dx)
                {
                    double x = track.wpts[dx].lon;
                    double y = track.wpts[dx].lat;

                    oCCGPS.Convert(x, y);
                    mapControl1.SetCenterZoom(oCCGPS.X, -oCCGPS.Y, mapControl1.Zoom, mapControl1.Bounds); // center poland
                }
            }*/
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test coordsys
            Datum datumEllipse = CoordSysFactory.CreateDatum(DatumID.WGS84);
            Datum datumSphere = CoordSysFactory.CreateDatum(Ellipsoid.Sphere, 0, 0, 0, 0, 0, 0, 0, 0);
            CoordSys source = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, datumEllipse, new AffineTransform());
            CoordSys target = CoordSysFactory.CreateCoordSys(CoordSysType.Mercator, datumEllipse, new AffineTransform());

            CoordConverter oCC = new CoordConverter();
            oCC.Init(source, target);

            oCC.Convert(21, 52);
            double x = oCC.X;  // 2337709.3066587453
            double y = oCC.Y;  // 6800125.4543973068

            // (21, 52) -> (2337709.3066587453, 6800125.4543973068)  (alg. Sphere)
            // (21, 52) -> (2337709.3066587453, 6766432.7231710562)  (alg. Ellipse)

            // (21, 52) -> (2337709.3066587453, 6800078.8362877583)  (DatumConv + Ellipse)
            // (21, 52) -> (2337709.3066587453, 6800125.4543973068)  (alg. Ellipse , data Sphere)
        }

        private void toolStripButton_info_Click(object sender, EventArgs e)
        {
            //            mapControl1.CurrentTool = MapControl.ToolConst.InfoTool;

            // detemine active layers
            List<LayerAbstract> layers = new List<LayerAbstract>();
            layers.Add(gpxMapView.o_LayerGPXSymbols);

            mapControl1.CurrentTool = MapControl.ToolConst.CustomToolProcessor;
            mapControl1.CurrentToolProcessor = MapTools.MapToolInfo(layers);
            mapControl1.CurrentToolProcessor.ToolDragShape = MapControl.ToolDragShapeConst.Point;
            mapControl1.Cursor = Cursors.Cross;
        }

        private void mapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (mapControl1.CurrentTool == MapControl.ToolConst.InfoTool)
            {
                // get gpx coordinate (from map or from gpx table)

                DPoint pt;
                mapControl1.Map.DisplayTransform.FromDisplay(new System.Drawing.Point(e.X, e.Y), out pt);

                // convert mercator to wgs84
                CoordConverter occ = new CoordConverter();
                CoordSys oCSMercator = CoordSysFactory.CreateCoordSys(CoordSysType.Mercator, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
                CoordSys oCSWGS84 = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
                occ.Init(oCSMercator, oCSWGS84);
                occ.Convert(pt.X, pt.Y);
                lblInfo.Text = string.Format("x={0:0.00000}, y={1:0.00000} Mercator: x={2:0}, y={3:0}", occ.X, occ.Y, pt.X, pt.Y);

                // search for features
                //Feature[] fts = oLayerPointer.Search(pt);



                //e.X 

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //mapControl1.CurrentTool = MapControl.ToolConst.AddFeatureTool;

            mapControl1.CurrentTool = MapControl.ToolConst.CustomToolProcessor;
            mapControl1.CurrentToolProcessor = MapTools.MapToolAddEdit;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl1.CurrentToolProcessor is MapToolSelectMove)
            {
                MapToolSelectMove tool = (MapToolSelectMove)mapControl1.CurrentToolProcessor;
                tool.PolylineLayer.Manager.playUndo();
                tool.SymbolLayer.Manager.playUndo();
            }
            else
            {
                mapControl1.Map.InsertionLayer.Manager.playUndo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl1.Map.InsertionLayer.Manager.playRedo();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // copty data from Layers into gpxFile object
            GPXFile gpxFile = GPXUtils.makeGPXfromMapLayers(gpxMapView.o_LayerGPXPolylines, gpxMapView.o_LayerGPXSymbols);

            // save to disk *.gpx file
            GPXUtils.SaveGPXv11(gpxFile, m_currentGPXfileName);
        }

        private void stripTimeStampToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPXFile gpxFile = GPXUtils.makeGPXfromMapLayers(gpxMapView.o_LayerGPXPolylines, gpxMapView.o_LayerGPXSymbols);

            // remove timestamps
            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        wpt.time = DateTime.MinValue;
                    }
                }
            }

            gpxMapView.o_LayerGPXPolylines.FeaturesClear();
            gpxMapView.o_LayerGPXSymbols.FeaturesClear();

            GPXUtils.makeMapLayersFromGPX(gpxFile, gpxMapView.o_LayerGPXPolylines, gpxMapView.o_LayerGPXSymbols);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // use Modified marker

            gpxPresenter.ClearAll();

            setCurrentGPX("");

            Settings.Default.RecentGPX = "";
            Settings.Default.Save();
        }
        /*
        private void gpxScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int value = gpxScrollBar.Value;
            int pos = 0;

            DPoint point_current = null;

            // select this point
            for (int i = 0; i < o_LayerGPXPolylines.FeaturesCount; i++)
            {
                Feature f = o_LayerGPXPolylines.FeatureGet(i);
                if (f is PolylineFeature)
                {
                    PolylineFeature poly = (PolylineFeature)f;
                    for (int ipart = 0; ipart < poly.m_oParts.Count; ipart++)
                    {
                        Part part = poly.m_oParts[ipart];
                        if (value >= pos && value < pos + part.Points.Count)
                        {
                            point_current = part.Points[value - pos];

                            double x = point_current.X;
                            double y = point_current.Y;
                            double zoom = mapControl1.Zoom;

                            oCCGPS.Convert(x, y);
                            mapControl1.SetCenterZoom(oCCGPS.X, -oCCGPS.Y, zoom, mapControl1.Bounds); // center poland
                            break;
                        }

                        pos += part.Points.Count;
                    }
                }
                if (point_current != null) break;
            }
            if (point_current == null) return;

            SymbolFeature symbol = (SymbolFeature)point_current.UserObject;

            double lon = point_current.X;
            double lat = point_current.Y;
            double ele = (double)symbol.getField("ele");
            DateTime time = (DateTime)symbol.getField("time");

            lblSelection.Text = time.ToString() + " (" + value + "/" + gpxScrollBar.Maximum + ")";

            o_LayerGPXPolylines.selectionClear();
            point_current.Selected = true;
        }
        */
        /*
        private void selectMoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // select points from first selection onward
            // select this point

            bool selectionFound = false;
            for (int i = 0; i < o_LayerGPXPolylines.FeaturesCount; i++)
            {
                Feature f = o_LayerGPXPolylines.FeatureGet(i);
                if (f is PolylineFeature)
                {
                    PolylineFeature poly = (PolylineFeature)f;
                    for (int ipart = 0; ipart < poly.m_oParts.Count; ipart++)
                    {
                        Part part = poly.m_oParts[ipart];
                        for (int ipoint = 0; ipoint < part.Points.Count; ipoint++)
                        {
                            if (selectionFound)
                            {
                                part.Points[ipoint].Selected = true;
                            }
                            else if (part.Points[ipoint].Selected == true)
                            {
                                selectionFound = true;
                            }
                        }
                    }
                }
            }
            MapControl.Globals.Instance.MapControl.InvalidateMap();
        }*/

        private void inverseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gpxPresenter.inverseSelection();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gpxPresenter.selectAll();
        }

        private void openMoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String fileName = @"d:\rav\Private\rv130_gps_tracks\Data_slady_gps_Logger\sandiego2\2010-04-17_sd.gpx";
            openFileDialog1.FileName = fileName;
            openFileDialog1.Filter = "GPX track files (*.gpx)|*.gpx|All files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel)
                return;

            // load GPX file
            GPXFile gpxFile = new GPXFile();
            foreach (string file in openFileDialog1.FileNames)
            {
                try
                {
                    gpxPresenter.LoadAndAppend(file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load gpx file. " + ex.Message);
                }
            }

            if (openFileDialog1.FileNames.GetLength(0) > 0)
                fileName = openFileDialog1.FileNames[0];

            setCurrentGPX(fileName);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ask for name
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "GPX file (*.gpx)|*.gpx|All files (*.*)|(*.*)";
            dlg.FileName = m_currentGPXfileName;
            dlg.ShowDialog();
            string fileName = dlg.FileName;

            gpxPresenter.Save(fileName);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // calculate length of gpx
            PropertiesGPX frm = new PropertiesGPX();
            frm.fileName = m_currentGPXfileName;

            double len = GPXUtils.CalcLength(gpxPresenter.gpxFile);
            if (len < 1000) // less than 1km
            {  // [m]
                frm.len = len.ToString("0") + " m";
            }
            else
            { // [km]
                double lenKM = len / 1000;
                frm.len = lenKM.ToString("0.000") + " km";
            }

            // time span
            frm.timeSpan = GPXUtils.TimeSpan(gpxPresenter.gpxFile);

            frm.ShowDialog();
        }

        private void tileDownloaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // auto download tiles

            // calculate x,y, range
            // calculate zoom range

            CoordSys layerCoordsys = tileLayer.LayerCoordSys;

            CoordConverter oCC = new CoordConverter();
            oCC.Init(layerCoordsys, mapControl1.Map.DisplayCoordSys);

            // this atPan converts DisplayCoordSys into Screen CoordSys[px]
            // DisplayCoordSys has Y axis up (unless its AT does not change it)
            // Screen Y axis is down
            AffineTransform atPan = new AffineTransform();
            atPan.OffsetInPlace((double)mapControl1.Map.MapOffsetX, (double)mapControl1.Map.MapOffsetY);
            atPan.MultiplyInPlace(mapControl1.Map.MapScale, -mapControl1.Map.MapScale);

            // add screen scale and offset transformation
            oCC.atMaster = oCC.atMaster.Compose(atPan);

            double zoomElevateUpscale = 1024 * 8; //1024;
            double scale = oCC.atMaster.A;

            int zoom = (int)Math.Log(scale * zoomElevateUpscale, 2);

            TileDownloader downloader = new TileDownloader();
            downloader.Download(zoom, ((LayerTilesAsynch)tileLayer).MapType, oCC, new System.Drawing.Rectangle(0, 0, mapControl1.Width, mapControl1.Height));
        }


    }
}
