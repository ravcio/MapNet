﻿
namespace GMap.NET.Projections
{
   using System;

   class MercatorProjectionYandex : PureProjection
   {
      const double MinLatitude = -85.05112878;
      const double MaxLatitude = 85.05112878;
      const double MinLongitude = -177;
      const double MaxLongitude = 177;
      const double RAD_DEG = 180 / Math.PI;
      const double DEG_RAD = Math.PI / 180;
      const double MathPiDiv4 = Math.PI / 4;

      Size tileSize = new Size(256, 256);
      public override Size TileSize
      {
         get
         {
            return tileSize;
         }
      }

      public override double Axis
      {
         get
         {
            return 6356752.3142;
         }
      }

      public override double Flattening
      {
         get
         {
            return (1.0 / 298.257223563);
         }
      }

      public override Point FromLatLngToPixel(double lat, double lng, int zoom)
      {
         lat = Clip(lat, MinLatitude, MaxLatitude);
         lng = Clip(lng, MinLongitude, MaxLongitude);

         double rLon = lng * DEG_RAD; // Math.PI / 180; 
         double rLat = lat * DEG_RAD; // Math.PI / 180; 

         double a = 6378137;
         double k = 0.0818191908426;

         double z = Math.Tan(MathPiDiv4 + rLat / 2) / Math.Pow((Math.Tan(MathPiDiv4 + Math.Asin(k * Math.Sin(rLat)) / 2)), k);
         double z1 = Math.Pow(2, 23 - zoom);

         double DX =  ((20037508.342789 + a * rLon) * 53.5865938 /  z1);
         double DY = ((20037508.342789 - a * Math.Log(z)) * 53.5865938 / z1);

         Point ret = Point.Empty;
         ret.X = (int) DX;
         ret.Y = (int) DY;

         return ret;
      }

      public override PointLatLng FromPixelToLatLng(int x, int y, int zoom)
      {
         Size s = GetTileMatrixSizePixel(zoom);

         double mapSizeX = s.Width;
         double mapSizeY = s.Height;

         double a = 6378137;
         double c1 = 0.00335655146887969;
         double c2 = 0.00000657187271079536;
         double c3 = 0.00000001764564338702;
         double c4 = 0.00000000005328478445;
         double z1 = (23 - zoom);
         double mercX = (x * Math.Pow(2, z1)) / 53.5865938 - 20037508.342789;
         double mercY = 20037508.342789 - (y *Math.Pow(2, z1)) / 53.5865938;

         double g = Math.PI /2 - 2 *Math.Atan(1 / Math.Exp(mercY /a));
         double z = g + c1 * Math.Sin(2 * g) + c2 * Math.Sin(4 * g) + c3 * Math.Sin(6 * g) + c4 * Math.Sin(8 * g);

         PointLatLng ret = PointLatLng.Empty;
         ret.Lat = z * RAD_DEG;
         ret.Lng = mercX / a * RAD_DEG;

         return ret;
      }

      /// <summary>
      /// Clips a number to the specified minimum and maximum values.
      /// </summary>
      /// <param name="n">The number to clip.</param>
      /// <param name="minValue">Minimum allowable value.</param>
      /// <param name="maxValue">Maximum allowable value.</param>
      /// <returns>The clipped value.</returns>
      double Clip(double n, double minValue, double maxValue)
      {
         return Math.Min(Math.Max(n, minValue), maxValue);
      }

      public override Size GetTileMatrixMinXY(int zoom)
      {
         return new Size(0, 0);
      }

      public override Size GetTileMatrixMaxXY(int zoom)
      {
         int xy = (1 << zoom);
         return new Size(xy - 1, xy - 1);
      }
   }
}
