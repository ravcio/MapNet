using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;

namespace hiMapNet
{
    /* todo: 
     * ³¹czenie z danymi
     * problem precyzji bitmapek
     * 
     * potrzebna funkcjonalnoœæ:
     * zapis/odczyt do pliku binarnego
     * import/export z TAB (z uzyciem hiMap2)
     * ³¹czenie z danymi
     * - styl linii przerywany (drobno), wszystkie style linii (czy siê da?)
     * gruboœci linii
     * drukowanie
     * generacja bitmapek png
     * justyfikacja tekstow, efekt halo, box, obroty
     * indeks przestrzenny (bounding boxes)
     * SearchAtPoint, SearchWithinRectangle
     * LocateOnPolyline, CastToPolyline
     * Operacje na poligonach
     */


    public partial class MapControl : UserControl
    {
        private Map m_oMap = null;
        private Point m_oMouseStart;
        private Point m_oMouseCurrent;

        // tool
        private MapTool currentToolProcessor = null;

        public MapTool CurrentToolProcessor
        {
            get { return currentToolProcessor; }
            set
            {
                m_eTool = ToolConst.CustomToolProcessor;

                this.Cursor = Cursors.UpArrow;

                currentToolProcessor = value;
                if (currentToolProcessor != null) currentToolProcessor.Init(Map);

            }
        }


        // pan tool
        private int m_iOffsetXStart = 0;
        private int m_iOffsetYStart = 0;


        // arrow / select tool
        //private bool m_bDrawSelection = false; // true = arrow/selection tool is used at the moment

        private ToolConst m_eTool;

        /*
                private ToolDragShapeConst m_eToolDragShape;
                public ToolDragShapeConst ToolDragShape
                {
                    get { return m_eToolDragShape; }
                    set { m_eToolDragShape = value; }
                }*/

        private bool m_bPanToolActive = false; // true = pan tool is used at the moment
        private bool bMouseUp = false;

        private Size controlSize;

        private bool mousePressed = false; // protect agains mouseUp without mouseDown

        public class Globals
        {
            private Globals()
            {
            }

            static Globals instance = null;

            public static Globals Instance
            {
                get
                {
                    if (instance == null) instance = new Globals();
                    return instance;
                }
            }

            MapControl mapControl = null;

            public MapControl MapControl
            {
                get { return mapControl; }
                set { mapControl = value; }
            }
        }

        public enum ToolConst
        {
            PanTool,
            ZoomInTool,
            ZoomOutTool,
            InfoTool,
            //ArrowTool,
            //AddFeatureTool,
            CustomToolProcessor
        }

        public enum ToolDragShapeConst
        {
            Point,
            Rectangle
        }

        public enum ResizeScaleConst
        {
            NoChange,
            KeepCC,
            KeepTL,
            KeepBL
        }
        private ResizeScaleConst resizeScale = ResizeScaleConst.NoChange;

        [Browsable(true)]
        [Description("Pan mode used when zoom changes."), Category("Behavior")]
        public ResizeScaleConst ResizeScaleMode
        {
            get { return resizeScale; }
            set { resizeScale = value; }
        }

        public MapControl()
        {
            InitializeComponent();
            CurrentTool = ToolConst.PanTool;
            m_oMap = new Map(this);
        }

        [Browsable(false)]
        public Map Map
        {
            get
            {
                return m_oMap;
            }
        }


        Bitmap bt_bg = null;
        Graphics g_bg = null;

        Bitmap bt_AnimationLayer = null;
        Graphics g_AnimationLayer = null;

        int m_iPanX, m_iPanY;


        //CachedBitmap cb = null;
        // todo: zrobiæ z uzyciem CachedBitmap
        private void MapControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            Bitmap bt = (bt_AnimationLayer == null ? bt_bg : bt_AnimationLayer);
            if (m_bPanToolActive == true)
            {
                DrawImageUnscaled_(e.Graphics, bt, (int)m_iPanX, (int)m_iPanY);
                return;
            }

            if (bMouseUp == true)
            {
                DrawImageUnscaled_(e.Graphics, bt, (int)m_iPanX, (int)m_iPanY);
                bMouseUp = false;

                // przeciwdzia³a skakaniu przy koñczeniu dzia³ania narzêdzia Pan
                this.Invalidate();
                return;
            }

            bool bDirtyAnimationLayer = false;

            if (m_oMap.Layers.IsDirty())
            {
                if (bt_bg == null)
                {
                    // make double buffer
                    bt_bg = new Bitmap(Width, Height, e.Graphics);
                    g_bg = Graphics.FromImage(bt_bg);
                    g_bg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    g_bg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                }

                // czyœci t³o (GDI+)
                g_bg.FillRectangle(Brushes.White, 0, 0, Width, Height);

                /*{
                    // czyœæ t³o (GDI)
                    IntPtr hdc1 = g_bg.GetHdc();
                    Win32.GDI.Rectangle(hdc1, 0, 0, Width, Height);
                    Win32.RECT oRect = new Win32.RECT();
                    oRect.Top = 0;
                    oRect.Bottom = Height;
                    oRect.Left = 0;
                    oRect.Right = Width;
                    IntPtr drawBrush = (IntPtr)Win32.GDI.CreateSolidBrush(0x00FFFFFF);
                    IntPtr drawBrush_old = (IntPtr)Win32.GDI.SelectObject(hdc1, drawBrush);
                    Win32.User.FillRect(hdc1, ref oRect, drawBrush);
                    Win32.GDI.SelectObject(hdc1, drawBrush_old);
                    g_bg.ReleaseHdc();
                }*/

                m_oMap.Draw(g_bg, new Rectangle(0, 0, Width, Height));

                // force animation redraw now
                bDirtyAnimationLayer = true;
            }

            if (m_oMap.Layers.AnimationLayer != null)
            {
                if (m_oMap.Layers.AnimationLayer.IsDirty()) bDirtyAnimationLayer = true;
            }

            if (bDirtyAnimationLayer)
            {
                if (bt_AnimationLayer == null)
                {
                    // make double buffer
                    bt_AnimationLayer = new Bitmap(Width, Height, e.Graphics);
                    g_AnimationLayer = Graphics.FromImage(bt_AnimationLayer);
                    g_AnimationLayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    g_AnimationLayer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                }
                // copy background to animation layer bitmap
                DrawImageUnscaled_(g_AnimationLayer, bt_bg, 0, 0);

                // draw animation layer
                if (m_oMap.Layers.AnimationLayer != null)
                {
                    m_oMap.DrawAnimationLayer(g_AnimationLayer, new Rectangle(0, 0, Width, Height));
                }
                bDirtyAnimationLayer = false;
            }

            if (m_oMap.Layers.AnimationLayer != null)
            {
                // copy bt_bg to screen
                DrawImageUnscaled_(e.Graphics, bt_AnimationLayer, 0, 0);
            }
            else
            {
                DrawImageUnscaled_(e.Graphics, bt_bg, 0, 0);
            }

            if (CurrentToolProcessor != null)
            {
                CurrentToolProcessor.PaintPost(e.Graphics);
            }

            if (this.DesignMode)
            {
                e.Graphics.DrawString("hiMapNet v.0.1", new Font("Arial", 16), Brushes.Black, new PointF(10, 10));
            }
        }

        private void DrawImageUnscaled_(Graphics graph_src, Bitmap bt_bg, int off_x, int off_y)
        {
            if (bt_bg == null) return;

            // GDI+
            graph_src.DrawImageUnscaled(bt_bg, off_x, off_y);

            // GDI
            /*            Graphics graphics_bt_dest = Graphics.FromImage(bt_bg);
                        IntPtr hdc_dest = graph_src.GetHdc();

                        IntPtr pSource = (IntPtr)Win32.GDI.CreateCompatibleDC(hdc_dest);
                        IntPtr HBitmap = bt_bg.GetHbitmap();
                        Win32.GDI.SelectObject(pSource, HBitmap);

                        IntPtr hdc_src = graphics_bt_dest.GetHdc();
                        Win32.GDI.BitBlt(hdc_dest, off_x, off_y, bt_bg.Width, bt_bg.Height, pSource, 0, 0, Win32.GDI.SRCCOPY);
                        Win32.GDI.DeleteDC(pSource);
                        Win32.GDI.DeleteObject(HBitmap);

                        graphics_bt_dest.ReleaseHdc();
                        graphics_bt_dest.Dispose();

                        graph_src.ReleaseHdc();*/
        }



        private void MapControl_MouseDown(object sender, MouseEventArgs e)
        {
            mousePressed = true;

            m_oMouseStart = new Point(e.X, e.Y);
            m_oMouseCurrent = m_oMouseStart;

            if (CurrentTool == ToolConst.PanTool)
            {
                m_iOffsetXStart = m_oMap.MapOffsetX;
                m_iOffsetYStart = m_oMap.MapOffsetY;
            }
            else if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.MouseDown(sender, e);
                }
            }
        }

        private void MapControl_MouseMove(object sender, MouseEventArgs e)
        {
            m_oMouseCurrent = new Point(e.X, e.Y);

            if (CurrentTool == ToolConst.PanTool)
            {
                m_bPanToolActive = false;

                if (e.Button == MouseButtons.Left)
                {
                    if (!mousePressed) return;

                    m_iPanX = m_oMouseCurrent.X - m_oMouseStart.X; // bedzie uzyte do przesuniecia bitmapy
                    m_iPanY = m_oMouseCurrent.Y - m_oMouseStart.Y;

                    this.Invalidate();
                    m_bPanToolActive = true;
                }
            }
            else if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.MouseMove(sender, e);
                }
            }
        }

        private void MapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mousePressed) return;

            if (CurrentTool == ToolConst.PanTool)
            {
                if (e.Button == MouseButtons.Left)
                {
                    m_oMouseCurrent = new Point(e.X, e.Y);
                    m_iPanX = m_oMouseCurrent.X - m_oMouseStart.X; // bedzie uzyte do przesuniecia bitmapy
                    m_iPanY = m_oMouseCurrent.Y - m_oMouseStart.Y;
                    m_oMap.MapOffsetX += m_iPanX;
                    m_oMap.MapOffsetY += m_iPanY;

                    m_oMap.Layers.Invalidate();

                    m_bPanToolActive = false;

                    bMouseUp = true;
                    m_oMap.FireViewChangedEvent();
                }
            }
            else if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.MouseUp(sender, e);
                }
            }

            { // fire ToolUsed event
                int x1 = Math.Min(m_oMouseStart.X, m_oMouseCurrent.X);
                int y1 = Math.Min(m_oMouseStart.Y, m_oMouseCurrent.Y);
                int x2 = Math.Max(m_oMouseStart.X, m_oMouseCurrent.X);
                int y2 = Math.Max(m_oMouseStart.Y, m_oMouseCurrent.Y);

                if (MapToolUsed != null)
                    MapToolUsed(this, new ToolUsedEventArgs(new Rectangle(x1, y1, x2 - x1, y2 - y1)));
            }

            mousePressed = false;
        }

        private void MapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            double dChange = e.Delta / 120.0;

            // zmiana skali przy zachowaniu po³o¿enia punktu x,y
            int x = Width / 2;
            int y = Height / 2;

            // ew. 
            x = m_oMouseCurrent.X;
            y = m_oMouseCurrent.Y;

            //Debug.Print("mouse:x={0},y={1}", x, y);

            // policz pkt na wsp. screen -> display coordsys
            double xDisp = (x - m_oMap.MapOffsetX) / m_oMap.MapScale;
            double yDisp = (y - m_oMap.MapOffsetY) / m_oMap.MapScale;

            // zmien skale

            m_oMap.MapScale = m_oMap.MapScale * (Math.Pow(2.0, dChange));
            if (m_oMap.MapScale > 1e10) m_oMap.MapScale = 1e10;
            if (m_oMap.MapScale < 1e-10) m_oMap.MapScale = 1e-10;

            // przelicz nowe polozenie tak, aby xDisp ma sie znalezc w miejscu x
            m_oMap.MapOffsetX = x - (int)(xDisp * m_oMap.MapScale);
            m_oMap.MapOffsetY = y - (int)(yDisp * m_oMap.MapScale);

            // todo : ograniczenie na wartosci Center

            m_oMap.FireViewChangedEvent();
            m_oMap.Layers.Invalidate();
        }

        [Browsable(true)]
        public ToolConst CurrentTool
        {
            get
            {
                return m_eTool;
            }
            set
            {
                currentToolProcessor = null;
                m_eTool = value;

                if (m_eTool == ToolConst.PanTool)
                {
                    /*
                    // source: http://forums.msdn.microsoft.com/en-US/csharpgeneral/thread/1c268a5f-e159-4890-bb03-19d4fb268c73/
                    Assembly asm = Assembly.GetExecutingAssembly();
                    using (Stream resStream = asm.GetManifestResourceStream("hiMapNet.Resources.cur00012.cur"))
                    {
                        this.Cursor = new Cursor(resStream);
                    }*/
                    this.Cursor = Cursors.Hand;
                }
                else if (m_eTool == ToolConst.InfoTool)
                {
                    this.Cursor = Cursors.Cross;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
        }






        private void MapControl_Resize(object sender, EventArgs e)
        {
            bt_bg = null;
            g_bg = null;

            bt_AnimationLayer = null;
            g_AnimationLayer = null;

            if (resizeScale == ResizeScaleConst.NoChange)
            {
                // sta³e scale
                // nop
            }
            else if (resizeScale == ResizeScaleConst.KeepCC)
            {
                // zoom behaviour
                // sta³y zoom?
                double zoomPrevious = (double)controlSize.Width / m_oMap.MapScale;
                double newMapScale = Width / zoomPrevious;

                // offset behaviour
                // constant point:
                int x = Width / 2;
                int y = Height / 2;

                double xDisp = (controlSize.Width / 2 - m_oMap.MapOffsetX) / m_oMap.MapScale;
                double yDisp = (controlSize.Height / 2 - m_oMap.MapOffsetY) / m_oMap.MapScale;

                m_oMap.MapOffsetX = x - (int)(xDisp * newMapScale);
                m_oMap.MapOffsetY = y - (int)(yDisp * newMapScale);
                m_oMap.MapScale = newMapScale;
            }
            else if (resizeScale == ResizeScaleConst.KeepTL)
            {
                // zoom behaviour
                // sta³y zoom?
                double zoomPrevious = (double)controlSize.Width / m_oMap.MapScale;
                double newMapScale = Width / zoomPrevious;

                // offset behaviour
                // constant point:
                int x = 0;
                int y = 0;

                double xDisp = (0 - m_oMap.MapOffsetX) / m_oMap.MapScale;
                double yDisp = (0 - m_oMap.MapOffsetY) / m_oMap.MapScale;

                m_oMap.MapOffsetX = x - (int)(xDisp * newMapScale);
                m_oMap.MapOffsetY = y - (int)(yDisp * newMapScale);
                m_oMap.MapScale = newMapScale;
            }
            else if (resizeScale == ResizeScaleConst.KeepBL)
            {
                throw new NotImplementedException();
            }
            else
            {
                Debug.Assert(false);
            }

            controlSize = new Size(Width, Height);
            m_oMap.Layers.Invalidate();
        }

        [Browsable(false)]
        public double Zoom
        {
            get
            {
                return (double)Width / m_oMap.MapScale;
            }
        }

        [Browsable(false)]
        public double CenterX
        {
            get
            {
                return (double)(Width / 2 - m_oMap.MapOffsetX) / m_oMap.MapScale;
            }
        }

        [Browsable(false)]
        public double CenterY
        {
            get
            {
                return (double)(Width / 2 - m_oMap.MapOffsetY) / m_oMap.MapScale;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCenter"></param>
        /// <param name="yCenter"></param>
        /// <param name="zoom">liczba metrów widocznych w poziomie na ekranie</param>
        /// <param name="viewRect"></param>
        public void SetCenterZoom(double xCenter, double yCenter, double zoom, Rectangle viewRect)
        {
            // primary position
            m_oMap.MapScale = (viewRect.Width / zoom);
            m_oMap.MapOffsetX = (int)((double)viewRect.Width / 2.0 - xCenter * m_oMap.MapScale);
            m_oMap.MapOffsetY = (int)((double)viewRect.Height / 2.0 - yCenter * m_oMap.MapScale);

            m_oMap.Layers.Invalidate();
        }

        private void MapControl_Load(object sender, EventArgs e)
        {
            Globals.Instance.MapControl = this;
        }

        public void InvalidateMap()
        {
            // TODO : refactor: remove .....
            m_oMap.Layers.Invalidate();
        }

        private void MapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.MouseClick(sender, e);
                }
            }
        }

        /// <summary>
        /// Fires then tool has been used (onMouseUp)
        /// </summary>
        // Summary: 
        //   ala opis
        [Browsable(true)]
        [Description("This event fires when any tool has been used."), Category("Action")]
        public event ToolUsedEventHandler MapToolUsed;

        private void MapControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.KeyPress(sender, e);
                }
            }
        }

        private void MapControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.KeyDown(sender, e);
                }
            }
        }

        private void MapControl_MouseHover(object sender, EventArgs e)
        {
            if (m_eTool == ToolConst.CustomToolProcessor)
            {
                if (currentToolProcessor != null)
                {
                    currentToolProcessor.MouseHover(sender, e);
                }
            }
        }
    }

    // event tool-used

    public class ToolUsedEventArgs : EventArgs
    {
        public ToolUsedEventArgs(Rectangle activeRectangle)
        {
            this.activeRectangle = activeRectangle;
        }

        Rectangle activeRectangle;
        public Rectangle ActiveRectangle
        {
            get { return activeRectangle; }
        }
    }

    public delegate void ToolUsedEventHandler(object sender, ToolUsedEventArgs e);
}
