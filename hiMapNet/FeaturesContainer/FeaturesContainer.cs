using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace hiMapNet
{
    /// <summary>
    /// This class is a container for features.
    /// This class is not involved in any coordinate convertions.
    /// Coordinates are stored as double values.
    /// It is planned to store coordinates as int32 values.
    /// Objcets stored in this class are refered by referencing ID.
    /// Object returned can not be modified and affect this container.
    /// </summary>
    public class FeaturesContainer
    {
        LayerAbstract parentLayer = null;

        public LayerAbstract ParentLayer
        {
            get { return parentLayer; }
            set { parentLayer = value; }
        }

        public FeaturesContainer(LayerAbstract parentLayer)
        {
            this.parentLayer = parentLayer;
        }

        public enum yesNoAuto
        {
            yes,
            no,
            auto
        };

        List<Feature> m_oFeatures = new List<Feature>();
        bool boundsOK = false;

        RenderGDIplus m_oRenderGDIplus = new RenderGDIplus();


        #region Feature management



        public int Count
        {
            get { return m_oFeatures.Count; }
        }

        public void addFeature(Feature feature)
        {
            if (feature == null) return;

            feature.FeatureID = m_oFeatures.Count;
            feature.setParentContainer(this);

            m_oFeatures.Add(feature);
            boundsOK = false;
        }

        public void removeFeature(int index)
        {
            if (m_oFeatures[index] == null) return;
            m_oFeatures[index].setParentContainer(null);
            m_oFeatures[index] = null;
            boundsOK = false;
        }

        public void clear()
        {
            for (int i = m_oFeatures.Count - 1; i >= 0; i--)
            {
                removeFeature(i);
            }
            m_oFeatures.Clear();
        }

        #endregion









        public void boundsDirty()
        {
            boundsOK = false;
        }


        /// <summary>
        /// Rednders contained features
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Rect"></param>
        /// <param name="oCC"></param>
        public void draw(Graphics g, Rectangle Rect, CoordConverter oCC)
        {
            if (boundsOK == false)
            {
                calculateBounds();
            }

            // calculate Bound in Layer Coordsys
            double xmin, xmax, ymin, ymax;  // bounds of current viewport (in map coordsys)

            // TODO: respoct rotation of coordsys
            oCC.ConvertInverse(Rect.Left, Rect.Top);
            xmin = oCC.X;
            ymin = oCC.Y;
            oCC.ConvertInverse(Rect.Right, Rect.Bottom);
            xmax = oCC.X;
            ymax = oCC.Y;

            if (ymin > ymax)
            {
                double t = ymin;
                ymin = ymax;
                ymax = t;
            }

            DRect rectScreenInLayer = new DRect(xmin, ymin, xmax, ymax);

            // refresh value of Feature.MBR
            for (int i = 0; i < m_oFeatures.Count; i++)
            {
                Feature feature = m_oFeatures[i];
                if (feature != null)
                {
                    int pixelMargin = feature.pixelMargin();

                    if (feature.MBR.X2 >= xmin - pixelMargin &&
                        feature.MBR.X1 <= xmax + pixelMargin &&
                        feature.MBR.Y2 >= ymin - pixelMargin &&
                        feature.MBR.Y1 <= ymax + pixelMargin)
                    {
                        drawMapFeature(g, Rect, rectScreenInLayer, oCC, feature, false);
                    }
                }
            }
        }

        private void drawMapFeature(Graphics g, Rectangle Rect, DRect rectScreenInLayer, CoordConverter oCC, Feature feature, bool selectionStyle)
        {
            Debug.Assert(feature != null);

            if (feature is PolylineFeature)
            {
                PolylineFeature oP = (PolylineFeature)feature;

                // draw polyline points
                for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
                {
                    List<DPoint> Points_list = oP.m_oParts[iPart].Points;
                    if (Points_list.Count > 0)
                    {
                        DPoint[] Points_src = oP.m_oParts[iPart].Points.ToArray();
                        int ipp_count = Points_src.GetLength(0);
                        Point[] Points_array = new Point[ipp_count];
                        Rectangle rect = new Rectangle(0, 0, 6, 6);

                        int? currPointX = null;
                        int? currPointY = null;
                        int? prevPointX = null;
                        int? prevPointY = null;

                        for (int ipp = 0; ipp < ipp_count; ipp++)
                        {

                            // only points withing screen area
                            if (rectScreenInLayer.Contains((double)Points_src[ipp].X, (double)Points_src[ipp].Y))
                            {
                                oCC.Convert(Points_src[ipp].X, Points_src[ipp].Y);
                                currPointX = (int)oCC.X;
                                currPointY = (int)oCC.Y;

                                rect.X = (int)oCC.X - 3;
                                rect.Y = (int)oCC.Y - 3;
                                rect.Width = 6;
                                rect.Height = 6;
                                if (Points_src[ipp].Selected)
                                {
                                    m_oRenderGDIplus.FillRectangle(g, rect, 0xffff0000); // red
                                }
                                else
                                {
                                    m_oRenderGDIplus.DrawRectangle(g, rect, 0xff0000ff);  // blue
                                }
                            }
                            else
                            {
                                currPointX = null;
                                currPointY = null;
                            }

                            // "add point marker"
                            if (ipp > 0)
                            {
                                if (currPointX != null || prevPointX != null)
                                { // draw if at least one point is visible

                                    if (prevPointX == null)
                                    {
                                        // failed to calculate earlier (not visible), do it now
                                        oCC.Convert(Points_src[ipp - 1].X, Points_src[ipp - 1].Y);
                                        prevPointX = (int)oCC.X;
                                        prevPointY = (int)oCC.Y;
                                    }
                                    if (currPointX == null)
                                    {
                                        // failed to calculate earlier (not visible), do it now
                                        oCC.Convert(Points_src[ipp].X, Points_src[ipp].Y);
                                        currPointX = (int)oCC.X;
                                        currPointY = (int)oCC.Y;
                                    }

                                    int dx = (int)(currPointX - prevPointX);
                                    int dy = (int)(currPointY - prevPointY);
                                    int len = (int)Math.Sqrt(dx * dx + dy * dy);

                                    // draw when line segment longer than 20 pixels
                                    if (len > 20)
                                    {
                                        int pointX = (int)(prevPointX + currPointX) / 2;
                                        int pointY = (int)(prevPointY + currPointY) / 2;

                                        //m_oRenderGDIplus.DrawLine(g, pointX - 3, pointY - 3, pointX + 3, pointY + 3, 2, Color.Blue, 1);
                                        //m_oRenderGDIplus.DrawLine(g, pointX - 3, pointY + 3, pointX + 3, pointY - 3, 2, Color.Blue, 1);
                                    }
                                }
                            }
                            prevPointX = currPointX;
                            prevPointY = currPointY;
                        }
                    }
                }

                // draw actual polyline
                for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
                {
                    List<DPoint> Points_list = oP.m_oParts[iPart].Points;
                    if (Points_list.Count > 0)
                    {
                        DPoint[] Points_src = oP.m_oParts[iPart].Points.ToArray();

                        int ipp_count = Points_src.GetLength(0);
                        Point[] Points_array = new Point[ipp_count];
                        for (int ipp = 0; ipp < ipp_count; ipp++)
                        {
                            oCC.Convert((double)Points_src[ipp].X, (double)Points_src[ipp].Y);
                            Points_array[ipp] = new Point((int)oCC.X, (int)oCC.Y);
                        }
                        if (oP.Selected)
                        {
                            m_oRenderGDIplus.DrawPolylineFeature(g, Points_array, oP.Style.LinePattern, Color.Red, oP.Style.LineWidth);
                        }
                        else
                        {
                            m_oRenderGDIplus.DrawPolylineFeature(g, Points_array, oP.Style.LinePattern, oP.Style.LineColor, oP.Style.LineWidth);
                        }
                    }
                }
            }
            else if (feature is SymbolFeature)
            {
                SymbolFeature oP = (SymbolFeature)feature;

                oCC.Convert(oP.x, oP.y);
                Rectangle rect = new Rectangle((int)oCC.X - 2, (int)oCC.Y - 2, 4, 4);
                if ((oP.color & 0xff000000) != 0x00)
                {
                    if (feature.Selected)
                    {
                        m_oRenderGDIplus.DrawRectangle(g, rect, (uint)Color.Red.ToArgb());
                    }
                    else
                    {
                        m_oRenderGDIplus.DrawRectangle(g, rect, oP.color);
                    }
                }
            }
            else if (feature is RectangleFeature)
            {
                RectangleFeature oP = (RectangleFeature)feature;

                oCC.Convert(oP.x, oP.y);
                Rectangle rect = new Rectangle((int)oCC.X - 2, (int)oCC.Y - 2, 4, 4);
                m_oRenderGDIplus.DrawRectangle(g, rect, (uint)0xff000000);
            }
            else if (feature is BitmapFeature)
            {
                BitmapFeature oB = feature as BitmapFeature;

                oCC.Convert(oB.X, oB.Y);
                int x = (int)oCC.X;
                int y = (int)oCC.Y;

                g.DrawImageUnscaled(oB.Bitmap, new Point(x - oB.Anchorx, y - oB.Anchory));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Recalculate bound of all features
        /// </summary>
        private void calculateBounds()
        {
            // refresh value of Feature.MBR
            for (int i = 0; i < m_oFeatures.Count; i++)
            {
                Feature feature = m_oFeatures[i];
                if (feature != null) feature.CalcMBR();
            }
            boundsOK = true;
        }

        internal Feature getFeature(int idx)
        {
            return m_oFeatures[idx];
        }

        internal void insertFeature(int idx, Feature feature)
        {
            Debug.Assert(feature != null);
            Debug.Assert(feature.ParentContainer == null);
            Debug.Assert(m_oFeatures[idx] == null); // make sure position is empty

            m_oFeatures[idx] = feature;
            feature.FeatureID = idx;
            feature.setParentContainer(this);

            boundsOK = false;
        }

        private yesNoAuto displayPoints;
        public yesNoAuto DisplayPoints
        {
            get { return displayPoints; }
            set { displayPoints = value; }
        }

        private yesNoAuto displayPointAddHandles;
        public yesNoAuto DisplayPointAddHandles
        {
            get { return displayPointAddHandles; }
            set { displayPointAddHandles = value; }
        }

        /// <summary>
        /// Search for features that intersect or are contained in a given rectangle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<Feature> searchAtPoint(DRect rect)
        {
            List<Feature> result = new List<Feature>();
            for (int i = 0; i < m_oFeatures.Count; i++)
            {
                Feature feature = m_oFeatures[i];
                if (feature.MBR.IntersectsWith(rect))
                {
                    // detailed collision test
                    if (feature is SymbolFeature) result.Add(feature);
                    if (feature is PolylineFeature)
                    {
                        //castToPolyline

                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Casts a point onto polyline and returns location of its projection.
        /// </summary>
        /// <param name="feature">feature of polyline type</param>
        /// <param name="part">part number to analyse</param>
        /// <param name="x">point being cast x coord</param>
        /// <param name="y">point being cast y coord</param>
        /// <param name="a">normalized cast location along the polyline part [0..1]</param>
        /// <param name="x_cast">cast x location on the polyline</param>
        /// <param name="y_cast">cast y location on the polyline</param>
        /// <param name="dDist">distance of point and polyline</param>
        /// <param name="dPolyLen">calculated polyline length</param>
        /// <param name="segNo">number of polyline segment that was closest (0-based)</param>
        /// <param name="a_seg">normalized position within polyline segment (note that a_seg might be outside of 0..1 range). Value smaller than 0 or greater than 1 denotes, that point is cast to end point of segment.</param>
        /// <param name="b_seg">distance from a line defined by polyline segment. Side is indicated by sign: positive for right side, negative for left side.</param>
        public void castToPolyline(PolylineFeature feature, int part, double x, double y,
            out double a, out double x_cast, out double y_cast,
            out double dDist, out double dPolyLen,
            out int segNo, out double a_seg, out double b_seg)
        {
            if (feature == null) throw new Exception("Invalid parameter: feature can not be null.");

            if (part < 0 || part > feature.m_oParts.Count) throw new Exception("Invalid parameter: part.");

            Part polylinePart = feature.m_oParts[part];

            if (polylinePart.Points.Count == 0)
            {
                dDist = double.MaxValue;
                a = 0;
                x_cast = double.MaxValue;
                y_cast = double.MaxValue;
                dPolyLen = 0;
                segNo = -1;
                a_seg = 1;
                b_seg = 1;
                return;
            }

            double a_tmp, b_tmp;
            double dSegLen;
            double dDistMin = -1;
            double dDistTmp = -1;
            dDist = 0;
            dPolyLen = 0;
            a = 0;
            a_seg = 0;
            b_seg = 0;
            segNo = 0;
            x_cast = 0;
            y_cast = 0;
            for (int j = 1; j < polylinePart.Points.Count; j++)
            {
                double ax, ay, bx, by;
                ax = polylinePart.Points[j - 1].X;
                ay = polylinePart.Points[j - 1].Y;
                bx = polylinePart.Points[j].X;
                by = polylinePart.Points[j].Y;

                dSegLen = Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by));

                if (dSegLen > 0)
                {
                    // a=0..1  -> rzut punktu x jest miedzy poczatkiem i koncem odcinka ab
                    a_tmp = ((x - ax) * (bx - ax) + (y - ay) * (by - ay)) / ((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
                    b_tmp = ((bx - ax) * (ay - y) - (by - ay) * (ax - x)) / dSegLen;
                    if (a_tmp > 0 && a_tmp < 1)
                    {  // policz odleglosc w rzucie do odcinka
                        dDistTmp = Math.Abs(b_tmp);
                    }
                    else if (a_tmp >= 1)
                    {
                        dDistTmp = Math.Sqrt((bx - x) * (bx - x) + (by - y) * (by - y));
                    }
                    else
                    {
                        dDistTmp = Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y));
                    }
                }
                else
                {
                    // ignore this segment
                    a_tmp = 0;
                    b_tmp = 0;
                    dDistTmp = Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y));
                }

                if (dDistTmp < dDistMin || dDistMin == -1)
                {
                    a_seg = a_tmp;
                    b_seg = b_tmp;
                    segNo = j - 1;
                    dDist = dDistTmp;
                    dDistMin = dDistTmp;

                    double dA_tmp;
                    dA_tmp = a_seg;
                    if (dA_tmp > 1) dA_tmp = 1;
                    if (dA_tmp < 0) dA_tmp = 0;

                    a = dPolyLen + dA_tmp * dSegLen;
                }
                dPolyLen += dSegLen;
            }

            if (dPolyLen == 0) throw new Exception("Polyline length can not be zero.");
            a = a / dPolyLen;
            {
                double ax, ay, bx, by;
                ax = polylinePart.Points[segNo].X;
                ay = polylinePart.Points[segNo].Y;
                bx = polylinePart.Points[segNo + 1].X;
                by = polylinePart.Points[segNo + 1].Y;
                if (a_seg >= 0 && a_seg <= 1)
                {
                    x_cast = ax + (bx - ax) * a_seg;
                    y_cast = ay + (by - ay) * a_seg;
                }
                else if (a_seg < 0)
                {
                    x_cast = ax;
                    y_cast = ay;
                }
                else if (a_seg > 1)
                {
                    x_cast = bx;
                    y_cast = by;
                }
            }
        }
    }
}
