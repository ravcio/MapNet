using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gpxEditor
{
    /// <summary>
    /// Element przechowywany w pojemniki cKML
    /// </summary>
    public class GpxWpt
    {
        public GPXTrkSeg parent;

        // id punktu
        public bool selected;

        // pozycja
        public double lon;
        public double lat;


        public double ele;
        public DateTime time;

        public string name;
        public string cmt;
        public string desc;


        // more properties are missing...


        // dodatkowe informacje (speed, heading, leg distance, track distance, leg time, 
        // hdop, nsat, name)
        public double speed;

        public GpxWpt(double lon, double lat, double ele, DateTime time, double speed)
        {
            this.lon = lon;
            this.lat = lat;
            this.ele = ele;
            this.time = time;
            this.speed = speed;
        }

        public GpxWpt(GpxWpt oWP)
        {
            lon = oWP.lon;
            lat = oWP.lat;
            ele = oWP.ele;
            time = oWP.time;
            speed = oWP.speed;
        }


    }
}
