using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace hiMapNet
{
    internal class UndoElementPrimitive
    {
        /// <summary>
        /// type of operation that was done (and might have to be undone)
        /// </summary>
        UndoElementType operationType = UndoElementType.Undetermined;

        // delete polyline point, part of polyline feature
        int featureIndex;
        Feature feature = null;
        int partIndex;
        Part part = null;
        int pointIndex;
        DPoint point = null; // pointer to actual point on layer (possibly attached)

        // move polyline point
        DPoint originalPoint = null;  // detached copy of original position
        Feature originalFeature = null;


        /// <summary>
        /// record feature delete
        /// </summary>
        /// <param name="featrueIdx">featureID</param>
        /// <param name="feature">what was deleted</param>
        public void featureDeleted(int featrueIdx, Feature feature)
        {
            Debug.Assert(feature != null);

            operationType = UndoElementType.DeleteFeature;
            this.featureIndex = featrueIdx;
            this.feature = feature;
        }

        public void partDeleted(int featrueIdx, int partIdx, Part part)
        {
            Debug.Assert(part != null);

            operationType = UndoElementType.DeletePolylinePart;
            this.featureIndex = featrueIdx;
            this.partIndex = partIdx;
            this.part = part;
        }

        public void pointDeleted(int featrueIdx, int partIdx, int pointIdx, DPoint point)
        {
            Debug.Assert(point != null);

            operationType = UndoElementType.DeletePolylinePoint;
            this.featureIndex = featrueIdx;
            this.partIndex = partIdx;
            this.pointIndex = pointIdx;
            this.point = point;
        }

        public void pointMoved(DPoint actualPointMoved, DPoint originalPointLocation)
        {
            operationType = UndoElementType.MovePolylinePoint;
            this.point = actualPointMoved;
            this.originalPoint = originalPointLocation;
        }

        public void featureMoved(SymbolFeature actualFeatureMoved, SymbolFeature originalFeatureLocation)
        {
            operationType = UndoElementType.MoveFeature;
            this.feature = actualFeatureMoved;
            this.originalFeature = originalFeatureLocation;
        }

        public void featureAdded(PolylineFeature feature)
        {
            operationType = UndoElementType.InsertFeature;
            this.featureIndex = feature.FeatureID;
        }

        public void play(FeaturesContainer featuresContainer)
        {
            if (operationType == UndoElementType.DeletePolylinePoint)
            {
                Debug.Assert(point != null);
                PolylineFeature polyline = (PolylineFeature)featuresContainer.getFeature(featureIndex);
                polyline.m_oParts[partIndex].Points.Insert(pointIndex, point);
                point.Selected = false;
                featuresContainer.boundsDirty();
            }
            else if (operationType == UndoElementType.DeletePolylinePart)
            {
                Debug.Assert(part != null);
                PolylineFeature polyline = (PolylineFeature)featuresContainer.getFeature(featureIndex);
                polyline.m_oParts.Insert(partIndex, part);
                featuresContainer.boundsDirty();
            }
            else if (operationType == UndoElementType.DeleteFeature)
            {
                Debug.Assert(feature != null);
                featuresContainer.insertFeature(featureIndex, feature);
                feature.Selected = false;
                featuresContainer.boundsDirty();
            }
            else if (operationType == UndoElementType.MovePolylinePoint)
            {
                Debug.Assert(point != null);

                point.X = originalPoint.X;
                point.Y = originalPoint.Y;
                point.Selected = false;
                featuresContainer.boundsDirty();
                // TODO: use featuresContainer to modify contents
            }
            else if (operationType == UndoElementType.MoveFeature)
            {
                Debug.Assert(feature != null);

                // undo feature move
                if (feature is SymbolFeature)
                {
                    SymbolFeature sf = (SymbolFeature)feature;
                    Debug.Assert(originalFeature is SymbolFeature);
                    sf.x = ((SymbolFeature)originalFeature).x;
                    sf.y = ((SymbolFeature)originalFeature).y;
                }
            }
            else if (operationType == UndoElementType.InsertFeature)
            {
                featuresContainer.removeFeature(featureIndex);
            }
            else throw new Exception("Internal error.");
        }
    }
}
