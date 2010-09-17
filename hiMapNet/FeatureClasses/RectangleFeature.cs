using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class RectangleFeature : Feature
    {
        public double x, y;
        public double w, h;

        public RectangleFeature(double x, double y, double w, double h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public override void CalcMBR()
        {
            base.m_oMBR = new DRect(x, y, x + w, y + h);
        }

        public override Feature clone()
        {
            throw new NotImplementedException();
        }

    }
}
