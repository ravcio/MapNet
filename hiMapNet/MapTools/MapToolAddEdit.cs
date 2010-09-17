using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hiMapNet
{
    public class MapToolAddEdit : MapTool
    {

        override public void MouseClick(object sender, MouseEventArgs e)
        {
            if (map.InsertionLayer == null) return;

            CoordSys layerCoordsys = map.InsertionLayer.LayerCoordSys;

            CoordConverter oCC = new CoordConverter();
            oCC.Init(layerCoordsys, map.DisplayCoordSys);

            // this atPan converts DisplayCoordSys into Screen CoordSys[px]
            // DisplayCoordSys has Y axis up (unless its AT does not change it)
            // Screen Y axis is down
            AffineTransform atPan = new AffineTransform();
            atPan.OffsetInPlace((double)map.MapOffsetX, (double)map.MapOffsetY);
            atPan.MultiplyInPlace(map.MapScale, -map.MapScale);

            // add screen scale and offset transformation
            oCC.atMaster = oCC.atMaster.Compose(atPan);

            oCC.ConvertInverse(e.X, e.Y);

            DPoint pt = new DPoint(oCC.X, oCC.Y);
            // szukaj w tym miejscu feature
            List<Feature> ftrs = map.InsertionLayer.Search(pt);

            if (ftrs.Count == 0)
            {
                Feature oF = FeatureFactory.CreateSymbol(oCC.X, oCC.Y);
                map.InsertionLayer.FeaturesAdd(oF);
            }

            MapControl.Globals.Instance.MapControl.InvalidateMap();
        }


        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);

        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            // highlight polilines and points

            if (map.InsertionLayer == null) return;

            CoordSys layerCoordsys = map.InsertionLayer.LayerCoordSys;

            CoordConverter oCC = new CoordConverter();
            oCC.Init(layerCoordsys, map.DisplayCoordSys);

            // this atPan converts DisplayCoordSys into Screen CoordSys[px]
            // DisplayCoordSys has Y axis up (unless its AT does not change it)
            // Screen Y axis is down
            AffineTransform atPan = new AffineTransform();
            atPan.OffsetInPlace((double)map.MapOffsetX, (double)map.MapOffsetY);
            atPan.MultiplyInPlace(map.MapScale, -map.MapScale);

            // add screen scale and offset transformation
            oCC.atMaster = oCC.atMaster.Compose(atPan);

            int margin = 5;

            oCC.ConvertInverse(e.X, e.Y);
            DPoint pt_center = new DPoint(oCC.X, oCC.Y);
            oCC.ConvertInverse(e.X - margin, e.Y - margin);
            DPoint pt1 = new DPoint(oCC.X, oCC.Y);
            oCC.ConvertInverse(e.X + margin, e.Y + margin);
            DPoint pt2 = new DPoint(oCC.X, oCC.Y);
            // szukaj w tym miejscu feature
            //List<Feature> ftrs = map.InsertionLayer.Search(pt);

            // construct search rectangle (10px wide)
            DRect rect = new DRect(pt1.X, pt2.Y, pt2.X, pt1.Y);

            //map.InsertionLayer.SelectWithinRectangle(rect);
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);

        }

        public override void MouseWheel(object sender, MouseEventArgs e)
        {
            base.MouseWheel(sender, e);
        }
    }
}
