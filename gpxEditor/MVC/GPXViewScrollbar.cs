using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gpxEditor
{
    class GPXViewScrollbar : IGPXView
    {
        HScrollBar scrollbar = null; // TODO: initalization list? how?

        bool ignoreScrollbarEvent = false;

        public GPXViewScrollbar(HScrollBar scrollbar)
        {
            this.scrollbar = scrollbar;

            this.scrollbar.ValueChanged += new EventHandler(scrollbar_ValueChanged);

        }

        void scrollbar_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreScrollbarEvent) return;
            // send new location event
            int idx = scrollbar.Value;

            // navigate to a given point
            GpxWpt wpt = navigateToWpt(idx);

            // notify about new focus
            gpxFile.location = wpt;
            if (ChangedLocation != null)
            {
                ChangedLocation(this, new EventArgs());
            }

        }

        GpxWpt navigateToWpt(int idx)
        {
            int count = 0;
            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        count++;
                        if (count == idx)
                        {
                            return wpt;
                        }
                    }
                }
            }
            return null;
        }

        int navigateToIdx(GpxWpt wptFind)
        {
            int count = 0;
            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        count++;
                        if (wpt == wptFind)
                        {
                            return count;
                        }
                    }
                }
            }
            return -1;
        }


        #region IGPXView Members

        GPXFile gpxFile = null;
        public void Bind(GPXFile gpxFile)
        {
            this.gpxFile = gpxFile;
            InitScroll();
        }

        public void Repaint()
        {
            InitScroll();
        }

        public void NewLocation()
        {
            GpxWpt wpt = gpxFile.location;
            if (wpt != null)
            {
                int idx = navigateToIdx(wpt);
                if (idx >= 0)
                {
                    ignoreScrollbarEvent = true;
                    scrollbar.Value = idx;  // this generates unwanted event
                    ignoreScrollbarEvent = false;
                }
            }
        }

        public event EventHandler ChangedData;
        public event EventHandler ChangedLocation;

        #endregion

        void InitScroll()
        {
            // count the number of wpts
            int count = 0;
            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        count++;
                    }
                }
            }

            scrollbar.Minimum = 0;
            scrollbar.Maximum = count;
        }

    }
}
