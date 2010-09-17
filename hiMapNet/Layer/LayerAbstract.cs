using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace hiMapNet
{
    public abstract class LayerAbstract
    {
        Layers parent = null;

        public Layers Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        protected CoordSys layerCoordSys;

        public CoordSys LayerCoordSys
        {
            get { return layerCoordSys; }
            set 
            { 
                layerCoordSys = value;
                Invalidate();
            }
        }

        protected bool bDirty = false;
        public void Invalidate()
        {
            bDirty = true;
            MapControl.Globals.Instance.MapControl.Invalidate();
        }

        public bool IsDirty()
        {
            return bDirty;
        }


        public virtual void Draw(Graphics g, Rectangle Rect, CoordConverter oCC1)
        {
            bDirty = false;
        }
    }
}
