using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;

namespace hiMapNet
{
    public class BitmapFeature : Feature
    {
        private Bitmap m_oBitmap = null;
        private byte[] m_oBytes = null;
        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        int anchorx;

        public int Anchorx
        {
            get { return anchorx; }
            set { anchorx = value; }
        }
        int anchory;

        public int Anchory
        {
            get { return anchory; }
            set { anchory = value; }
        }


        public BitmapFeature(double x, double y, int anchorx, int anchory, Bitmap bitmap)
        {
            this.x = x;
            this.y = y;
            this.anchorx = anchorx;
            this.anchory = anchory;
            this.m_oBitmap = bitmap;
        }

        public BitmapFeature(double x, double y, int anchorx, int anchory, string filename)
        {
            this.x = x;
            this.y = y;
            this.anchorx = anchorx;
            this.anchory = anchory;



            // read stream into memory
            long size = new FileInfo(filename).Length;
            if (size > 0xffffffff) throw new Exception("File too big");
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(file);

            m_oBytes = new byte[size];
            reader.Read(m_oBytes, 0, (int)size);

            reader.Close();
            file.Close();

            // decompress the image
            if (filename.ToLower().StartsWith("http"))
            {
                m_oBitmap = DownloadImage(filename);
            }
            else
            {
                m_oBitmap = new Bitmap(new MemoryStream(m_oBytes));
            }
        }

        public BitmapFeature(BitmapFeature feature)
        {
            this.x = feature.x;
            this.y = feature.y;
            this.anchorx = feature.anchorx;
            this.anchory = feature.anchory;
            this.m_oBitmap = feature.m_oBitmap;
        }

        public Bitmap Bitmap
        {
            get { return m_oBitmap; }
        }

        private Bitmap DownloadImage(string sHttp)
        {
            //sHttp = "http://83.19.124.227/TileWMS/709/group1/z05x000008y000018.aspx";
            System.Net.WebClient Client = new WebClient();
            Stream strm = Client.OpenRead(sHttp);

            Bitmap oImg = null;
            if (strm.CanRead)
            {
                oImg = new Bitmap(strm);
            }
            return oImg;
        }

        public override void CalcMBR()
        {
            base.m_oMBR = new DRect(x, y, x, y);
        }

        public override Feature clone()
        {
            return new BitmapFeature(this);
        }

        public override int pixelMargin()
        {
            return 50;
        }
    }
}
