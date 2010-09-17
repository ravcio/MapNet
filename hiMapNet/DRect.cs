using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hiMapNet
{
    public class DRect
    {
        private double m_x1;

        public double X1
        {
            get { return m_x1; }
            set { m_x1 = value; }
        }
        private double m_y1;

        public double Y1
        {
            get { return m_y1; }
            set { m_y1 = value; }
        }
        private double m_x2;

        public double X2
        {
            get { return m_x2; }
            set { m_x2 = value; }
        }
        private double m_y2;

        public double Y2
        {
            get { return m_y2; }
            set { m_y2 = value; }
        }

        public DRect(double x1, double y1, double x2, double y2)
        {
            m_x1 = x1;
            m_y1 = y1;
            m_x2 = x2;
            m_y2 = y2;
        }

        public DRect()
        {
            m_x1 = double.MaxValue;
            m_y1 = double.MaxValue;
            m_x2 = double.MinValue;
            m_y2 = double.MinValue;
        }

        public DRect(System.Drawing.Rectangle rect)
        {
            m_x1 = rect.Left;
            m_y1 = rect.Bottom;
            m_x2 = rect.Right;
            m_y2 = rect.Top;
        }

        public double Width()
        {
            return Math.Abs(m_x2 - m_x1);
        }

        public double Height()
        {
            return Math.Abs(m_y2 - m_y1);
        }

        public void MergeRects(DRect rect)
        {
            double x_min = Math.Min(m_x1, rect.m_x1);
            double y_min = Math.Min(m_y1, rect.m_y1);
            double x_max = Math.Max(m_x2, rect.m_x2);
            double y_max = Math.Max(m_y2, rect.m_y2);

            m_x1 = x_min;
            m_x2 = x_max;
            m_y1 = y_min;
            m_y2 = y_max;
        }

        public void MergeRectWithPoint(DPoint point)
        {
            double x_min = Math.Min(m_x1, point.X);
            double y_min = Math.Min(m_y1, point.Y);
            double x_max = Math.Max(m_x2, point.X);
            double y_max = Math.Max(m_y2, point.Y);

            m_x1 = x_min;
            m_x2 = x_max;
            m_y1 = y_min;
            m_y2 = y_max;
        }


        /// <summary>
        /// Sprawdza czy istnieje częsc wspolna.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns>true=prostokąty kolidują, false=prostokąty są rozłączne</returns>
        public bool IntersectsWith(DRect rect)
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


        /// <summary>
        /// Checks if this contains rect
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool Contains(DRect rect)
        {
            if (m_x1 < rect.m_x1 && m_x2 > rect.m_x2)
            {
                if (m_y1 < rect.m_y1 && m_y2 > rect.m_y2)
                {
                    return true;
                }
            }
            return false;
        }



        internal bool Contains(double x, double y)
        {
            if (m_x1 <= x && x <= m_x2 &&
                m_y1 <= y && y <= m_y2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
