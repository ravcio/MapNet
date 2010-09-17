using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace hiMapNet
{
    public class Layers : IList<LayerAbstract> 
    {
        private List<LayerAbstract> m_oLayers = new List<LayerAbstract>();

        LayerAbstract animationLayer = null;

        public LayerAbstract AnimationLayer
        {
            get { return animationLayer; }
            set { animationLayer = value; }
        }

        #region IList<Layer> Members

        public int IndexOf(LayerAbstract item)
        {
            return m_oLayers.IndexOf(item);
        }

        public void Insert(int index, LayerAbstract item)
        {
            m_oLayers.Insert(index, item);
            item.Parent = this;
        }

        public void RemoveAt(int index)
        {
            m_oLayers.RemoveAt(index);
        }

        public LayerAbstract this[int index]
        {
            get
            {
                return m_oLayers[index];
            }
            set
            {
                m_oLayers[index] = value;
            }
        }

        #endregion

        #region ICollection<ILayer> Members

        public void Add(LayerAbstract item)
        {
            m_oLayers.Add(item);
            item.Parent = this;
        }

        public void Clear()
        {
            m_oLayers.Clear();
        }

        public bool Contains(LayerAbstract item)
        {
            return m_oLayers.Contains(item);
        }

        public void CopyTo(LayerAbstract[] array, int arrayIndex)
        {
            throw new NotImplementedException("operation not supported");
        }

        public int Count
        {
            get { return m_oLayers.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(LayerAbstract item)
        {
            return m_oLayers.Remove(item);
        }

        #endregion

        #region IEnumerable<LayerAbstract> Members

        public IEnumerator<LayerAbstract> GetEnumerator()
        {
            return m_oLayers.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_oLayers.GetEnumerator();
        }

        #endregion


        internal bool IsDirty()
        {
            foreach (LayerAbstract layer in m_oLayers)
            {
                if (layer != AnimationLayer)
                {
                    if (layer.IsDirty()) return true;
                }
            }
            return false;
        }

        public void Invalidate()
        {
            foreach (LayerAbstract layer in m_oLayers)
            {
                layer.Invalidate();
            }
        }
    }

}
