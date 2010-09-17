using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
//using GeometryLibrary.Geometry2D.Integer;

namespace hiMapNet
{
    public class LayerVectors : LayerAbstract
    {
        FeaturesContainer featuresContainer = null;

        private bool m_bVisible = true;

        private bool m_bBoundsOK = false;
        private LRect m_oMBR = null;

        public LayerVectors()
        {
            featuresContainer = new FeaturesContainer(this);
            layerCoordSys = new CoordSys(CoordSysType.LatLong, new Datum(DatumID.WGS84), new AffineTransform());
        }


        public LRect MBR
        {
            get
            {
                if (m_bBoundsOK == false)
                {
                    CalcMBR();
                }

                return m_oMBR;
            }
        }

        RenderGDI m_oRenderGDI = new RenderGDI();
        RenderGDIplus m_oRenderGDIplus = new RenderGDIplus();

        private void CalcMBR()
        {
            if (m_bBoundsOK == true) return;

            // policz bounds
            int count = featuresContainer.Count;

            DRect oLayerMBR = new DRect();
            for (int i = 0; i < count; i++)
            {
                Feature oF = featuresContainer.getFeature(i);
                oF.CalcMBR();
                oLayerMBR.MergeRects(oF.MBR);
            }

            m_bBoundsOK = true;
        }


        // 2008-08-15 by Rav
        // 
        public override void Draw(Graphics g, Rectangle Rect, CoordConverter oCC)
        {
            base.Draw(g, Rect, oCC);

            featuresContainer.draw(g, Rect, oCC);
        }


        //    /*  if (m_bBoundsOK == false)
        //      {
        //          CalcMBR();
        //      }*/

        //    // przelicz viewrect na wsp. layera
        //    //            LRect ViewRect = new LRect(Rect);
        //    //            Matrix oCC_inv = oCC.Clone().Invert();

        //    /*
        //Point[] pts = new Point[2];
        //pts[0].X = (int)ViewRect.X1;
        //pts[0].Y = (int)ViewRect.Y1;
        //pts[1].X = (int)ViewRect.X2;
        //pts[1].Y = (int)ViewRect.Y2;

        //oCC_inv.TransformPoints(pts);

        //ViewRect = new LRect(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);*/

        //    //m_oRenderGDI.BeginRender(g);
        //    int iF;
        //    for (iF = 0; iF < m_oFeatures.Count; iF++)
        //    {
        //        Feature oF = m_oFeatures[iF];

        //        //if (oF.MBR.IntersectsWith(ViewRect) == false) continue;

        //        if (oF is PolylineFeature)
        //        {
        //            PolylineFeature oP = (PolylineFeature)oF;
        //            // draw polyline
        //            for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
        //            {
        //                List<DPoint> Points_list = oP.m_oParts[iPart].Points;
        //                if (Points_list.Count > 0)
        //                {
        //                    DPoint[] Points_src = oP.m_oParts[iPart].Points_;
        //                    int ipp_count = Points_src.GetLength(0);
        //                    Point[] Points_array = new Point[ipp_count];
        //                    for (int ipp = 0; ipp < ipp_count; ipp++)
        //                    {
        //                        oCC.Convert((double)Points_src[ipp].X, (double)Points_src[ipp].Y);
        //                        Points_array[ipp] = new Point((int)oCC.X, (int)oCC.Y);
        //                    }

        //                    m_oRenderGDIplus.DrawPolylineFeature(g, Points_array, oP.Style.LinePattern, oP.Style.LineColor, oP.Style.LineWidth);
        //                }
        //            }
        //        }
        //        /*else if (oF is BitmapFeature)
        //        {
        //            BitmapFeature oBF = (BitmapFeature)oF;
        //            Point[] Points_array = new Point[2];
        //            Points_array[0].X = oBF.Position.X;
        //            Points_array[0].Y = oBF.Position.Y + oBF.Position.Height;
        //            Points_array[1].X = oBF.Position.X + oBF.Position.Width + 1;
        //            Points_array[1].Y = oBF.Position.Y - 1;

        //            oCC.TransformPoints(Points_array);
        //            g.DrawImage(oBF.Bitmap, Points_array[0].X, Points_array[0].Y,
        //                Points_array[1].X - Points_array[0].X, Points_array[1].Y - Points_array[0].Y);

        //        } */
        //        else if (oF is PolygonFeature)
        //        {
        //            PolygonFeature oP = (PolygonFeature)oF;
        //            // draw polyline
        //            for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
        //            {
        //                List<DPoint> Points_list = oP.m_oParts[iPart].Points;
        //                if (Points_list.Count > 0)
        //                {
        //                    {
        //                        //oCC.TransformPoints(Points_array); // to dzia³a, ale powolnie
        //                    }

        //                    double a = Convert.ToDouble(oCC.atMaster.A);
        //                    double b = Convert.ToDouble(oCC.atMaster.B);
        //                    int c = Convert.ToInt32(oCC.atMaster.C);
        //                    double d = Convert.ToDouble(oCC.atMaster.D);
        //                    double e = Convert.ToDouble(oCC.atMaster.E);
        //                    int f = Convert.ToInt32(oCC.atMaster.F);

        //                    DPoint[] Points_src = oP.m_oParts[iPart].Points_;
        //                    int ipp_count = Points_src.GetLength(0);
        //                    Point[] Points_array = new Point[ipp_count + 1];
        //                    for (int ipp = 0; ipp < ipp_count; ipp++)
        //                    {
        //                        int x = (int)(Points_src[ipp].X * a + Points_src[ipp].Y * b) + c;
        //                        int y = (int)(Points_src[ipp].X * d + Points_src[ipp].Y * e) + f;
        //                        Points_array[ipp].X = x;
        //                        Points_array[ipp].Y = y;
        //                    }
        //                    Points_array[ipp_count] = Points_array[0];  // last same as first


        //                    m_oRenderGDIplus.DrawPolygonFeature(g, Points_array, oP.Style.LinePattern, oP.Style.LineColor, oP.Style.LineWidth,
        //                        oP.Style.FillPattern, oP.Style.RegionColor, oP.Style.RegionBackColor);
        //                }
        //            }
        //        }
        //        /*else if (oF is TextFeature)
        //        {
        //            TextFeature oTF = oF as TextFeature;

        //            Point[] Points_array = new Point[2];
        //            Points_array[0].X = oTF.Position.X;
        //            Points_array[0].Y = oTF.Position.Y;
        //            Points_array[1].X = oTF.Position.X;
        //            Points_array[1].Y = oTF.Position.Y + (int)oTF.Style.FontSize;


        //            oCC.TransformPoints(Points_array);
        //            float size = Math.Abs(Points_array[1].Y - Points_array[0].Y);
        //            if (size == 0) continue;
        //            Font oFont = new Font(oTF.Style.FontName, size);

        //            SizeF iTextLen = g.MeasureString(oTF.Text, oFont);
        //            float x, y;

        //            if (oTF.Style.Justify == 0)
        //            {
        //                x = Points_array[0].X - iTextLen.Width / 2;
        //                y = Points_array[0].Y + iTextLen.Height / 2;
        //            }
        //            else if (oTF.Style.Justify == 1)
        //            {
        //                x = Points_array[0].X - iTextLen.Width;
        //                y = Points_array[0].Y;
        //            }
        //            else if (oTF.Style.Justify == 2)
        //            {
        //                x = Points_array[0].X - iTextLen.Width / 2;
        //                y = Points_array[0].Y;
        //            }
        //            else if (oTF.Style.Justify == 3)
        //            {
        //                x = Points_array[0].X;
        //                y = Points_array[0].Y;
        //            }
        //            else if (oTF.Style.Justify == 4)
        //            {
        //                x = Points_array[0].X - iTextLen.Width;
        //                y = Points_array[0].Y + iTextLen.Height / 2;
        //            }
        //            else if (oTF.Style.Justify == 5)
        //            {
        //                x = Points_array[0].X;
        //                y = Points_array[0].Y + iTextLen.Height / 2;
        //            }
        //            else if (oTF.Style.Justify == 6)
        //            {
        //                x = Points_array[0].X - iTextLen.Width;
        //                y = Points_array[0].Y + iTextLen.Height;
        //            }
        //            else if (oTF.Style.Justify == 7)
        //            {
        //                x = Points_array[0].X - iTextLen.Width / 2;
        //                y = Points_array[0].Y + iTextLen.Height;
        //            }
        //            else if (oTF.Style.Justify == 8)
        //            {
        //                x = Points_array[0].X;
        //                y = Points_array[0].Y + iTextLen.Height;
        //            }




        //            g.DrawString(oTF.Text, oFont, Brushes.Black, Points_array[0].X, Points_array[0].Y);
        //        }*/
        //        else if (oF is SymbolFeature)
        //        {
        //            SymbolFeature oP = (SymbolFeature)oF;

        //            oCC.Convert(oP.x, oP.y);
        //            Rectangle rect = new Rectangle((int)oCC.X-2, (int)oCC.Y-2, 4, 4);
        //            m_oRenderGDIplus.DrawRectangle(g, rect, (int)0x7f000000);
        //        }
        //        else if (oF is RectangleFeature)
        //        {
        //            RectangleFeature oP = (RectangleFeature)oF;

        //            oCC.Convert(oP.x, oP.y);
        //            Rectangle rect = new Rectangle((int)oCC.X-2, (int)oCC.Y-2, 4, 4);
        //            m_oRenderGDIplus.DrawRectangle(g, rect, (uint)0xff000000);
        //        }
        //        else
        //        {
        //            throw new Exception("Unsupported feature type");
        //        }
        //    }

        //    //m_oRenderGDI.EndRender();
        //}


        // angle beween OX axis and [dx,dy] vector
        // result is normalized: [0..2PI)
        private double calcAngle(double dx, double dy)
        {
            double len = Math.Sqrt(dx * dx + dy * dy);
            dx = dx / len;
            dy = dy / len;

            double dAngle_base = Math.Acos(dx);
            if (dy < 0) dAngle_base = 2 * Math.PI - dAngle_base;
            return dAngle_base;
        }

        // normalize angle, make angle be in the range [0..2PI)
        private double normAngle(double Angle)
        {
            int iCount = (int)(Angle / (2 * Math.PI));
            if (Angle < 0) iCount--;
            return Angle - (2 * Math.PI * iCount);
        }

        private double segment_angle = 2.0 * Math.PI / 24;
        private int radius = 200;

        private Point normalise(Point pt)
        {
            double len = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
            double x = (double)(pt.X * radius) / len;
            double y = (double)(pt.Y * radius) / len;
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        private void addArc(List<Point> points, Point pt, double start, double end)
        {
            double act;
            if (end < start) end += 2 * Math.PI;
            for (act = start; act < end; act += segment_angle)
            {
                Point po = new Point((int)Math.Round(radius * Math.Cos(act)),
                                    (int)Math.Round(radius * Math.Sin(act)));
                po.Offset(pt);
                points.Add(po);
            }

            if ((end - act + segment_angle) >= (Math.PI / 180.0))
            {
                Point po = new Point((int)Math.Round(radius * Math.Cos(end)),
                                    (int)Math.Round(radius * Math.Sin(end)));
                po.Offset(pt);
                points.Add(po);
            }
        }

        public bool Visible
        {
            get
            {
                return m_bVisible;
            }
            set
            {
                m_bVisible = value;
            }
        }

        public void FeaturesAdd(Feature oFeature)
        {
            Feature f = oFeature.clone();

            featuresContainer.addFeature(oFeature);

            if (this.DataTable != null)
            {
                this.DataTable.Rows.Add(); // add empty row
            }

            // invalidate map, request redraw
            Invalidate();
        }

        public void FeaturesRemoveAt(int index)
        {
            // detach feature
            Feature feature = featuresContainer.getFeature(index);
            feature.FeatureID = -1;
            feature.setParentContainer(null);
            feature.Selected = false;

            featuresContainer.removeFeature(index);
        }

        public int FeaturesCount
        {
            get { return featuresContainer.Count; }
        }

        public void FeaturesClear()
        {
            // detach features
            for (int i = 0; i < featuresContainer.Count; i++)
            {
                Feature feature = featuresContainer.getFeature(i);
                if (feature != null)
                {
                    feature.FeatureID = -1;
                    feature.setParentContainer(null);
                    feature.Selected = false;
                }
            }

            featuresContainer.clear();

            if (this.DataTable != null)
            {
                this.DataTable.Rows.Clear();
            }
            //m_oFeatures.Clear();
            //m_bBoundsOK = false;
            // invalidate map, request redraw
            Invalidate();
        }

        /// <summary>
        /// Search for features in the vicinity of pt
        /// pt in coordsys NumericCoordsys
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public List<Feature> Search(DPoint pt)
        {
            double distanceInPixels = 5; // 5pixels

            List<Feature> ftrs = new List<Feature>();
            int iF;
            for (iF = 0; iF < featuresContainer.Count; iF++)
            {
                Feature oF = featuresContainer.getFeature(iF);
                if (oF is SymbolFeature)
                {
                    SymbolFeature oP = (SymbolFeature)oF;

                    double distSqr = (pt.X - oP.x) * (pt.X - oP.x) + (pt.Y - oP.y) * (pt.Y - oP.y);
                    // TODO: calc pixel -> layerCoordsys
                    if (distSqr < distanceInPixels)
                    {
                        //ftrs.Add(oF);
                    }
                }
            }
            return ftrs;
        }



        /// <summary>
        /// gather polyline points
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public List<DPoint> SearchForPolylinePoints(DRect rect)
        {
            List<DPoint> res = new List<DPoint>();
            for (int i = 0; i < featuresContainer.Count; i++)
            {
                Feature feature = featuresContainer.getFeature(i);

                // building points of polyline
                // select polyline points
                if (feature is PolylineFeature)
                {
                    PolylineFeature oP = (PolylineFeature)feature;

                    // draw polyline points
                    for (int iPart = 0; iPart < oP.m_oParts.Count; iPart++)
                    {
                        List<DPoint> Points_list = oP.m_oParts[iPart].Points;
                        if (Points_list.Count > 0)
                        {
                            List<DPoint> points = oP.m_oParts[iPart].Points;
                            for (int ipp = 0; ipp < points.Count; ipp++)
                            {
                                if (rect.Contains(points[ipp].X, points[ipp].Y))
                                {
                                    res.Add(points[ipp]);
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }



        public void castToPolyline(PolylineFeature feature, int part, double x, double y,
            out double a, out double x_cast, out double y_cast,
            out double dDist, out double dPolyLen,
            out int segNo, out double a_seg, out double b_seg)
        {
            featuresContainer.castToPolyline(feature, part, x, y, out a, out x_cast, out y_cast,
                out  dDist, out  dPolyLen,
                out  segNo, out  a_seg, out  b_seg);
        }



        public List<Feature> SearchForFeaturesColliding(DRect rect)
        {
            List<Feature> res = new List<Feature>();
            for (int i = 0; i < featuresContainer.Count; i++)
            {
                Feature feature = featuresContainer.getFeature(i);
                if (feature == null) continue;

                if (feature.MBR.IntersectsWith(rect))
                {
                    if (feature is PolylineFeature)
                    {
                        PolylineFeature oP = (PolylineFeature)feature;
                        double x = (rect.X1 + rect.X2) / 2;
                        double y = (rect.Y1 + rect.Y2) / 2;

                        for (int p = 0; p < oP.m_oParts.Count; p++)
                        {
                            double a, x_cast, y_cast;
                            double dist, polylen, a_seg, b_seg;
                            int segNo;

                            if (oP.m_oParts[p].Points.Count > 1)
                            {
                            featuresContainer.castToPolyline(oP, 0, x, y,
                                out a, out x_cast, out y_cast,
                                out dist, out polylen,
                                out segNo, out a_seg, out b_seg);

                                if (dist < rect.Width())
                                {
                                    res.Add(feature);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        res.Add(feature);
                    }
                }
            }
            return res;
        }

        public List<Feature> SearchForFeaturesContained(DRect rect)
        {
            List<Feature> res = new List<Feature>();
            for (int i = 0; i < featuresContainer.Count; i++)
            {
                Feature feature = featuresContainer.getFeature(i);
                if (feature != null)
                {
                    if (rect.Contains(feature.MBR))
                    {
                        res.Add(feature);
                    }
                }
            }
            return res;
        }

        public void selectionClear()
        {
            for (int f = 0; f < featuresContainer.Count; f++)
            {
                Feature feature = featuresContainer.getFeature(f);
                if (feature == null) continue;

                feature.Selected = false;
                if (feature is PolylineFeature)
                {
                    PolylineFeature pf = (PolylineFeature)feature;
                    for (int p = 0; p < pf.m_oParts.Count; p++)
                    {
                        List<DPoint> points = pf.m_oParts[p].Points;
                        for (int i = 0; i < points.Count; i++)
                            points[i].Selected = false;
                    }
                }
            }
            Invalidate();
        }

        public List<Feature> selectionFeatures()
        {
            List<Feature> res = new List<Feature>();
            for (int f = 0; f < featuresContainer.Count; f++)
            {
                Feature feature = featuresContainer.getFeature(f);
                if (feature != null)
                {
                    if (feature.Selected) res.Add(feature);
                }
            }
            return res;
        }


        /// <summary>
        /// Collect selected points.
        /// </summary>
        /// <param name="includeCompleteSelectedPolylines">true=treat every point of selected polyline as selected point.</param>
        /// <returns></returns>
        public List<DPoint> selectionPoints(bool includeCompleteSelectedPolylines)
        {
            List<DPoint> res = new List<DPoint>();
            for (int f = 0; f < featuresContainer.Count; f++)
            {
                Feature feature = featuresContainer.getFeature(f);
                if (feature is PolylineFeature)
                {
                    PolylineFeature pf = (PolylineFeature)feature;
                    if (pf.Selected && includeCompleteSelectedPolylines)
                    {
                        // add all points
                        for (int p = 0; p < pf.m_oParts.Count; p++)
                        {
                            List<DPoint> points = pf.m_oParts[p].Points;
                            for (int i = 0; i < points.Count; i++)
                            {
                                res.Add(points[i]);
                            }
                        }
                    }
                    else
                    {
                        // add selected points
                        for (int p = 0; p < pf.m_oParts.Count; p++)
                        {
                            List<DPoint> points = pf.m_oParts[p].Points;
                            for (int i = 0; i < points.Count; i++)
                            {
                                if (points[i].Selected) res.Add(points[i]);
                            }
                        }
                    }
                }
            }
            return res;
        }

        UndoManager manager = null;

        public UndoManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new UndoManager(featuresContainer);

                return manager;
            }
        }

        DataTable dataTable = null;

        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        public Feature FeatureGet(int iFeature)
        {
            return featuresContainer.getFeature(iFeature);
        }
    }
}

