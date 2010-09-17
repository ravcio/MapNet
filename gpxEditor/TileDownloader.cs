using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using hiMapNet;
using System.Diagnostics;

namespace gpxEditor
{
    class TileDownloader
    {
        private const double zoomElevateUpscale = 1024 * 8; //1024;

        public void Download(int zoom, MapType mapType, CoordConverter oCC, System.Drawing.Rectangle Rect)
        {

            // produce a list of needed tiles

            // ustal dla jakiego zoom wyswietlac bedziemy kafelki
            for (int enlarge = 0; enlarge < 7; enlarge++)
            {
                DownloadZoom(zoom, enlarge, mapType, oCC, Rect);
            }
            //System.Windows.Forms.MessageBox
        }


        public void DownloadZoom(int zoom, int enlarge, MapType mapType, CoordConverter oCC, System.Drawing.Rectangle Rect)
        {

            int enlargeMul = 1 << enlarge;

            // produce a list of needed tiles

            // ustal dla jakiego zoom wyswietlac bedziemy kafelki

            if (oCC.atMaster.IsRotating()) return; //can not display
            double scale = oCC.atMaster.A * enlargeMul;
            //System.Diagnostics.Debug.Print("Scale={0}", scale);
            // scale = 1,2,4,8

            int zoom1 = (int)Math.Log(scale * zoomElevateUpscale, 2);

            int tileSizePx = (int)Math.Round(256.0 / scale);  // zoom=0 -> 256 * zoomElevate
            if (tileSizePx < 1) return;


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


            // zaokraglij wsp. w ukl mapy do pelnych rozmiarow kafelka
            int xx1 = (x1 / tileSizePx) * tileSizePx;
            int yy1 = (y1 / tileSizePx) * tileSizePx;

            int xx2 = (x2 / tileSizePx) * tileSizePx + tileSizePx;
            int yy2 = (y2 / tileSizePx) * tileSizePx + tileSizePx;


            // cache
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                xx1 / tileSizePx, yy1 / tileSizePx,
                (xx2 - xx1) / tileSizePx, (yy2 - yy1) / tileSizePx);

            int max = (int)(scale * zoomElevateUpscale);

            int xcount = xx2 / tileSizePx - xx1 / tileSizePx ;
            int ycount = yy2 / tileSizePx - yy1 / tileSizePx ;
            int total = xcount * ycount;

            frmTileDownloader frm = new frmTileDownloader();
            frm.Show();


            int count = 0;
            for (int x = xx1; x < xx2; x += tileSizePx)
            {
                for (int y = yy1; y < yy2; y += tileSizePx)
                {
                    int picx = x / tileSizePx;
                    int picy = y / tileSizePx;
                    int picZoom = zoom;

                    if (picx >= 0 && picy >= 0 && picx < max && picy < max)
                    {
//                        drawOneLayerTile(x, y, (int)(tileSizePx), (int)(tileSizePx), oCC, picx, picy, picZoom, false);
                        drawOneLayerTile(picx, picy, picZoom, mapType);
                    }
                    count++;
                }
                //Debug.Print("progress for zoom {0}: {1}/{2}", zoom, count, total);
                string message = String.Format("progress for zoom {0}: {1}/{2}", zoom1, count, total);
                frm.Update(message, 100 * count  / total);
                if (frm.cancel) break;
            }

            frm.Close();
        }







        void drawOneLayerTile(int x, int y, int zoom, MapType mapType)
        {
            PureImage img = null;
            try
            {
                Exception result;
                img = GMaps.Instance.GetImageFrom(mapType, new GMap.NET.Point(x, y), zoom, out result);
            }
            catch (Exception)
            {

            }
        }
    }
}
