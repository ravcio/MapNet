using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace hiMapNet
{
    public class MapToolInfo : MapTool
    {
        List<LayerAbstract> layers = null;

        // tooltip
        private System.Windows.Forms.ToolTip toolTip;
        protected int _ToolTipInterval = 3000;

        string tooltipText = "";


        public MapToolInfo(List<LayerAbstract> layers)
        {
            this.layers = layers;

            //            this.toolTip = new System.Windows.Forms.ToolTip(MapControl.Globals.Instance.MapControl.comp this.components);
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            this.toolTip.ShowAlways = true;
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            // find polyline point
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i] is LayerVectors)
                {
                    LayerVectors layer = (LayerVectors)layers[i];

                    // search for feature under cursor

                    // try to selec polyline point
                    DRect rect = calculateRectangleFromPoint(e.X, e.Y);

                    List<Feature> features = layer.SearchForFeaturesColliding(rect);

                    if (features.Count > 0)
                    {
                        Feature f = features[0];
                        object value = f.getField("time");

                        Debug.Print("time=" + value);
                        // show tooltip

                        tooltipText = value.ToString();

                    }
                }
            }
        }

        public override void MouseHover(object sender, EventArgs e)
        {
            base.MouseHover(sender, e);

            toolTip.Show(tooltipText, MapControl.Globals.Instance.MapControl,
                m_oMouseCurrent.X, m_oMouseCurrent.Y + 5, _ToolTipInterval);
        }


        private DRect calculateRectangleFromPoint(int x, int y)
        {
            // try to select a polyline point
            int margin = 3;

            System.Drawing.Point p1 = new System.Drawing.Point(x - margin, y - margin);
            System.Drawing.Point p2 = new System.Drawing.Point(x + margin, y + margin);

            DPoint pt1, pt2;
            map.DisplayTransform.FromDisplay(p1, out pt1);
            map.DisplayTransform.FromDisplay(p2, out pt2);

            // convert mercator to wgs84
            CoordConverter occ = new CoordConverter();
            CoordSys oCSMercator = CoordSysFactory.CreateCoordSys(CoordSysType.Mercator, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            CoordSys oCSWGS84 = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            occ.Init(oCSMercator, oCSWGS84);
            occ.Convert(pt1.X, pt1.Y);
            DPoint dp1 = new DPoint(occ.X, occ.Y);
            occ.Convert(pt2.X, pt2.Y);
            DPoint dp2 = new DPoint(occ.X, occ.Y);
            DRect rect = new DRect(dp1.X, dp2.Y, dp2.X, dp1.Y);
            return rect;
        }
    }
}
