using System;
using System.Collections.Generic;
using System.Text;

namespace gpxEditor
{
    public interface IGPXView
    {
        void Bind(GPXFile gpxFile);

        void Repaint();  // react to new data
        event EventHandler ChangedData;  // report new data

        void NewLocation(); // react to new wpt selected
        event EventHandler ChangedLocation;  // report selection of new point
    }
}
