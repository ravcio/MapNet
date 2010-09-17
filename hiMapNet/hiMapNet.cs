using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    class hiMapNet
    {
        List<LayerVectors> oLayers = new List<LayerVectors>();


        internal LayerVectors AddLayer()
        {
            LayerVectors oLayer = new LayerVectors();
            oLayers.Add(oLayer);
            return oLayer;
        }
    }
}
