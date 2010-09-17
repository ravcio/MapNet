using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gpxEditor
{
    public class GPXTrkSeg
    {
        public GPXTrk parent;
        public bool selected;

        public List<GpxWpt> wpts = new List<GpxWpt>();
    }
}
