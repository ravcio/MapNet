using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

namespace hiMapNet
{
    abstract public class Feature
    {
        private object tag = null;

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        protected Style m_oStyle;

        // bounding box in Layer Corrdsys
        protected DRect m_oMBR;

        public DRect MBR
        {
            get
            {
                return m_oMBR;
            }
            set
            {
                m_oMBR = value;
            }
        }

        /// <summary>
        /// extra margin required when calculating visibility
        /// </summary>
        /// <returns></returns>
        public virtual int pixelMargin()
        {
            return 0;
        }


        public Style Style
        {
            get
            {
                return m_oStyle;
            }
            set
            {
                m_oStyle = value;
            }
        }




        /// <summary>
        /// Refresh value of MBR
        /// </summary>
        abstract public void CalcMBR();

        private bool m_bSelected = false;

        public bool Selected
        {
            get { return m_bSelected; }
            set { m_bSelected = value; }
        }


        FeaturesContainer parentContainer = null;

        public FeaturesContainer ParentContainer
        {
            get { return parentContainer; }
        }

        internal void setParentContainer(FeaturesContainer layer)
        {
            parentContainer = layer;
        }


        int featureID = -1;

        public int FeatureID
        {
            get { return featureID; }
            set { featureID = value; }
        }

        public void setField(string name, object value)
        {
            if (parentContainer == null) throw new Exception("Feature must be attached.");

            if (parentContainer.ParentLayer is LayerVectors)
            {
                LayerVectors layer = (LayerVectors)parentContainer.ParentLayer;

                DataTable dt = layer.DataTable;
                DataRow dr = dt.Rows[featureID];
                dr[name] = value;
            }
            else
            {
                throw new Exception("Parent layer must be LayerVector.");
            }
        }

        public object getField(string name)
        {
            if (parentContainer == null) throw new Exception("Feature must be attached.");

            if (parentContainer.ParentLayer is LayerVectors)
            {
                LayerVectors layer = (LayerVectors)parentContainer.ParentLayer;

                DataTable dt = layer.DataTable;
                DataRow dr = dt.Rows[featureID];
                return dr[name];
            }
            else
            {
                throw new Exception("Parent layer must be LayerVector.");
            }
        }

        abstract public Feature clone();
    }
}
