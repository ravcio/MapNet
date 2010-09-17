using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    public class DPoint : ICloneable
    {
        double m_x;

        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }
        double m_y;

        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public DPoint()
        {
            m_x = double.MinValue;
            m_y = double.MinValue;
        }

        public DPoint(double x, double y)
        {
            m_x = x;
            m_y = y;
        }

        public DPoint(System.Drawing.Point point)
        {
            m_x = point.X;
            m_y = point.Y;
        }

        public DPoint(DPoint point)
        {
            m_x = point.m_x;
            m_y = point.m_y;
            selected = point.selected;
        }

        public bool Equals(DPoint obj)
        {
            if (X == obj.X && Y == obj.Y) return true;
            return false;
        }

        bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        object tag = null;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
