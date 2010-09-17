using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace hiMapNet
{
    /// <summary>
    /// Klasa renderuj¹ca obiekty graficzne poprzez GDI
    /// </summary>
    class RenderGDI
    {
        private IntPtr m_hdc;
        private Graphics m_oG;

        public RenderGDI()
        {
            m_hdc = IntPtr.Zero;
        }

        internal void DrawPolylineFeature(Graphics g, System.Drawing.Point[] Points_array,
            int LinePattern, System.Drawing.Color LineColor, int LineWidth)
        {
            IntPtr drawPen;		// gdi pen
            IntPtr drawPen_old;		// gdi pen

            if (LinePattern == 2)
            {
                drawPen = (IntPtr)Win32.GDI.CreatePen(Win32.GDI.PS_GEOMETRIC, LineWidth, LineColor.ToArgb());
                drawPen_old = (IntPtr)Win32.GDI.SelectObject(m_hdc, drawPen);

                int count = Points_array.GetLength(0);
                Win32.GDI.Polyline(m_hdc, ref Points_array[0], count);

                Win32.GDI.SelectObject(m_hdc, drawPen_old);
                Win32.GDI.DeleteObject(drawPen);
            }

        }

        private int makeColor(int red, int green, int blue)
        {
            return blue * 65536 + green * 256 + red;
        }
        private int makeColor(Color c)
        {
            return c.B * 65536 + c.G * 256 + c.R;
        }


        internal void BeginRender(Graphics g)
        {
            if (m_hdc != IntPtr.Zero) throw new Exception("EndRender first.");

            m_oG = g;
            m_hdc = m_oG.GetHdc();
        }

        internal void EndRender()
        {
            m_oG.ReleaseHdc();
            m_hdc = IntPtr.Zero;
        }
    }
}
