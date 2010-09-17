using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hiMapNet
{
    enum UndoElementType
    {
        Undetermined,
        DeleteFeature,
        DeletePolylinePart,
        DeletePolylinePoint,
        InsertFeature,
        MovePolylinePoint,
        MoveFeature
    }
}
