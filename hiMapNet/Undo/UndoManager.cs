using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hiMapNet
{
    public class UndoManager
    {
        // undo sequence
        Stack<UndoElement> undoElements = new Stack<UndoElement>();
        Stack<UndoElement> redoElements = new Stack<UndoElement>();

        FeaturesContainer featuresContainer = null;

        public UndoManager(FeaturesContainer featuresContainer)
        {
            if (featuresContainer == null) throw new Exception("Invalid parameter");
            this.featuresContainer = featuresContainer;
        }

        /// <summary>
        /// remove undo element from stack and execute it
        /// </summary>
        public void playUndo()
        {
            if (undoElements.Count > 0)
            {
                UndoElement undoElement = undoElements.Pop();
                undoElement.playReverse();
                redoElements.Push(undoElement);
                MapControl.Globals.Instance.MapControl.InvalidateMap();
            }
        }


        public void playRedo()
        {
            if (redoElements.Count > 0)
            {
                UndoElement undoElement = redoElements.Pop();
                undoElement.playReverse();
                undoElements.Push(undoElement);
                MapControl.Globals.Instance.MapControl.InvalidateMap();
            }
        }


        UndoElement undoElementCurrent = null;

        public void startRecordingUndoElement()
        {
            undoElementCurrent = new UndoElement(featuresContainer);
        }

        public void stopRecordingUndoElement()
        {
            undoElements.Push(undoElementCurrent);
            redoElements.Clear();
        }

        public void recordMovePoint(DPoint point, DPoint newPoint)
        {
            undoElementCurrent.movePoint(point, new DPoint(point));

            point.X = newPoint.X;
            point.Y = newPoint.Y;
        }

        public void recordMoveFeature(SymbolFeature symbol, SymbolFeature newSymbol)
        {
            undoElementCurrent.moveSymbol(symbol,  new SymbolFeature(symbol));

            symbol.x = newSymbol.x;
            symbol.y = newSymbol.y;
        }

        public void recordRemoveFeature(Feature feature)
        {
            undoElementCurrent.removeFeature(feature.FeatureID);
            redoElements.Clear();
        }

        public void recordRemovePoint(int featureIdx, int partIdx, int pointIdx)
        {
            undoElementCurrent.removePoint(featureIdx, partIdx, pointIdx);
            redoElements.Clear();
        }

        public void recordRemovePart(int featrueIdx, int partIdx)
        {
            undoElementCurrent.removePart(featrueIdx, partIdx);
            redoElements.Clear();
        }

        public void recordAddFeature(PolylineFeature feature)
        {
            featuresContainer.addFeature(feature); // make feature attached
            undoElementCurrent.addFeature(feature);
            redoElements.Clear();
        }
    }
}
