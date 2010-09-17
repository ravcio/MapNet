using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace hiMapNet
{
    /// <summary>
    /// Map is stateless object 
    /// Map object is used as Layer holder. It can be used to render map
    /// when no Control is needed by application.
    /// </summary>
    public class Map
    {
        LayerVectors insertionLayer = null;

        public LayerVectors InsertionLayer
        {
            get { return insertionLayer; }
            set { insertionLayer = value; }
        }

        // map primary position
        double mapScale = 1;

        public double MapScale
        {
            get { return mapScale; }
            set { mapScale = value; }
        }
        int mapOffsetX = 0;
        int mapOffsetY = 0;

        public int MapOffsetX
        {
            get { return mapOffsetX; }
            set { mapOffsetX = value; }
        }
        public int MapOffsetY
        {
            get { return mapOffsetY; }
            set { mapOffsetY = value; }
        }

        // map projection
        AffineTransform atPanZoom = new AffineTransform();
        CoordSys displayCoordSys;

        public CoordSys DisplayCoordSys
        {
            get { return displayCoordSys; }
            set { displayCoordSys = value; }
        }
        private CoordSys numericCoordSys;

        public CoordSys NumericCoordSys
        {
            get { return numericCoordSys; }
            set { numericCoordSys = value; }
        }

        // events
        public event EventHandler ViewChangedEvent;
        public void FireViewChangedEvent()
        {
            if (ViewChangedEvent != null) ViewChangedEvent(this, new EventArgs());
        }

        Layers m_oLayers = new Layers();
//        bool m_bDirty = true;

        // public no parameter constructor (e.g. for WEB usage)
        public Map()
        {
        }

        private Control m_oParentControl = null;
        public Map(Control oParentControl)
        {
            m_oParentControl = oParentControl;

            DisplayCoordSys = new CoordSys(CoordSysType.Mercator, new Datum(DatumID.WGS84), new AffineTransform());
        }

        public Layers Layers
        {
            get
            {
                return m_oLayers;
            }
        }

        public int Bounds
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        // 
        internal void Draw(Graphics g, Rectangle Rect)
        {
            for (int i = 0; i < m_oLayers.Count; i++)
            {
                LayerAbstract layer = m_oLayers[i];
                if (layer != m_oLayers.AnimationLayer)
                {
                    DrawLayer(m_oLayers[i], g, Rect);
                }
            }
        }

        internal void DrawAnimationLayer(Graphics g, Rectangle Rect)
        {
            if (m_oLayers.AnimationLayer == null) return;
            DrawLayer(m_oLayers.AnimationLayer, g, Rect);
        }

        void DrawLayer(LayerAbstract oL, Graphics g, Rectangle Rect)
        {
            CoordSys layerCoordsys = oL.LayerCoordSys;

            CoordConverter oCC = new CoordConverter();
            oCC.Init(layerCoordsys, DisplayCoordSys);

            // this atPan converts DisplayCoordSys into Screen CoordSys[px]
            // DisplayCoordSys has Y axis up (unless its AT does not change it)
            // Screen Y axis is down
            AffineTransform atPan = new AffineTransform();
            atPan.OffsetInPlace((double)mapOffsetX, (double)mapOffsetY);
            atPan.MultiplyInPlace(mapScale, -mapScale);

            // add screen scale and offset transformation
            oCC.atMaster = oCC.atMaster.Compose(atPan);

            oL.Draw(g, Rect, oCC);
        }




/*        public void SetDirty()
        {
            m_bDirty = true;
        }*/

        public bool IsDirty()
        {
            return m_oLayers.IsDirty();
        }

        /*
        public bool IsDirty
        {
            get
            {

                //if (m_dZoom != m_dZoom_last) return true;
                //if (m_oCenter != m_oCenter_last) return true;
                if (m_bDirty) return true;
                return false;
            }
        }*/

        public DisplayTransform DisplayTransform
        {
            get
            {
                AffineTransform atPan = new AffineTransform();
                atPan.OffsetInPlace((double)mapOffsetX, (double)mapOffsetY);
                atPan.MultiplyInPlace(mapScale, -mapScale);
                return new DisplayTransform(atPan);
            }
        }
    }
}
