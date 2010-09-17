using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class PolylineFeature : Feature
    {
        public List<Part> m_oParts;

        /// <summary>
        /// creates one part polyline
        /// </summary>
        /// <param name="oPoints"></param>
        /// <param name="oStyle"></param>
        public PolylineFeature(List<DPoint> oPoints, Style oStyle)
        {
            m_oParts = new List<Part>();
            Part oPart = new Part();
            m_oParts.Add(oPart);
            m_oStyle = oStyle;

            oPart.Points.AddRange(oPoints);
        }

        public PolylineFeature(PolylineFeature feature)
        {
            m_oStyle = feature.m_oStyle;
            m_oParts = new List<Part>();

            foreach (Part part in feature.m_oParts)
            {
                Part newPart = new Part();
                m_oParts.Add(newPart);
                foreach (DPoint point in part.Points)
                {
                    newPart.Points.Add(new DPoint(point));
                }
            }
        }

        public override void CalcMBR()
        {
            DRect oMBR = new DRect();

            PolylineFeature oP = this;
            // draw polyline
            for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
            {
                List<DPoint> Points_list = oP.m_oParts[iPart].Points;
                for (int iPnt = 0; iPnt < Points_list.Count; iPnt++)
                {
                    DPoint oPnt = Points_list[iPnt];
                    oMBR.MergeRectWithPoint(new DPoint(oPnt.X, oPnt.Y));
                }
            }

            base.m_oMBR = oMBR;
        }

        public override Feature clone()
        {
            return new PolylineFeature(this);
        }
    }
}
