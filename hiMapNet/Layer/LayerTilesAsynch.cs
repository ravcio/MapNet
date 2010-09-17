using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Diagnostics;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.ComponentModel;
using System.Timers;
using System.Threading;

namespace hiMapNet
{
    public class LayerTilesAsynch : LayerAbstract
    {
        class TileProperty
        {
            public TileProperty()
            {
            }

            public TileProperty(int x, int y, int zoom, MapType mapType)
            {
                this.x = x;
                this.y = y;
                this.zoom = zoom;
                this.mapType = mapType;
            }

            // download properties
            public int x, y, zoom;
            public MapType mapType;

            // loaded image
            public Image image = null;

            // time stamp started loading
            public DateTime timeDownloadStarted = DateTime.MinValue;
            public DateTime timeDownloadEnded = DateTime.MinValue;
        }


        /// <summary>
        /// Asynchronous image loading and caching
        /// </summary>
        class TileImageLoaderAndCache
        {
            static TileImageLoaderAndCache instance = null;

            public static TileImageLoaderAndCache Instance
            {
                get
                {
                    if (instance == null) instance = new TileImageLoaderAndCache();
                    return instance;
                }
            }

            TileProperty[,] imageArray = null;
            System.Drawing.Rectangle currentScreenRect = new System.Drawing.Rectangle();
            MapType currentMapType = MapType.GoogleMap;

            int activeDownloads = 0;
            const int activeDownloadsLimit = 16;

            List<TileProperty> downloadsStack = new List<TileProperty>();
            const int downloadsStackSizeLimit = 256;

            List<TileProperty> memoryCache = new List<TileProperty>();
            const int memoryCacheSizeLimit = 1024;

            System.Timers.Timer timer = null;
            bool refreshPending = false;

            /// <summary>
            /// private constructor to prevent external creation of singleton
            /// </summary>
            TileImageLoaderAndCache()
            {
            }

            /// <summary>
            /// Set new active area or tile type for which tiles will be retrieved.
            /// </summary>
            /// <param name="screenRect"></param>
            /// <param name="mapType"></param>
            public void newArea(System.Drawing.Rectangle screenRect, MapType mapType)
            {
                if (currentScreenRect != screenRect || currentMapType != mapType)
                {
                    currentMapType = mapType;
                    currentScreenRect = screenRect;
                    int w = currentScreenRect.Width;
                    int h = currentScreenRect.Height;
                    imageArray = new TileProperty[w, h];
                }
            }

            /// <summary>
            /// Get image cache or schedule it for later download.
            /// </summary>
            /// <param name="x">tile x in tile-map coordinates</param>
            /// <param name="y"></param>
            /// <param name="zoom"></param>
            /// <param name="mapType"></param>
            /// <returns></returns>
            public Image getImage(int x, int y, int zoom, MapType mapType, bool tryDownloading)
            {
                int w = currentScreenRect.Width;
                int h = currentScreenRect.Height;

                int dx = x - currentScreenRect.X;
                int dy = y - currentScreenRect.Y;

                if (tryDownloading == false)
                {
                    //TileProperty tile = searchInMemoryCache(x, y, zoom, mapType);



                }



                if (dx >= 0 && dx < w && dy >= 0 && dy < h)
                {
                    // get from instant cache (L1)
                    TileProperty tile = imageArray[dx, dy];
                    if (tile == null)
                    {
                        // get from bigCache (L2)
                        tile = searchInMemoryCache(x, y, zoom, mapType);
                        if (tile == null)
                        {
                            tile = new TileProperty(x, y, zoom, mapType);
                        }
                        imageArray[dx, dy] = tile; // download pending (queued)
                    }
                    if (tile.image == null && tile.timeDownloadStarted == DateTime.MinValue)
                    {
                        if (activeDownloads >= activeDownloadsLimit)
                        {
                            // stack up this request
                            TileProperty tileFound = searchOnStackAndPop(x, y, zoom, mapType);
                            if (tileFound == null)
                            {
                                downloadsStack.Add(tile);
                                if (downloadsStack.Count > downloadsStackSizeLimit)
                                {
                                    // remove bottom element
                                    downloadsStack.RemoveAt(0);
                                }

                                Debug.Print("Stack size (+)=" + downloadsStack.Count.ToString());
                            }
                            else
                            {
                                downloadsStack.Add(tileFound); // move this tile to top (priritize it)
                            }
                        }
                        else
                        {
                            // initiate asynchronious image loading
                            scheduleForDownload(tile);
                        }
                    }
                    return tile.image;
                }
                else
                {
                    // this image is outside requested area
                    return null;
                }
            }

            void scheduleForDownload(TileProperty tile)
            {
                Debug.Assert(tile != null);

                // stats
                tile.timeDownloadStarted = DateTime.Now;
                activeDownloads++;
                Debug.Print("Active downloads (+)=" + activeDownloads.ToString());

                BackgroundWorker bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
                bgWorker.RunWorkerAsync(tile);
            }

            void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                activeDownloads--;
                Debug.Print("Active downloads (-)=" + activeDownloads.ToString());

                // loaded image already in the array
                TileProperty tile = (TileProperty)e.Result;
                tile.timeDownloadEnded = DateTime.Now;

                addToBigCache(tile);

                // trigger refresh
                RefreshMapWithDelay();

                // processes stacked downloads
                if (activeDownloads < activeDownloadsLimit && downloadsStack.Count > 0)
                {
                    TileProperty tileStacked = downloadsStack[downloadsStack.Count - 1];
                    downloadsStack.RemoveAt(downloadsStack.Count - 1);

                    Debug.Print("Stack size (-)=" + downloadsStack.Count.ToString());
                    scheduleForDownload(tileStacked);
                }
            }

            void bgWorker_DoWork(object sender, DoWorkEventArgs e)
            {
                Thread thread = Thread.CurrentThread;
                Console.WriteLine("CurrentThread Name: {0}", thread.Name);

                TileProperty arg = (TileProperty)e.Argument;

                PureImage img = null;
                try
                {
                    Exception result;
                    img = GMaps.Instance.GetImageFrom(arg.mapType, new GMap.NET.Point(arg.x, arg.y), arg.zoom, out result);
                }
                catch (Exception)
                {

                }
                WindowsFormsImage imgWin = (WindowsFormsImage)img;

                if (imgWin != null)
                {
                    arg.image = imgWin.Img;
                }
                e.Result = arg;
            }

            TileProperty searchInMemoryCache(int x, int y, int zoom, MapType mapType)
            {
                foreach (TileProperty tile in memoryCache)
                {
                    if (tile.x == x && tile.y == y && tile.zoom == zoom && tile.mapType == mapType)
                    {
                        return tile;
                    }
                }
                return null;
            }

            TileProperty searchOnStackAndPop(int x, int y, int zoom, MapType mapType)
            {
                foreach (TileProperty tile in downloadsStack)
                {
                    if (tile.x == x && tile.y == y && tile.zoom == zoom && tile.mapType == mapType)
                    {
                        return tile;
                    }
                }
                return null;
            }


            void addToBigCache(TileProperty tile)
            {
                TileProperty tileFound = searchInMemoryCache(tile.x, tile.y, tile.zoom, tile.mapType);

                if (tileFound == null)
                {
                    memoryCache.Add(tile);

                    while (memoryCache.Count > memoryCacheSizeLimit)
                    {
                        memoryCache.RemoveAt(0);
                    }
                }
                else
                {
                    tileFound.x = tile.x;
                    tileFound.y = tile.y;
                    tileFound.zoom = tile.zoom;
                    tileFound.mapType = tile.mapType;
                    tileFound.image = tile.image;
                    tileFound.timeDownloadStarted = tile.timeDownloadStarted;
                    tileFound.timeDownloadEnded = tile.timeDownloadEnded;
                }
            }


            void RefreshMapWithDelay()
            {
                if (refreshPending) return;

                // trigger refresh in 200ms
                timer = new System.Timers.Timer(200);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.AutoReset = false;
                refreshPending = true;
                timer.Start();
            }

            void timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                MapControl mapControl = MapControl.Globals.Instance.MapControl;
                mapControl.InvalidateMap();
                refreshPending = false;
            }
        }


        private MapType mapType = MapType.GoogleMap;

        public MapType MapType
        {
            get { return mapType; }
            set 
            { 
                mapType = value;
                Invalidate();
            }
        }

        private const double zoomElevateUpscale = 1024 * 8; //1024;

        public LayerTilesAsynch()
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
        override public void Draw(Graphics g, System.Drawing.Rectangle Rect, CoordConverter oCC)
        {
            base.Draw(g, Rect, oCC);

            // produce a list of needed tiles

            // ustal dla jakiego zoom wyswietlac bedziemy kafelki

            if (oCC.atMaster.IsRotating()) return; //can not display
            double scale = oCC.atMaster.A;
            //System.Diagnostics.Debug.Print("Scale={0}", scale);
            // scale = 1,2,4,8

            int zoom = (int)Math.Log(scale * zoomElevateUpscale, 2);

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


            // okresl liste kafelkow do wyswietlenia
            int xx1 = (x1 / tileSizePx) * tileSizePx;
            int yy1 = (y1 / tileSizePx) * tileSizePx;

            int xx2 = (x2 / tileSizePx) * tileSizePx + tileSizePx;
            int yy2 = (y2 / tileSizePx) * tileSizePx + tileSizePx;


            // cache
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                xx1 / tileSizePx, yy1 / tileSizePx,
                (xx2 - xx1) / tileSizePx, (yy2 - yy1) / tileSizePx);
            TileImageLoaderAndCache.Instance.newArea(rect, mapType);

//            drawLayerTiles(rect, zoom, g, oCC, true);
             
            int max = (int)(scale * zoomElevateUpscale);

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
                        drawOneLayerTile(x, y, (int)(tileSizePx), (int)(tileSizePx), g, oCC, picx, picy, picZoom, false);
                    }
                    //DrawEllipse(x, y, tileSizePx, tileSizePx, g, oCC1);
                    count++;
                }
            }
        }

        void drawLayerTiles(System.Drawing.Rectangle area, int zoom, Graphics g, CoordConverter oCC, bool tryDownloading)
        {
            int tileSizePx = (1 << (8 - zoom)) * (int)zoomElevateUpscale;
            int maxIdx = 1 << zoom;

            int count = 0;
            for (int x = area.X; x < area.X + area.Width; x++)
            {
                for (int y = area.Y; y < area.Y + area.Height; y++)
                {
                    if (count < 66 && x >= 0 && y >= 0 && x < maxIdx && y < maxIdx)
                    {
                        drawOneLayerTile(x * tileSizePx, y * tileSizePx, tileSizePx, tileSizePx, g, oCC, x, y, zoom, tryDownloading);
                    }
                    //DrawEllipse(x, y, tileSizePx, tileSizePx, g, oCC);
                    count++;
                }
            }
        }

        private void drawOneLayerTile(int x1, int y1, int w, int h, Graphics g, CoordConverter oCC, int picx, int picy, int picZoom, bool tryDownloading)
        {
            Image imageToScreen = null;

            // get image from cache or schedule its download
            imageToScreen = TileImageLoaderAndCache.Instance.getImage(picx, picy, picZoom, mapType, tryDownloading);

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

            if (imageToScreen != null)
            {
                Debug.Assert(xScrWidth == 256);
                Debug.Assert(yScrWidth == 256);

                g.DrawImage(imageToScreen, xScr, yScr, xScrWidth, yScrWidth);
            }
            else
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("hiMapNet.Resources.h0.jpeg");
                if (myStream != null)
                {
                    Bitmap bmp = new Bitmap(myStream);
                    g.DrawImage(bmp, xScr, yScr, xScrWidth, yScrWidth);
                    bmp.Dispose();
                }
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

        private void RequestRefresh()
        {
            System.Windows.Forms.UserControl userControl = MapControl.Globals.Instance.MapControl;
            userControl.Invalidate();
        }
    }
}
