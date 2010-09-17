using System;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Diagnostics;

namespace hiMapNet
{
    public class LayerTiles : LayerAbstract
    {
        private MapType mapType = MapType.GoogleMap;

        public MapType MapType
        {
            get { return mapType; }
            set { mapType = value; /* todo: invalide */ }
        }

        private double zoomElevateUpscale = 1024 * 8; //1024;

        public LayerTiles()
        {
            Datum datumWGS84Sphere = CoordSysFactory.CreateDatum(Ellipsoid.Sphere, 0, 0, 0, 0, 0, 0, 0, 0);

            double r = datumWGS84Sphere.SemiMajorAxis;

            // scale =256/360, offset=128, this is calculated for zoom=0 (one tile)
            // converts merc deg (-180;180 , -90;90) into pixels (0;256 , 0;256)
            AffineTransform affineTransform = new AffineTransform();
            //zoomElevate = 1024; // zwieksz gestosc wspolrzednych o 10 pozomow zoom
            //            affineTransform.MultiplyInPlace(256.0 / 360.0 * zoomElevateUpscale, -256.0 / 360.0 * zoomElevateUpscale);
            //            affineTransform.OffsetInPlace(-128, 128);

            affineTransform.MultiplyInPlace(256.0 / (2.0 * Math.PI * r) * zoomElevateUpscale, -256.0 / (2.0 * Math.PI * r) * zoomElevateUpscale);
            affineTransform.OffsetInPlace(128 * zoomElevateUpscale, 128 * zoomElevateUpscale);

            layerCoordSys = new CoordSys(CoordSysType.Mercator, datumWGS84Sphere, affineTransform);

            // depending o tile zoom lever (1,2,3,...,17) there is different AT

            // tiles loader
            WindowsFormsImageProxy wimg = new WindowsFormsImageProxy();
            GMaps.Instance.ImageProxy = wimg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">Graphics na ktorym robimy rysunek w ukladzie wspolrzenych screen</param>
        /// <param name="Rect">Obszar ekranu do odrysowania we wsp. screen</param>
        /// <param name="oCC">obsolete</param>
        /// <param name="oCC1">converter layer->screen</param>
        public override void Draw(Graphics g, System.Drawing.Rectangle Rect, CoordConverter oCC)
        {
            base.Draw(g, Rect, oCC);

            // produce a list of needed tiles

            // ustal dla jakiego zoom wyswietlac bedziemy kafelki

            if (oCC.atMaster.IsRotating()) return; //can not display
            double scale = oCC.atMaster.A;
            //System.Diagnostics.Debug.Print("Scale={0}", scale);
            // scale = 1,2,4,8

            int zoom = (int)Math.Log(scale * zoomElevateUpscale, 2);

            // ustal uklad wsp. w jakim są przechowywane kafelki
            // jest to inny uklad w kazdym zoomie
            // zoom=0 -> 0;0 - 256;256 (2^0=1)   0x100
            // zoom=1 -> 0;0 - 512;512 (2^1=2)   0x200
            // zoom=2 -> 0;0 - 1024;1024 (2^2=4) 0x400
            // ...
            // zoom=17 -> 0;0 - 33554432;33554432 (2^17=131072) 0x02000000 (32bit)
            // zoom=18 -> 
            // zoom=19 -> 

            // oblicz prostokat widocznosci w ukladzie kafelkow (z Display Coordsys do Layer Coordsys)
            int x1, y1;
            oCC.ConvertInverse(Rect.X, Rect.Y);
            x1 = (int)oCC.X;
            y1 = (int)oCC.Y;
            int x2, y2;
            oCC.ConvertInverse(Rect.X + Rect.Width, Rect.Y + Rect.Height);
            x2 = (int)oCC.X;
            y2 = (int)oCC.Y;

            if (x1 > x2) { int tmp = x1; x1 = x2; x2 = tmp; }
            if (y1 > y2) { int tmp = y1; y1 = y2; y2 = tmp; }

            // zoom=0 -> 256
            // zoom=1 -> 128
            // zoom=2 -> 64

            int tileSizePx = (int)Math.Round(256.0 / scale);  // zoom=0 -> 256 * zoomElevate
            if (tileSizePx < 1) return;

            // okresl liste kafelkow do wyswietlenia
            int xx1 = (x1 / tileSizePx) * tileSizePx;
            int yy1 = (y1 / tileSizePx) * tileSizePx;

            int xx2 = (x2 / tileSizePx) * tileSizePx + tileSizePx;
            int yy2 = (y2 / tileSizePx) * tileSizePx + tileSizePx;

            int max = (int)(scale * zoomElevateUpscale);

            int count = 0;
            for (int x = xx1; x < xx2; x += tileSizePx)
            {
                for (int y = yy1; y < yy2; y += tileSizePx)
                {
                    int picx = x / tileSizePx;
                    int picy = y / tileSizePx;
                    int picZoom = zoom;

                    if (count < 66 && picx >= 0 && picy >= 0 && picx < max && picy < max)
                    {
                        DrawImageGMap(x, y, (int)(tileSizePx), (int)(tileSizePx), g, oCC, picx, picy, picZoom);
                    }
                    //DrawEllipse(x, y, tileSizePx, tileSizePx, g, oCC1);
                    count++;
                }
            }
        }

        private void DrawImageGMap(int x1, int y1, int w, int h, Graphics g, CoordConverter oCC, int picx, int picy, int picZoom)
        {
            PureImage img = null;
            try
            {
                Exception result;
                img = GMaps.Instance.GetImageFrom(mapType, new GMap.NET.Point(picx, picy), picZoom, out result);
            }
            catch (Exception)
            {

            }
            WindowsFormsImage imgWin = (WindowsFormsImage)img;
            if (imgWin == null) return;

            // bitmap position
            oCC.Convert(x1, y1);
            int xScr = (int)Math.Round(oCC.X);
            int yScr = (int)Math.Round(oCC.Y);
            oCC.Convert(x1 + w, y1 + h);
            int xScrWidth = (int)Math.Round(oCC.X - xScr);
            int yScrWidth = (int)Math.Round(oCC.Y - yScr);

            if (xScrWidth < 0)
            {
                xScrWidth = -xScrWidth;
                xScr = xScr - xScrWidth;
            }
            if (yScrWidth < 0)
            {
                yScrWidth = -yScrWidth;
                yScr = yScr - yScrWidth;
            }

            if (img != null)
            {
                Debug.Assert(xScrWidth == 256);
                Debug.Assert(yScrWidth == 256);

                g.DrawImage(imgWin.Img, xScr, yScr, xScrWidth, yScrWidth);
            }
            else
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("hiMapNet.Resources.h0.jpeg");
                Bitmap bmp = new Bitmap(myStream);
                g.DrawImage(bmp, xScr, yScr, xScrWidth, yScrWidth);
                bmp.Dispose();
            }
        }

        private void DrawEllipse(int x1, int y1, int w, int h, Graphics g, CoordConverter oCC1)
        {
            // bitmap position
            oCC1.Convert(x1, y1);
            int xScr = (int)oCC1.X;
            int yScr = (int)oCC1.Y;
            oCC1.Convert(x1 + w, y1 + h);
            int xScrWidth = (int)(oCC1.X - xScr);
            int yScrWidth = (int)(oCC1.Y - yScr);

            g.DrawEllipse(Pens.Red, xScr, yScr, xScrWidth, yScrWidth);
            string s = string.Format("x={0}, y={1}", x1, y1);
            g.DrawString(s, new Font("Arial", 16), Brushes.Green, xScr + 64, yScr + 128, StringFormat.GenericDefault);
        }
    }
}
