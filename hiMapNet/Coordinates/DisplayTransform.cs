using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class DisplayTransform
    {
        AffineTransform at = null;
        internal DisplayTransform(AffineTransform affineTransform)
        {
            at = affineTransform;
        }
        public void FromDisplay(Point pntSrc, out DPoint pntDest)
        {
            double x = pntSrc.X;
            double y = pntSrc.Y;
            double xOut, yOut;
            at.ConvertInverse(x, y, out xOut, out yOut);
            pntDest = new DPoint(xOut, yOut);
        }

        public void ToDisplay(DPoint pntSrc, out Point pntDest)
        {
            double x = pntSrc.X;
            double y = pntSrc.Y;
            double xOut, yOut;
            at.Convert(x, y, out xOut, out yOut);
            pntDest = new Point((int)xOut, (int)yOut);
        }
    }
}
