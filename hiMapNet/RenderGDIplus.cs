using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    /// <summary>
    /// klasa renderuj¹ca z u¿yciem biblioteki GDI+
    ///
    /// </summary>
    class RenderGDIplus
    {
        int[] dashArray = new int[10];
        int dashArrayLen;

        public void DrawPolylineFeature(Graphics g, System.Drawing.Point[] Points_array,
            int LinePattern, System.Drawing.Color LineColor, int LineWidth)
        {
            if (LinePattern == 2)
            {
                if (Points_array.GetLength(0) > 1)
                {
                    g.DrawLines(new Pen(LineColor), Points_array);
                }
            }
            else
            {
                DrawDashedPolyline(g, Points_array, LinePattern, LineColor, LineWidth, 0);
            }
        }

        public void DrawLine(Graphics g, int x1, int y1, int x2, int y2, int LinePattern, System.Drawing.Color LineColor, int LineWidth)
        {
            g.DrawLine(new Pen(LineColor), new Point(x1, y1), new Point(x2, y2));
        }

        public void DrawDashedPolyline(Graphics g, Point[] oPoints,
            int LinePattern, Color LineColor, int LineWidth, int iBegPos)
        {
            if (LinePattern == 3)
            {
                // drobno przerywana
                dashArrayLen = 2;
                dashArray[0] = LineWidth;
                dashArray[1] = LineWidth * 2;

                int iTab = 0; // indeks w tablicy
                double dPos = iBegPos; // pozycja w indeksie

                if (dPos >= dashArray[0]) { dPos -= dashArray[0]; iTab++; }
                if (dPos >= dashArray[1]) { dPos -= dashArray[1]; iTab++; }

                if (oPoints.Length <= 1) return;  // <0 to moze blad?
                for (int i = 1; i < oPoints.Length; i++)
                {
                    //if (prvCollisionPossible(rcBounds, pPoints[i - 1].x, pPoints[i - 1].y, pPoints[i].x, pPoints[i].y))
                    {
                        prvStyledDashedLine(g, LinePattern, LineColor, LineWidth, oPoints[i - 1].X, oPoints[i - 1].Y,
                            oPoints[i].X, oPoints[i].Y, out iTab, out dPos);
                    }
                }
            }
        }

        private void prvStyledDashedLine(Graphics g, int LinePattern, Color LineColor, int LineWidth, int x1, int y1, int x2, int y2,
            out int iTab, out double dPos)
        {
            IntPtr hdc = g.GetHdc();
            Win32.POINT oP = new Win32.POINT();

            double dPatternPosition = 0;

            double dxx = (x2 - x1);
            double dyy = (y2 - y1);
            double dLen = Math.Sqrt(dxx * dxx + dyy * dyy);

            iTab = 0;
            dPos = 0;
            if (dLen == 0) return;

            double dX = (x2 - x1) / dLen;
            double dY = (y2 - y1) / dLen;

            double x = x1;
            double y = y1;
            int ix1 = 0, ix2 = 0, iy1 = 0, iy2 = 0;
            do
            {
                if ((iTab & 1) == 0)
                { // draw forecolor
                    ix1 = (int)(x + dX * dPatternPosition);
                    iy1 = (int)(y + dY * dPatternPosition);

                    dPatternPosition += dashArray[iTab] - dPos;
                    if (dPatternPosition >= dLen)
                    {
                        dPos = dashArray[iTab] - (dPatternPosition - dLen);
                        dPatternPosition = dLen;
                    }
                    else
                    {
                        iTab++;
                        if (iTab >= dashArrayLen) iTab = 0;
                        dPos = 0;
                    }
                    ix2 = (int)(x + dX * dPatternPosition);
                    iy2 = (int)(y + dY * dPatternPosition);

                    Win32.GDI.MoveToEx(hdc, ix1, iy1, ref oP);
                    Win32.GDI.LineTo(hdc, ix2, iy2);

                    //g.DrawLine(new Pen(LineColor), ix1, iy1, ix2, iy2);
                }
                else
                { // draw background
                    dPatternPosition += dashArray[iTab] - dPos;
                    if (dPatternPosition >= dLen)
                    {
                        dPos = dashArray[iTab] - (dPatternPosition - dLen);
                        dPatternPosition = dLen;
                    }
                    else
                    {
                        iTab++;
                        if (iTab >= dashArrayLen) iTab = 0;
                        dPos = 0;
                    }
                }
            } while (dPatternPosition < dLen);

            g.ReleaseHdc();

            return;  // correct this
        }



        internal void DrawPolygonFeature(Graphics g, System.Drawing.Point[] Points_array,
            int LinePattern, Color LineColor, int LineWidth,
            int FillPattern, System.Drawing.Color RegionColor, System.Drawing.Color RegionBackColor)
        {
            // rysuj wnêtrze
            if (FillPattern == 2) // no fill 
            {
                Brush oB = new SolidBrush(RegionColor);
                g.FillPolygon(oB, Points_array);
            }

            // rysuj ramke
            if (LinePattern == 2)
            {
                Pen oPen = new Pen(LineColor);
                g.DrawLines(oPen, Points_array);
            }
        }

        public void DrawRectangle(Graphics g, Rectangle rect, uint argb)
        {
            Pen p = new Pen(Color.FromArgb((int)argb));
            g.DrawRectangle(p, rect);
        }

        public void FillRectangle(Graphics g, Rectangle rect, uint argb)
        {
            //Pen p = new Pen(Color.FromArgb((int)argb));
            Brush b = Brushes.Red;

            g.FillRectangle(b, rect);
        }
    }
}
