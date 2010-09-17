using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace hiMapNet
{
    public class FeatureFactory
    {
        public PolylineFeature CreatePolyline(List<DPoint> oPoints)
        {
            return new PolylineFeature(oPoints, new Style());
        }
        static public PolylineFeature CreatePolyline(List<DPoint> oPoints, Style oStyle)
        {
            return new PolylineFeature(oPoints, oStyle);
        }

        static public PolygonFeature CreatePolygon(List<DPoint> oPoints, Style oStyle)
        {
            return new PolygonFeature(oPoints, oStyle);
        }

        static public TextFeature CreateText(DPoint oPoint, string Text, Style oStyle)
        {
            return new TextFeature(oPoint, Text, oStyle);
        }

        public void CreateEllipse()
        {
            throw new System.NotImplementedException();
        }

        public void CreateArc()
        {
            throw new System.NotImplementedException();
        }

        static public RectangleFeature CreateRectangle(double x, double y, double w, double h)
        {
            return new RectangleFeature(x, y, w, h);
        }

        public void CreateText()
        {
            throw new System.NotImplementedException();
        }

        static public Feature CreateSymbol(double x, double y)
        {
            return new SymbolFeature(x, y);
        }
        static public Feature CreateSymbol(double x, double y, uint color)
        {
            return new SymbolFeature(x, y, color);
        }

        public static Feature CreateBitmap(double x, double y, string name)
        {
            if (name == "baloon")
            {
                string sResName = "hiMapNet.Resources.marker_greenA.png";
                
                Stream stream = System.Reflection.Assembly.
                    GetExecutingAssembly().GetManifestResourceStream(sResName);
                Bitmap bmp = new Bitmap(stream);
                stream.Close();

                int anchorX = 8;
                int anchorY = 33;
//                BitmapFeature bf = new BitmapFeature(x, y, anchorX, anchorY, 
//                    @"d:\rav\Private\rv125_GPS_Software\Prog_MapNet\Prog_MapNet\res_google\marker_greenA.png");
                BitmapFeature bf = new BitmapFeature (x, y, anchorX, anchorY, bmp);

                return bf;
            }
            return null;
        }
    }
}
