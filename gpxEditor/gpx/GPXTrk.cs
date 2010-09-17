using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gpxEditor
{
    public class GPXTrk
    {
        public GPXFile parent;
        public bool selected;

        public string name = "";
        public string cmt = "";
        public string desc = "";

        public List<GPXTrkSeg> trkSeg = new List<GPXTrkSeg>();
    }
}
