using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    /// <summary>
    /// Represents point with long precision coordinates.
    /// </summary>
    public class LPoint
    {
        long m_x;

        public long X
        {
            get { return m_x; }
            set { m_x = value; }
        }
        long m_y;

        public long Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public LPoint()
        {
            m_x = long.MinValue;
            m_y = long.MinValue;
        }

        public LPoint(long x, long y)
        {
            m_x = x;
            m_y = y;
        }

        public LPoint(System.Drawing.Point point )
        {
            m_x = point.X;
            m_y = point.Y;
        }


        public bool Equals(LPoint obj)
        {
            if (X == obj.X && Y == obj.Y) return true;
            return false;
        }

    }
}
