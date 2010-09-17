using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeSlideControl;

namespace gpxEditor
{
    class GPXViewTimeSlide : IGPXView
    {
        TimeSlide timeSlide = null;

        public GPXViewTimeSlide(TimeSlide timeSlide)
        {
            this.timeSlide = timeSlide;
            this.timeSlide.MarkerChanging += new EventHandler(timeSlide_MarkerChanging);
        }

        void timeSlide_MarkerChanging(object sender, EventArgs e)
        {
            // find new wpt
            DateTime selectionTime = timeSlide.ValueMarker;
            GpxWpt wpt = findNearestWpt(selectionTime.ToUniversalTime());

            // fire ChangedLocation
            if (wpt != null)
            {
                gpxFile.location = wpt;
                if (ChangedLocation != null) ChangedLocation(this, new EventArgs());
            }
        }

        GpxWpt findNearestWpt(DateTime timeToFind)
        {
            TimeSpan minDelta = new TimeSpan (365, 0,0,0);
            GpxWpt minWpt = null;

            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        if (wpt.time > DateTime.MinValue)
                        {
                            TimeSpan delta = timeToFind - wpt.time;
                            if (delta.Ticks < 0) delta = new TimeSpan(-delta.Ticks);
                            if (delta < minDelta)
                            {
                                minDelta = delta;
                                minWpt = wpt;
                            }
                        }
                    }
                }
            }
            return minWpt;
        }


        #region IGPXView Members

        GPXFile gpxFile = null;
        public void Bind(GPXFile gpxFile)
        {
            this.gpxFile = gpxFile;
        }

        public void Repaint()
        {
            timeSlide.PlotValuesBlack.Clear();
            timeSlide.PlotValuesRed.Clear();

            DateTime minTimeInWpts = DateTime.MinValue;

            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        if (wpt.time > DateTime.MinValue)
                        {
                            DateTime localTime = wpt.time.ToLocalTime();
                            if (wpt.selected == false)
                            {
                                timeSlide.PlotValuesBlack.Add(new KeyValuePair<DateTime, double>(localTime, wpt.ele / 100.0));
                            }
                            else
                            {
                                timeSlide.PlotValuesRed.Add(new KeyValuePair<DateTime, double>(localTime, wpt.ele / 100.0));
                            }

                            if (minTimeInWpts == DateTime.MinValue)
                            {
                                minTimeInWpts = wpt.time;
                            }
                        }
                    }
                }
            }
            if (timeSlide.ValueStart == DateTime.MinValue)
            {
                timeSlide.ValueStart = minTimeInWpts.ToLocalTime();
            }
            timeSlide.Invalidate();
        }

        public void NewLocation()
        {
            GpxWpt wpt = gpxFile.location;
            if (wpt != null)
            {
                if (wpt.time != DateTime.MinValue)
                {
                    DateTime time = wpt.time.ToLocalTime();
                    timeSlide.ValueMarker = time;
                }
            }

            // scroll ruler to show the marker


        }

        public event EventHandler ChangedData;
        public event EventHandler ChangedLocation;

        #endregion
    }
}
