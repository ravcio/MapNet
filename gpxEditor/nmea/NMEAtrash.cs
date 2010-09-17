using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gpxEditor.nmea
{
    class NMEAtrash
    {

        //#region serialport
        //private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    // This method will be called when there is data waiting in the port's buffer
        //    // Read all the data waiting in the buffer and pasrse it

        //    /* http://forums.microsoft.com/MSDN/ShowPost.aspx?PageIndex=2&SiteID=1&PostID=293187
        //     * You would need to use Control.Invoke() to update the GUI controls
        //     * because unlike Windows Forms events like Button.Click which are processed 
        //     * in the GUI thread, SerialPort events are processed in a non-GUI thread 
        //     * (more precisely a ThreadPool thread). 
        //     */
        //    this.Invoke(new EventHandler(HandleGPSstring));
        //}

        //private void HandleGPSstring(object s, EventArgs e)
        //{
        //    string inbuff;
        //    inbuff = comport.ReadExisting();
        //    if (inbuff != null)
        //    {
        //        if (inbuff.StartsWith("$"))
        //        {
        //            instring = inbuff;
        //        }
        //        else
        //        {
        //            instring += inbuff;
        //        }
        //        gpsString = instring.Split();
        //        foreach (string item in gpsString) GPS.Parse(item);
        //    }
        //}
        //#endregion


        private void GPS_PositionReceived(string Lat, string Lon)
        {
            /*
               double dLat, dLon;

               ParseNMEA(Lat, Lon, out dLat, out dLon);
               oCCGPS.Convert(dLon, -dLat);  // todo: jest jakiś bug w przeliczeniach

               oLayerPointer.FeaturesClear();
               // create polyline
               List<DPoint> oPoints = new List<DPoint>();
               double rr = mapControl1.Zoom / 10000000.0;
               oPoints.Add(new DPoint(dLon - rr, dLat - rr));
               oPoints.Add(new DPoint(dLon + rr, dLat - rr));
               oPoints.Add(new DPoint(dLon + rr, dLat + rr));
               oPoints.Add(new DPoint(dLon - rr, dLat + rr));
               oPoints.Add(new DPoint(dLon - rr, dLat - rr));
               oPoints.Add(new DPoint(dLon + rr, dLat + rr));
               oPoints.Add(new DPoint(dLon + rr, dLat - rr));
               oPoints.Add(new DPoint(dLon - rr, dLat + rr));
               PolylineFeature oPolyline = FeatureFactory.CreatePolyline(oPoints, new Style(Color.Red));
               oLayerPointer.FeaturesInsert(oPolyline);

               mapControl1.SetCenterZoom(oCCGPS.X, oCCGPS.Y, mapControl1.Zoom, mapControl1.Bounds);*/
        }
        /*
                // Processes WGS84 lat and lon in NMEA form 
                // 52°09.1461"N         002°33.3717"W
                public void ParseNMEA(string Nlat, string Nlon, out double dLat, out double dLon)
                {
                    dLat = 0;
                    dLon = 0;
                    try
                    {
                        //grab the bit up to the °
                        dLat = Convert.ToDouble(Nlat.Substring(0, Nlat.IndexOf("°")), CultureInfo.InvariantCulture);
                        dLon = Convert.ToDouble(Nlon.Substring(0, Nlon.IndexOf("°")), CultureInfo.InvariantCulture);

                        //remove that bit from the string now we've used it and the ° symbol
                        Nlat = Nlat.Substring(Nlat.IndexOf("°") + 1);
                        Nlon = Nlon.Substring(Nlon.IndexOf("°") + 1);

                        //grab the bit up to the " - divide by 60 to convert to degrees and add it to our double value
                        dLat += (Convert.ToDouble(Nlat.Substring(0, Nlat.IndexOf("\"")), CultureInfo.InvariantCulture)) / 60;
                        dLon += (Convert.ToDouble(Nlon.Substring(0, Nlat.IndexOf("\"")), CultureInfo.InvariantCulture)) / 60;

                        //ok remove that now and just leave the compass direction
                        Nlat = Nlat.Substring(Nlat.IndexOf("\"") + 1);
                        Nlon = Nlon.Substring(Nlon.IndexOf("\"") + 1);

                        // check for negative directions
                        if (Nlat == "S") dLat = 0 - dLat;
                        if (Nlon == "W") dLon = 0 - dLon;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //now we can parse them
                    return;
                }
                */
    }
}
