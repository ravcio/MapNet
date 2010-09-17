using System;
using System.Collections.Generic;
using System.Text;
using hiMapNet;
using System.Data;
using System.Drawing;
using System.Diagnostics;

namespace gpxEditor
{
    public class GPXViewMap : IGPXView
    {
        // working MapNet Layer
        public LayerVectors o_LayerGPXPolylines = null;
        public LayerVectors o_LayerGPXSymbols = null;
        public LayerVectors o_LayerGPXAnimation = null;

        // selection tool
        public MapToolSelectMove toolSelectMove;
        

        CoordConverter oCCGPS = new CoordConverter();
        MapControl mapControl1 = null;

        public GPXViewMap(MapControl mapControl1)
        {
            this.mapControl1 = mapControl1;
            Layers layers = mapControl1.Map.Layers;

            toolSelectMove = new MapToolSelectMove();
            toolSelectMove.ToolUsed += new EventHandler(toolSelectMove_ToolUsed);

            // Verctor layers (polylines and points with gpx data)
            o_LayerGPXPolylines = new LayerVectors();
            o_LayerGPXSymbols = new LayerVectors();

            DataTable dt = new DataTable();
            dt.Columns.Add("_feature_id", typeof(System.Int32));
            dt.Columns.Add("lat", typeof(System.Double));
            dt.Columns.Add("lon", typeof(System.Double));
            dt.Columns.Add("ele", typeof(System.Double));
            dt.Columns.Add("time", typeof(System.DateTime));
            dt.Columns.Add("name", typeof(System.String));
            dt.Columns.Add("cmt", typeof(System.String));
            dt.Columns.Add("desc", typeof(System.String));
            dt.Columns.Add("fix", typeof(System.String));
            dt.Columns.Add("sat", typeof(System.Int32));
            dt.Columns.Add("hdop", typeof(System.Double));
            dt.Columns.Add("vdop", typeof(System.Double));
            dt.Columns.Add("pdop", typeof(System.Double));
            o_LayerGPXSymbols.DataTable = dt;

            layers.Add(o_LayerGPXPolylines);
            layers.Add(o_LayerGPXSymbols);

            oCCGPS = new CoordConverter();
            oCCGPS.Init(o_LayerGPXPolylines.LayerCoordSys, mapControl1.Map.DisplayCoordSys);

            // animation layer
            o_LayerGPXAnimation = new LayerVectors();
            layers.Add(o_LayerGPXAnimation);
            layers.AnimationLayer = o_LayerGPXAnimation;
        }

        void toolSelectMove_ToolUsed(object sender, EventArgs e)
        {
            // this makes new object
            GPXFile gpxFileNew = GPXUtils.makeGPXfromMapLayers(o_LayerGPXPolylines, o_LayerGPXSymbols);

            // copy to original
            gpxFile.trks.Clear();
            gpxFile.trks.AddRange(gpxFileNew.trks);

            if (ChangedData != null) ChangedData(this, new EventArgs());
        }

        #region IGPXView Members
        
        GPXFile gpxFile;
        public void Bind(GPXFile gpxFile)
        {
            this.gpxFile = gpxFile;
        }

        public void Repaint()
        {
            GPXUtils.makeMapLayersFromGPX(gpxFile, o_LayerGPXPolylines, o_LayerGPXSymbols);
            o_LayerGPXAnimation.FeaturesClear();
        }

        public void NewLocation()
        {
            // scroll the map
            GpxWpt wpt = gpxFile.location;
            if (wpt != null)
            {
                // scroll map
                oCCGPS.Convert(wpt.lon, wpt.lat);
                double x = oCCGPS.X;
                double y = -oCCGPS.Y;
                //mapControl1.SetCenterZoom(oCCGPS.X, -oCCGPS.Y, mapControl1.Zoom, mapControl1.Bounds); // center poland

                // draw marker
                o_LayerGPXAnimation.FeaturesClear();

                Feature oSymbol = FeatureFactory.CreateSymbol(wpt.lon, wpt.lat, (uint)0xFFFF0000);
                o_LayerGPXAnimation.FeaturesAdd(oSymbol);

                Feature oBitmap = FeatureFactory.CreateBitmap(wpt.lon, wpt.lat, "baloon");
                o_LayerGPXAnimation.FeaturesAdd(oBitmap);

            }
        }

        public event EventHandler ChangedData;
        public event EventHandler ChangedLocation;

        #endregion
    }
}
