using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace hiMapNet
{
    public class UndoElement
    {
        List<UndoElementPrimitive> undoPrimitives = new List<UndoElementPrimitive>();

        FeaturesContainer featuresContainer = null;
        public UndoElement(FeaturesContainer featuresContainer)
        {
            if (featuresContainer == null) throw new Exception("Invalid parameter");
            this.featuresContainer = featuresContainer;
        }

        public void removeFeature(int featrueIdx)
        {
            Feature feature = featuresContainer.getFeature(featrueIdx);
            UndoElementPrimitive elem = new UndoElementPrimitive();
            elem.featureDeleted(featrueIdx, feature);
            undoPrimitives.Add(elem);
            featuresContainer.removeFeature(featrueIdx);
        }
        public void removePart(int featrueIdx, int partIdx)
        {
            PolylineFeature feature = (PolylineFeature)featuresContainer.getFeature(featrueIdx);
            Part part = feature.m_oParts[partIdx];
            UndoElementPrimitive elem = new UndoElementPrimitive();
            elem.partDeleted(featrueIdx, partIdx, part);
            undoPrimitives.Add(elem);
            feature.m_oParts.RemoveAt(partIdx);
        }
        public void removePoint(int featureIdx, int partIdx, int pointIdx)
        {
            PolylineFeature feature = (PolylineFeature)featuresContainer.getFeature(featureIdx);
            DPoint point = feature.m_oParts[partIdx].Points[pointIdx];
            UndoElementPrimitive elem = new UndoElementPrimitive();
            elem.pointDeleted(featureIdx, partIdx, pointIdx, point);
            undoPrimitives.Add(elem);

            feature.m_oParts[partIdx].Points.RemoveAt(pointIdx);
        }

        public void playReverse()
        {
            for (int i = undoPrimitives.Count - 1; i >= 0; i--)
            {
                UndoElementPrimitive action = undoPrimitives[i];
                action.play(featuresContainer);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualPointMoved">points to actual point in contianer (attached)</param>
        /// <param name="originalPointLocation">clone of point (detached)</param>
        public void movePoint(DPoint actualPointMoved, DPoint originalPointLocation)
        {
            UndoElementPrimitive elem = new UndoElementPrimitive();
            elem.pointMoved(actualPointMoved, originalPointLocation);
            undoPrimitives.Add(elem);
        }

        public void moveSymbol(SymbolFeature actualFeatureMoved, SymbolFeature originalFeatureLocation)
        {
            UndoElementPrimitive elem = new UndoElementPrimitive();
            elem.featureMoved(actualFeatureMoved, originalFeatureLocation);
            undoPrimitives.Add(elem);
        }

        public void addFeature(PolylineFeature feature)
        {
            UndoElementPrimitive elem = new UndoElementPrimitive();
            elem.featureAdded(feature);
            undoPrimitives.Add(elem);
        }
    }
}
