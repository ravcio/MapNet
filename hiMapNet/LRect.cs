using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    public class LRect
    {
        private long m_x1;

        public long X1
        {
            get { return m_x1; }
            set { m_x1 = value; }
        }
        private long m_y1;

        public long Y1
        {
            get { return m_y1; }
            set { m_y1 = value; }
        }
        private long m_x2;

        public long X2
        {
            get { return m_x2; }
            set { m_x2 = value; }
        }
        private long m_y2;

        public long Y2
        {
            get { return m_y2; }
            set { m_y2 = value; }
        }

        public LRect(long x1, long y1, long x2, long y2)
        {
            m_x1 = x1;
            m_y1 = y1;
            m_x2 = x2;
            m_y2 = y2;
        }

        public LRect()
        {
            m_x1 = long.MaxValue;
            m_y1 = long.MaxValue;
            m_x2 = long.MinValue;
            m_y2 = long.MinValue;
        }

        public LRect(System.Drawing.Rectangle rect)
        {
            m_x1 = rect.Left;
            m_y1 = rect.Bottom;
            m_x2 = rect.Right;
            m_y2 = rect.Top;
        }

        public long Width()
        {
            return Math.Abs(m_x2 - m_x1);
        }

        public long Height()
        {
            return Math.Abs(m_y2 - m_y1);
        }

        public void MergeRects(LRect rect)
        {
            long x_min = Math.Min(m_x1, rect.m_x1);
            long y_min = Math.Min(m_y1, rect.m_y1);
            long x_max = Math.Max(m_x2, rect.m_x2);
            long y_max = Math.Max(m_y2, rect.m_y2);

            m_x1 = x_min;
            m_x2 = x_max;
            m_y1 = y_min;
            m_y2 = y_max;
        }

        public void MergeRectWithPoint(LPoint point)
        {
            long x_min = Math.Min(m_x1, point.X);
            long y_min = Math.Min(m_y1, point.Y);
            long x_max = Math.Max(m_x2, point.X);
            long y_max = Math.Max(m_y2, point.Y);

            m_x1 = x_min;
            m_x2 = x_max;
            m_y1 = y_min;
            m_y2 = y_max;
        }


        /// <summary>
        /// Sprawdza czy istnieje czêsc wspolna.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns>true=prostok¹ty koliduj¹, false=prostok¹ty s¹ roz³¹czne</returns>
        public bool IntersectsWith(LRect rect)
        {
            if (m_x2 > rect.m_x1 && m_x1 < rect.m_x2)
            {
                if (m_y2 > rect.m_y1 && m_y1 < rect.m_y2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
