using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;



namespace hiMapNet
{
    public abstract class MapTool
    {
        // mouse drag tool
        protected Point m_oMouseStart;
        protected Point m_oMouseCurrent;

        protected System.Drawing.Rectangle ActiveRectangle; // m_oMouseCurrent-m_oMouseStart normalized

        // drag shape
        protected MapControl.ToolDragShapeConst m_eToolDragShape;

        public MapControl.ToolDragShapeConst ToolDragShape
        {
            get { return m_eToolDragShape; }
            set { m_eToolDragShape = value; }
        }
        private bool m_bDrawSelection = false; // true = arrow/selection tool is used at the moment


        protected Map map;
        public void Init(Map map)
        {
            this.map = map;
        }

        public virtual void PaintPost(Graphics g) 
        {
            if (m_eToolDragShape == MapControl.ToolDragShapeConst.Rectangle)
            {
                if (m_bDrawSelection == true)
                {
                    // draw selection rectangle on top
                    Pen p = new Pen(Color.Black, 1);
                    float[] f = { 2, 2 };
                    p.DashPattern = f;

                    g.DrawLine(p, m_oMouseStart.X, m_oMouseStart.Y, m_oMouseStart.X, m_oMouseCurrent.Y);
                    g.DrawLine(p, m_oMouseStart.X, m_oMouseStart.Y, m_oMouseCurrent.X, m_oMouseStart.Y);
                    g.DrawLine(p, m_oMouseCurrent.X, m_oMouseStart.Y, m_oMouseCurrent.X, m_oMouseCurrent.Y);
                    g.DrawLine(p, m_oMouseStart.X, m_oMouseCurrent.Y, m_oMouseCurrent.X, m_oMouseCurrent.Y);

                    p.Dispose();
                }
            }
        }

        public virtual void MouseDown(object sender, MouseEventArgs e)
        {
            m_oMouseStart = new Point(e.X, e.Y);
            m_oMouseCurrent = m_oMouseStart;

            m_bDrawSelection = true;
        }

        public virtual void MouseMove(object sender, MouseEventArgs e)
        {
            m_oMouseCurrent = new Point(e.X, e.Y);

            if (m_eToolDragShape == MapControl.ToolDragShapeConst.Rectangle)
            {
                if (m_bDrawSelection) MapControl.Globals.Instance.MapControl.Invalidate();
            }

            {
                // recalculate new activeRectangle
                int x1 = Math.Min(m_oMouseStart.X, m_oMouseCurrent.X);
                int y1 = Math.Min(m_oMouseStart.Y, m_oMouseCurrent.Y);
                int x2 = Math.Max(m_oMouseStart.X, m_oMouseCurrent.X);
                int y2 = Math.Max(m_oMouseStart.Y, m_oMouseCurrent.Y);
                ActiveRectangle = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);

                MapControl.Globals.Instance.MapControl.Invalidate();
            }
        }


        public virtual void MouseUp(object sender, MouseEventArgs e)
        {
            m_bDrawSelection = false;

            {
                // recalculate new activeRectangle
                int x1 = Math.Min(m_oMouseStart.X, m_oMouseCurrent.X);
                int y1 = Math.Min(m_oMouseStart.Y, m_oMouseCurrent.Y);
                int x2 = Math.Max(m_oMouseStart.X, m_oMouseCurrent.X);
                int y2 = Math.Max(m_oMouseStart.Y, m_oMouseCurrent.Y);
                ActiveRectangle = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
                
                MapControl.Globals.Instance.MapControl.Invalidate();
            }
        }

        public virtual void MouseWheel(object sender, MouseEventArgs e) {  }

        public virtual void MouseClick(object sender, MouseEventArgs e) {  }

        public virtual void KeyPress(object sender, KeyPressEventArgs e) { }

        public virtual void KeyDown(object sender, KeyEventArgs e) {  }

        public virtual void MouseHover(object sender, EventArgs e) { }
    }
}
