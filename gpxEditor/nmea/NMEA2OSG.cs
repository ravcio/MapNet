/***********************************************************************
*                               NMEA2OSG                               *
*                                                                      * 
*  A class to generate Ordinance Survey Grid References from           *
*  Lat, Lon and elipsoid height from a GPS based on the WGS84 elipsoid *
*  Author: Alex Etchells                                               *
*  March 2006                                                          *
*                                                                      * 
*        calculations were derived (i.e. copied) from the work of      *
*               Roger Muggleton (http://www.carabus.co.uk/)            *
***********************************************************************/


using System;
using System.Collections.Generic;
using System.Text;

public class NMEA2OSG
{
    private static double deg2rad = Math.PI / 180;
    private static double rad2deg = 180.0 / Math.PI;
    public double deciLat;
    public double deciLon;
       
   #region Delegates
    public delegate void NorthingEastingReceivedEventHandler(double northing,double easting);
    public delegate void NatGridRefReceivedEventHandler(string ngr);
   #endregion

   #region Events
    public event NorthingEastingReceivedEventHandler NorthingEastingReceived;
    public event NatGridRefReceivedEventHandler NatGridRefReceived;
   #endregion

    
    // Processes WGS84 lat and lon in NMEA form 
    // 52°09.1461"N         002°33.3717"W
    public bool ParseNMEA(string Nlat, string Nlon, double height)
    {
     //grab the bit up to the °
     deciLat = Convert.ToDouble(Nlat.Substring(0, Nlat.IndexOf("°")));
     deciLon = Convert.ToDouble(Nlon.Substring(0, Nlon.IndexOf("°")));
     
     //remove that bit from the string now we've used it and the ° symbol
     Nlat = Nlat.Substring(Nlat.IndexOf("°") + 1);
     Nlon = Nlon.Substring(Nlon.IndexOf("°") + 1);
     
     //grab the bit up to the " - divide by 60 to convert to degrees and add it to our double value
     deciLat += (Convert.ToDouble(Nlat.Substring(0, Nlat.IndexOf("\""))))/60;
     deciLon += (Convert.ToDouble(Nlon.Substring(0, Nlat.IndexOf("\""))))/60;
     
     //ok remove that now and just leave the compass direction
     Nlat = Nlat.Substring(Nlat.IndexOf("\"") + 1);
     Nlon = Nlon.Substring(Nlon.IndexOf("\"") + 1);
  
     // check for negative directions
     if (Nlat == "S") deciLat = 0 - deciLat;
     if (Nlon == "W") deciLon = 0 - deciLon;
     
     //now we can parse them
     return Transform(deciLat, deciLon, height);
    }
    
    // Processes WGS84 lat and lon in decimal form with S and W as -ve
    public bool Transform(double WGlat, double WGlon, double height)
    {
     //first off convert to radians
     double radWGlat = WGlat * deg2rad;
     double radWGlon = WGlon * deg2rad;

     /* these calculations were derived from the work of
      * Roger Muggleton (http://www.carabus.co.uk/) */

     /* quoting Roger Muggleton :-
      * There are many ways to convert data from one system to another, the most accurate 
      * being the most complex! For this example I shall use a 7 parameter Helmert 
      * transformation.
      * The process is in three parts: 
      * (1) Convert latitude and longitude to Cartesian coordinates (these also include height 
      * data, and have three parameters, X, Y and Z). 
      * (2) Transform to the new system by applying the 7 parameters and using a little maths.
      * (3) Convert back to latitude and longitude.
      * For the example we shall transform a GRS80 location to Airy, e.g. a GPS reading to 
      * the Airy spheroid.
      * The following code converts latitude and longitude to Cartesian coordinates. The 
      * input parameters are: WGS84 latitude and longitude, axis is the GRS80/WGS84 major 
      * axis in metres, ecc is the eccentricity, and height is the height above the 
      *  ellipsoid.
      *  v = axis / (Math.sqrt (1 - ecc * (Math.pow (Math.sin(lat), 2))));
      *  x = (v + height) * Math.cos(lat) * Math.cos(lon);
      * y = (v + height) * Math.cos(lat) * Math.sin(lon);
      * z = ((1 - ecc) * v + height) * Math.sin(lat);
      * The transformation requires the 7 parameters: xp, yp and zp correct the coordinate 
      * origin, xr, yr and zr correct the orientation of the axes, and sf deals with the 
      * changing scale factors. */

     //these are the values for WGS86(GRS80) to OSGB36(Airy)
     double a = 6378137;              // WGS84_AXIS
     double e = 0.00669438037928458;  // WGS84_ECCENTRIC
     double h = height;               // height above datum  (from GPS GGA sentence)
     double a2 = 6377563.396;         //OSGB_AXIS
     double e2 = 0.0066705397616;     // OSGB_ECCENTRIC 
     double xp = -446.448;
     double yp = 125.157;
     double zp = -542.06;
     double xr = -0.1502;
     double yr = -0.247;
     double zr = -0.8421;
     double s = 20.4894;

     // convert to cartesian; lat, lon are radians
     double sf = s * 0.000001;
     double v = a / (Math.Sqrt(1 - (e * (Math.Sin(radWGlat) * Math.Sin(radWGlat)))));
     double x = (v + h) * Math.Cos(radWGlat) * Math.Cos(radWGlon);
     double y = (v + h) * Math.Cos(radWGlat) * Math.Sin(radWGlon);
     double z = ((1 - e) * v + h) * Math.Sin(radWGlat);

     // transform cartesian
     double xrot = (xr / 3600) * deg2rad;
     double yrot = (yr / 3600) * deg2rad;
     double zrot = (zr / 3600) * deg2rad;
     double hx = x + (x * sf) - (y * zrot) + (z * yrot) + xp;
     double hy = (x * zrot) + y + (y * sf) - (z * xrot) + yp;
     double hz = (-1 * x * yrot) + (y * xrot) + z + (z * sf) + zp;

     // Convert back to lat, lon
     double newLon = Math.Atan(hy / hx);
     double p = Math.Sqrt((hx * hx) + (hy * hy));
     double newLat = Math.Atan(hz / (p * (1 - e2)));
     v = a2 / (Math.Sqrt(1 - e2 * (Math.Sin(newLat) * Math.Sin(newLat))));
     double errvalue = 1.0;
     double lat0 = 0;
     while (errvalue > 0.001)
        {
         lat0 = Math.Atan((hz + e2 * v * Math.Sin(newLat)) / p);
         errvalue = Math.Abs(lat0 - newLat);
         newLat = lat0;
        }
   
     //convert back to degrees
     newLat = newLat * rad2deg;
     newLon = newLon * rad2deg;

     //convert lat and lon (OSGB36)  to OS 6 figure northing and easting
     LLtoNE(newLat, newLon);
           
     return true;
    }

    //converts lat and lon (OSGB36)  to OS 6 figure northing and easting
    private void LLtoNE(double lat, double lon)
    {
     double phi = lat * deg2rad;          // convert latitude to radians
     double lam = lon * deg2rad;          // convert longitude to radians
     double a = 6377563.396;              // OSGB semi-major axis
     double b = 6356256.91;               // OSGB semi-minor axis
     double e0 = 400000;                  // easting of false origin
     double n0 = -100000;                 // northing of false origin
     double f0 = 0.9996012717;            // OSGB scale factor on central meridian
     double e2 = 0.0066705397616;         // OSGB eccentricity squared
     double lam0 = -0.034906585039886591; // OSGB false east
     double phi0 = 0.85521133347722145;   // OSGB false north
     double af0 = a * f0;
     double bf0 = b * f0;

     // easting
     double slat2 = Math.Sin(phi) * Math.Sin(phi);
     double nu = af0 / (Math.Sqrt(1 - (e2 * (slat2))));
     double rho = (nu * (1 - e2)) / (1 - (e2 * slat2));
     double eta2 = (nu / rho) - 1;
     double p = lam - lam0;
     double IV = nu * Math.Cos(phi);
     double clat3 = Math.Pow(Math.Cos(phi), 3);
     double tlat2 = Math.Tan(phi) * Math.Tan(phi);
     double V = (nu / 6) * clat3 * ((nu / rho) - tlat2);
     double clat5 = Math.Pow(Math.Cos(phi), 5);
     double tlat4 = Math.Pow(Math.Tan(phi), 4);
     double VI = (nu / 120) * clat5 * ((5 - (18 * tlat2)) + tlat4 + (14 * eta2) - (58 * tlat2 * eta2));
     double east = e0 + (p * IV) + (Math.Pow(p, 3) * V) + (Math.Pow(p, 5) * VI);

     // northing
     double n = (af0 - bf0) / (af0 + bf0);
     double M = Marc(bf0, n, phi0, phi);
     double I = M + (n0);
     double II = (nu / 2) * Math.Sin(phi) * Math.Cos(phi);
     double III = ((nu / 24) * Math.Sin(phi) * Math.Pow(Math.Cos(phi), 3)) * (5 - Math.Pow(Math.Tan(phi), 2) + (9 * eta2));
     double IIIA = ((nu / 720) * Math.Sin(phi) * clat5) * (61 - (58 * tlat2) + tlat4);
     double north = I + ((p * p) * II) + (Math.Pow(p, 4) * III) + (Math.Pow(p, 6) * IIIA);

     // make whole number values
     east = Math.Round(east);   // round to whole number
     north = Math.Round(north); // round to whole number
        
     // Notify the calling application of the change
     if (NorthingEastingReceived != null) NorthingEastingReceived(north, east);

     // convert to nat grid ref
     NE2NGR(east, north);
    }

    // a function used in LLtoNE  - that's all I know about it
    private double Marc(double bf0, double n, double phi0, double phi)
    {
     return bf0 * (((1 + n + ((5 / 4) * (n * n)) + ((5 / 4) * (n * n * n))) * (phi - phi0))
        - (((3 * n) + (3 * (n * n)) + ((21 / 8) * (n * n * n))) * (Math.Sin(phi - phi0)) * (Math.Cos(phi + phi0)))
        + ((((15 / 8) * (n * n)) + ((15 / 8) * (n * n * n))) * (Math.Sin(2 * (phi - phi0))) * (Math.Cos(2 * (phi + phi0))))
        - (((35 / 24) * (n * n * n)) * (Math.Sin(3 * (phi - phi0))) * (Math.Cos(3 * (phi + phi0)))));
    }

    //convert 12 (6e & 6n) figure numeric to letter and number grid system
    private void NE2NGR(double east, double north)
    {
     double eX = east / 500000;
     double nX = north / 500000;
     double tmp = Math.Floor(eX) - 5.0 * Math.Floor(nX) + 17.0;  //Math.Floor Returns the largest integer less than or equal to the specified number.
     nX = 5 * (nX - Math.Floor(nX));
     eX = 20 - 5.0 * Math.Floor(nX) + Math.Floor(5.0 * (eX - Math.Floor(eX)));
     if (eX > 7.5) eX = eX + 1;
     if (tmp > 7.5) tmp = tmp + 1;
     string eing = Convert.ToString(east);
     string ning = Convert.ToString(north);
     int lnth = eing.Length;
     eing = eing.Substring(lnth - 5);
     lnth = ning.Length;
     ning = ning.Substring(lnth - 5);
     string ngr = Convert.ToString((char)(tmp + 65)) + Convert.ToString((char)(eX + 65)) + " " + eing + " " + ning;
     
     // Notify the calling application of the change
     if (NatGridRefReceived != null) NatGridRefReceived(ngr);
    }

}

