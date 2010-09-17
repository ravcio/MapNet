using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class PolygonFeature : Feature
    {
        public List<Part> m_oParts;

        internal PolygonFeature(List<DPoint> oPoints, Style oStyle)
        {
            m_oParts = new List<Part>();

            Part oPart = new Part();
            m_oParts.Add(oPart);
            m_oStyle = oStyle;

            oPart.Points.AddRange(oPoints);
        }

        // TODO: przerobiæ Part na LPoint
        public override void CalcMBR()
        {
            DRect oMBR = new DRect();
            /*
            PolygonFeature oP = this;
            // draw polyline
            for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
            {
                List<Point> Points_list = oP.m_oParts[iPart].Points;
                for (int iPnt = 0; iPnt < Points_list.Count; iPnt++)
                {
                    Point oPnt = Points_list[iPnt];
                    oMBR.MergeRectWithPoint(new LPoint(oPnt.X, oPnt.Y));
                }
            }*/

            base.m_oMBR = oMBR;
        }

        public override Feature clone()
        {
            throw new NotImplementedException();
        }

    }
}
