using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GMap.NET;
using System.Drawing;
using hiMapNet;

namespace gpxEditor
{
    /// <summary>
    /// This tool is for editing GPX traks stored on Polyline and Symbol layers.
    /// This tool allows to select area of featrues or polyline points.
    /// If dragging it allows to relocate selected features or polyline points.
    /// 
    /// This tool uses two layers: Polyline layer and Symbol layer.
    /// PolylineLayer is used to do graphical edition.
    /// Symbol Layer is used to store points data (lon, lat, datetime, ele, etc.)
    /// 
    /// 
    /// Mouse Down logic:
    /// 1. enter
    /// 2. if hit Point than:
    ///    2.1 action:
    ///    Sel, Ctrl, Shift
    ///    0,0,0 -> Clear Selection; AddToSelection; enter DragMode (1)
    ///    0,0,1 -> AddToSelection; enter DragMode (1)
    ///    0,1,0 -> NOP; enter DragMode (1)
    ///    0,1,1 -> NOP; enter RotationMode (2)
    ///    1,0,0 -> NOP; enter DragMode (1)
    ///    1,0,1 -> NOP; enter DragMode (1)
    ///    1,1,0 -> NOP; enter DragMode (1)
    ///    1,1,1 -> NOP; enter RotationMode (2)
    /// 
    /// note: Sel = hit point is selected
    ///       Ctrl = control key is pressed
    ///       Shift = shift key is pressed
    ///       NOP = no operation
    /// 
    ///    2.2 Exit
    ///    
    /// 3. if hit Polyline than:
    ///    3.1 action:
    ///    Sel, Ctrl, Shift
    ///    0,0,0 -> Clear Selection; AddToSelection; enter DragMode (1)
    ///    0,0,1 -> AddToSelection; enter DragMode (1)
    ///    0,1,0 -> NOP; enter DragMode (1)
    ///    0,1,1 -> NOP; enter RotationMode (2)
    ///    1,0,0 -> NOP; enter DragMode (1)
    ///    1,0,1 -> NOP; enter DragMode (1)
    ///    1,1,0 -> NOP; enter DragMode (1)
    ///    1,1,1 -> NOP; enter RotationMode (2)
    ///    
    ///    3.2 Exit
    /// 
    /// 4. if (Ctrl=1 and Shift=1) than
    ///         enter RotationMode (2)
    ///    else
    ///         enter RectSelectionMode (3)
    ///         
    /// Mouse Move logic:
    /// 1. if (DragMode) than action: Relocate
    /// 2. if (RotationMode) than action: Rotate
    /// 3. Exit
    /// 
    /// Mouse Up logic:
    /// 1. if (DragMode) than action: Relocate
    /// 2. if (RotationMode) than action: Rotate
    /// 3. if (RectSelectionMode) than
    ///    3.1 action:
    ///       Shift, Ctrl
    ///       0,0 -> Clear Selection; AddToSelection;
    ///       1,0 -> AddToSelection;
    ///       0,1 -> NOP
    ///       1,1 -> NOP
    /// 4. if hit Point
    ///    4.1 action:
    ///    Sel, Ctrl, Shift
    ///    0,0,0 -> NOP
    ///    0,0,1 -> NOP
    ///    0,1,0 -> AddToSelection
    ///    0,1,1 -> NOP
    ///    1,0,0 -> ClearSelection; AddToSelection
    ///    1,0,1 -> NOP
    ///    1,1,0 -> RemoveFromSelection
    ///    1,1,1 -> NOP
    ///    
    /// 5. if hit Polyline
    ///    5.1 action:
    ///    Sel, Ctrl, Shift
    ///    0,0,0 -> NOP
    ///    0,0,1 -> NOP
    ///    0,1,0 -> AddToSelection
    ///    0,1,1 -> NOP
    ///    1,0,0 -> ClearSelection; AddToSelection
    ///    1,0,1 -> NOP
    ///    1,1,0 -> Deselect Polyline
    ///    1,1,1 -> NOP
    ///    
    /// TODO: animations on mouse move
    /// TODO: rotation (is it needed)
    /// 
    /// </summary>
    public class MapToolSelectMove : MapTool
    {
        //public event WptSelectEventArg WptSelected;
        public event EventHandler ToolUsed;

        LayerVectors polylineLayer = null;

        public LayerVectors PolylineLayer
        {
            get { return polylineLayer; }
            set { polylineLayer = value; }
        }
        LayerVectors symbolLayer = null;

        public LayerVectors SymbolLayer
        {
            get { return symbolLayer; }
            set { symbolLayer = value; }
        }

        bool relocateMode = false;
        bool relocateModeAllowRelocation = false; // true if movement was big enough (e.g. more that 5 pixels)
        bool rotateMode = false;
        bool rectSelectionMode = false;

        Timer timer = null;

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            base.MouseDown(sender, e);

            bool modified = false;

            relocateMode = false;
            rotateMode = false;
            rectSelectionMode = false;

            bool keyShift = ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift);
            bool keyControl = ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control);

            // try to selec polyline point
            DRect rect = calculateRectangleFromPoint(e.X, e.Y);
            List<DPoint> points = polylineLayer.SearchForPolylinePoints(rect);
            if (points.Count > 0)
            {
                // if clicked on select point we will drag it
                if (points[0].Selected == false)
                {
                    // if not SHIFT clear selection
                    if (!keyShift && !keyControl)
                    {
                        polylineLayer.selectionClear();
                        modified = true;
                    }
                    if (!keyControl)
                    {
                        points[0].Selected = true;
                        MapControl.Globals.Instance.MapControl.InvalidateMap();
                        modified = true;
                    }
                }
                if (keyControl && keyShift) rotateMode = true;
                else
                {
                    relocateMode = true;
                    relocateModeAllowRelocation = false;
                }

                m_eToolDragShape = MapControl.ToolDragShapeConst.Point;

                if (modified)
                {
                    if (ToolUsed != null) ToolUsed(this, new EventArgs());
                }
                return;
            }

            // try to select an object
            List<Feature> features = polylineLayer.SearchForFeaturesColliding(rect);
            if (features.Count > 0)
            {
                if (features[0].Selected == false)
                {
                    // if not SHIFT clear selection
                    if (!keyShift && !keyControl)
                    {
                        polylineLayer.selectionClear();
                        modified = true;
                    }
                    if (!keyControl)
                    {
                        features[0].Selected = true;
                        MapControl.Globals.Instance.MapControl.InvalidateMap();
                        modified = true;
                    }
                }
                if (keyControl && keyShift) rotateMode = true;
                else
                {
                    relocateMode = true;
                    relocateModeAllowRelocation = false;
                }

                // we will drag feature
                m_eToolDragShape = MapControl.ToolDragShapeConst.Point;

                if (modified)
                {
                    if (ToolUsed != null) ToolUsed(this, new EventArgs());
                }
                return;
            }

            // we clicked empty space: we start rectangle selection
            // ativate drag mode
            if (keyControl && keyShift)
            {
                rotateMode = true;
                m_eToolDragShape = MapControl.ToolDragShapeConst.Point;
            }
            else
            {
                rectSelectionMode = true;
                m_eToolDragShape = MapControl.ToolDragShapeConst.Rectangle;
            }
        }

        private DRect calculateRectangleFromPoint(int x, int y)
        {
            // try to select a polyline point
            int margin = 3;

            System.Drawing.Point p1 = new System.Drawing.Point(x - margin, y - margin);
            System.Drawing.Point p2 = new System.Drawing.Point(x + margin, y + margin);

            DPoint pt1, pt2;
            map.DisplayTransform.FromDisplay(p1, out pt1);
            map.DisplayTransform.FromDisplay(p2, out pt2);

            // convert mercator to wgs84
            CoordConverter occ = new CoordConverter();
            CoordSys oCSMercator = CoordSysFactory.CreateCoordSys(CoordSysType.Mercator, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            CoordSys oCSWGS84 = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
            occ.Init(oCSMercator, oCSWGS84);
            occ.Convert(pt1.X, pt1.Y);
            DPoint dp1 = new DPoint(occ.X, occ.Y);
            occ.Convert(pt2.X, pt2.Y);
            DPoint dp2 = new DPoint(occ.X, occ.Y);
            DRect rect = new DRect(dp1.X, dp2.Y, dp2.X, dp1.Y);
            return rect;
        }


        /// <summary>
        /// Select all objects within the rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool modified = false;

            base.MouseUp(sender, e);

            bool keyShift = ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift);
            bool keyControl = ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control);

            MapControl.Globals.Instance.MapControl.Cursor = Cursors.Arrow;

            if (ActiveRectangle.Width == 0 && ActiveRectangle.Height == 0)
            {
                relocateMode = false;
                rotateMode = false;
                rectSelectionMode = false;
                relocateModeAllowRelocation = false;

                DRect rect = calculateRectangleFromPoint(e.X, e.Y);
                List<DPoint> points = polylineLayer.SearchForPolylinePoints(rect);
                if (points.Count > 0)
                {
                    if (points[0].Selected)
                    {
                        if (!keyControl && !keyShift)
                        {
                            polylineLayer.selectionClear();
                            points[0].Selected = true;
                            MapControl.Globals.Instance.MapControl.InvalidateMap();
                            modified = true;
                        }
                        if (keyControl && !keyShift)
                        {
                            points[0].Selected = false;
                            MapControl.Globals.Instance.MapControl.InvalidateMap();
                            modified = true;
                        }
                    }
                    else
                    {
                        if (keyControl && !keyShift)
                        {
                            points[0].Selected = true;
                            MapControl.Globals.Instance.MapControl.InvalidateMap();
                            modified = true;
                        }
                    }
                    if (modified)
                    {
                        if (ToolUsed != null) ToolUsed(this, new EventArgs());
                    }
                    return;
                }

                // select polyline
                // try to select an object
                List<Feature> features = polylineLayer.SearchForFeaturesColliding(rect);
                if (features.Count > 0)
                {
                    if (features[0].Selected)
                    {
                        if (!keyControl && !keyShift)
                        {
                            polylineLayer.selectionClear();
                            features[0].Selected = true;
                            modified = true;
                            MapControl.Globals.Instance.MapControl.InvalidateMap();
                        }
                        if (keyControl && !keyShift)
                        {
                            features[0].Selected = false;
                            modified = true;
                            MapControl.Globals.Instance.MapControl.InvalidateMap();
                        }
                    }
                    else
                    {
                        if (keyControl && !keyShift)
                        {
                            features[0].Selected = true;
                            modified = true;
                            MapControl.Globals.Instance.MapControl.InvalidateMap();
                        }
                    }
                    if (modified)
                    {
                        if (ToolUsed != null) ToolUsed(this, new EventArgs());
                    }
                    return;
                }

                if (!keyControl && !keyShift)
                {
                    polylineLayer.selectionClear();
                    modified = true;
                }
            }
            else
            {
                // Dragging done
                if (relocateMode && relocateModeAllowRelocation)
                {
                    List<Feature> features = polylineLayer.selectionFeatures();
                    List<DPoint> points = polylineLayer.selectionPoints(true);

                    // calculate delta...
                    // mouse start: m_oMouseStart
                    // mouse end: m_oMouseCurrent

                    int dxi = m_oMouseStart.X - m_oMouseCurrent.X;
                    int dyi = m_oMouseStart.Y - m_oMouseCurrent.Y;
                    int pixelDelta = (int)Math.Sqrt(dxi * dxi + dyi * dyi);

                    DRect r1 = calculateRectangleFromPoint(m_oMouseStart.X, m_oMouseStart.Y);
                    DRect r2 = calculateRectangleFromPoint(m_oMouseCurrent.X, m_oMouseCurrent.Y);

                    double dx = r2.X1 - r1.X1;
                    double dy = r2.Y1 - r1.Y1;

                    // store copy of original point for undo
                    polylineLayer.Manager.startRecordingUndoElement();
                    symbolLayer.Manager.startRecordingUndoElement();

                    // move polylines
                    for (int i = 0; i < points.Count; i++)
                    {
                        DPoint newPoint = new DPoint(points[i].X + dx, points[i].Y + dy);

                        polylineLayer.Manager.recordMovePoint(points[i], newPoint);

                        // find correcponding symbol in symbolLayer
                        SymbolFeature symbol = (SymbolFeature)points[i].Tag;
                        Debug.Assert(symbol != null);

                        SymbolFeature newSymbol = new SymbolFeature(symbol.x + dx, symbol.y + dy);
                        symbolLayer.Manager.recordMoveFeature(symbol, newSymbol);
                        modified = true;
                    }

                    polylineLayer.Manager.stopRecordingUndoElement();
                    symbolLayer.Manager.stopRecordingUndoElement();

                    // refresh view
                    MapControl.Globals.Instance.MapControl.InvalidateMap();
                }
                if (rotateMode)
                {
                    // rotation


                }
                if (rectSelectionMode)
                {
                    if (!keyShift && !keyControl)
                    {
                        polylineLayer.selectionClear();
                        modified = true;

                        /*
                        // emit clear selection for all wpt-s
                        for (int i = 0; i < symbolLayer.FeaturesCount; i++)
                        {
                            SymbolFeature symbol = symbolLayer.FeatureGet(i) as SymbolFeature;
                            if (symbol != null)
                            {
                                //WptSelected(this, (symbol.Tag as GpxWpt), false);
                            }
                        }*/
                    }

                    if (!keyControl)
                    {
                        System.Drawing.Rectangle r = ActiveRectangle;
                        System.Drawing.Point p1 = new System.Drawing.Point(r.X, r.Y);
                        System.Drawing.Point p2 = new System.Drawing.Point(r.X + r.Width, r.Y + r.Height);

                        DPoint pt1, pt2;
                        map.DisplayTransform.FromDisplay(p1, out pt1);
                        map.DisplayTransform.FromDisplay(p2, out pt2);

                        // convert mercator to wgs84
                        CoordConverter occ = new CoordConverter();
                        CoordSys oCSMercator = CoordSysFactory.CreateCoordSys(CoordSysType.Mercator, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
                        CoordSys oCSWGS84 = CoordSysFactory.CreateCoordSys(CoordSysType.LatLong, CoordSysFactory.CreateDatum(DatumID.WGS84), new AffineTransform());
                        occ.Init(oCSMercator, oCSWGS84);
                        occ.Convert(pt1.X, pt1.Y);
                        DPoint dp1 = new DPoint(occ.X, occ.Y);
                        occ.Convert(pt2.X, pt2.Y);
                        DPoint dp2 = new DPoint(occ.X, occ.Y);
                        DRect rect = new DRect(dp1.X, dp2.Y, dp2.X, dp1.Y);

                        List<DPoint> points = polylineLayer.SearchForPolylinePoints(rect);
                        for (int i = 0; i < points.Count; i++)
                        {
                            points[i].Selected = true;
                            modified = true;
                        }

                        List<Feature> features = polylineLayer.SearchForFeaturesContained(rect);
                        for (int i = 0; i < features.Count; i++)
                        {
                            features[i].Selected = true;
                            modified = true;
                        }

                        MapControl.Globals.Instance.MapControl.InvalidateMap();
                    }
                }

                if (timer != null)
                {
                    timer.Stop();
                    timer = null;
                }

                relocateMode = false;
                rotateMode = false;
                rectSelectionMode = false;
                relocateModeAllowRelocation = false;
            }
            if (modified)
            {
                if (ToolUsed != null) ToolUsed(this, new EventArgs());
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);

            bool keyShift = ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift);
            bool keyControl = ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control);

            Debug.Print("mouse move relocateMode=" + relocateMode + relocateModeAllowRelocation + ActiveRectangle);

            if (ActiveRectangle.Width == 0 && ActiveRectangle.Height == 0) return;

            // Dragging done
            if (relocateMode && relocateModeAllowRelocation == false)
            {
                int dxi = m_oMouseStart.X - m_oMouseCurrent.X;
                int dyi = m_oMouseStart.Y - m_oMouseCurrent.Y;
                int pixelDelta = (int)Math.Sqrt((dxi * dxi + dyi * dyi));
                /*
                if (pixelDelta > 5)
                {
                    relocateModeAlowRelocation = true;
                    MapControl.Globals.Instance.MapControl.Cursor = Cursors.SizeAll;
                }*/
                int movement = Math.Abs(dxi) + Math.Abs(dyi);
                if (movement > 0)
                {
                    // start timer, after 500 milisec activate movement
                    lock (this)
                    {
                        if (timer == null)
                        {
                            timer = new Timer();
                            timer.Interval = 200;
                            timer.Tick += new EventHandler(timer_Tick);
                            timer.Start();
                        }
                    }
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lock (this)
            {
                relocateModeAllowRelocation = true;
                MapControl.Globals.Instance.MapControl.Cursor = Cursors.SizeAll;
                timer.Stop();
                timer = null;
            }
        }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            base.KeyDown(sender, e);
            // delete selected features and points

            if (e.KeyCode == Keys.Delete)
            {
                // delete selected points, split polyline into two where applicable

                // start recording undo step
                polylineLayer.Manager.startRecordingUndoElement();
                symbolLayer.Manager.startRecordingUndoElement();

                int count = polylineLayer.FeaturesCount;
                for (int iFeature = count - 1; iFeature >= 0; iFeature--)
                {
                    Feature feature = polylineLayer.FeatureGet(iFeature);
                    if (feature == null) continue;

                    if (feature.Selected)
                    {
                        // remove this polyliline and all related symbols
                        PolylineFeature poly = (PolylineFeature)feature;
                        for (int iPart = 0; iPart < poly.m_oParts.Count; iPart++)
                        {
                            Part part = poly.m_oParts[iPart];
                            for (int iPoint = 0; iPoint < part.Points.Count; iPoint++)
                            {
                                DPoint point = part.Points[iPoint];
                                symbolLayer.Manager.recordRemoveFeature((SymbolFeature)point.Tag);
                            }
                        }
                        polylineLayer.Manager.recordRemoveFeature((PolylineFeature)feature);
                    }
                    else
                    {
                        // building points of polyline
                        // select polyline points
                        if (feature is PolylineFeature)
                        {
                            PolylineFeature oP = (PolylineFeature)feature;

                            // if this polyline has selected point(s), it will be split
                            // calculate number of new polylines

                            int newPolylines = 0;
                            int partCount = oP.m_oParts.Count;
                            for (int iPart = partCount - 1; iPart >= 0; iPart--)
                            {
                                List<DPoint> points = oP.m_oParts[iPart].Points;
                                int pointCount = points.Count;
                                for (int iPoint = 1; iPoint < pointCount; iPoint++)
                                {
                                    if (iPoint == 1)
                                    {
                                        newPolylines = (points[0].Selected ? 0 : 1);
                                    }

                                    if (points[iPoint - 1].Selected == true
                                        && points[iPoint].Selected == false)
                                    {
                                        newPolylines++;
                                    }
                                }
                            }

                            if (newPolylines < 2)
                            {
                                // just modify existing polyline
                                int partCount1 = oP.m_oParts.Count;
                                for (int iPart = partCount1 - 1; iPart >= 0; iPart--)
                                {
                                    List<DPoint> points = oP.m_oParts[iPart].Points;
                                    int pointCount = points.Count;
                                    for (int iPoint = pointCount - 1; iPoint >= 0; iPoint--)
                                    {
                                        if (points[iPoint].Selected)
                                        {
                                            symbolLayer.Manager.recordRemoveFeature((SymbolFeature)points[iPoint].Tag);
                                            polylineLayer.Manager.recordRemovePoint(iFeature, iPart, iPoint);
                                        }
                                    }
                                    if (points.Count == 0)
                                    {
                                        polylineLayer.Manager.recordRemovePart(iFeature, iPart);
                                    }
                                }
                                if (oP.m_oParts.Count == 0)
                                {
                                    polylineLayer.Manager.recordRemoveFeature((PolylineFeature)feature);
                                }
                            }
                            else
                            {
                                // add new polyline(s), delete original
                                PolylineFeature featureNew = null;
                                List<DPoint> newPoints = null;

                                // draw polyline points
                                int partCount1 = oP.m_oParts.Count;
                                for (int iPart = partCount1 - 1; iPart >= 0; iPart--)
                                {
                                    List<DPoint> points = oP.m_oParts[iPart].Points;
                                    int pointCount = points.Count;
                                    for (int iPoint = 0; iPoint < pointCount; iPoint++)
                                    {
                                        if (points[iPoint].Selected)
                                        {
                                            if (newPoints != null)
                                            {
                                                featureNew = new PolylineFeature(newPoints, feature.Style);
                                                polylineLayer.Manager.recordAddFeature(featureNew);
                                                featureNew = null;
                                                newPoints = null;
                                            }

                                            symbolLayer.Manager.recordRemoveFeature((SymbolFeature)points[iPoint].Tag);
                                        }
                                        else
                                        {
                                            if (newPoints == null) newPoints = new List<DPoint>();
                                            newPoints.Add((DPoint)points[iPoint].Clone());
                                        }
                                    }
                                }

                                if (newPoints != null)
                                {
                                    featureNew = new PolylineFeature(newPoints, feature.Style);
                                    polylineLayer.Manager.recordAddFeature(featureNew);
                                    featureNew = null;
                                    newPoints = null;
                                }

                                polylineLayer.Manager.recordRemoveFeature(feature);
                            }
                        }
                    }
                }

                // finish recording undo step
                polylineLayer.Manager.stopRecordingUndoElement();
                symbolLayer.Manager.stopRecordingUndoElement();

                // emit event 
                ToolUsed(this, new EventArgs());
            }
            // clear selection
            MapControl.Globals.Instance.MapControl.InvalidateMap();
        }
    }
}
