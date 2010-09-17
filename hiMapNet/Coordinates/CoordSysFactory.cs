using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    public class CoordSysFactory
    {
        static public CoordSys CreateCoordSys(
            CoordSysType type,
            Datum datum,
            /*double originLongitude,
            double originLatitude,
            double standardParallelOne,
            double standardParallelTwo,
            double azimuth,
            double scaleFactor,
            double falseEasting,
            double falseNorthing,
            double range,*/
            AffineTransform affineTransform)
        {
            CoordSys oCS = new CoordSys(type, datum, affineTransform);
            return oCS;
        }

        static public CoordSys CreateCoordSys(string name)
        {
            if (name == "Mercator Datum(WGS84)")
            {
                AffineTransform at = new AffineTransform();

                // uklad wsp: 
                // at zamiania wgs84 na px (256x256) i dalej na [m]
//                at.MultiplyInPlace(256.0 / 360.0, -256.0 / 180.0);
//                at.OffsetInPlace(-128, 128);
//                at.MultiplyInPlace(Math.PI * r / 180, Math.PI * r / 180.0);

                CoordSys oCS = new CoordSys(CoordSysType.Mercator, new Datum(DatumID.WGS84), at);
                return oCS;
            }
            else
                throw new Exception("Unsupported coordsys");
        }

        static public Datum CreateDatum(DatumID datumID)
        {
            return new Datum(datumID); 
        }

        static public Datum CreateDatum(Ellipsoid ellipsoid,
            double shiftX,
            double shiftY,
            double shiftZ,
            double rotateX,
            double rotateY,
            double rotateZ,
            double scaleAdjust,
            double primeMeridian)
        {
            return new Datum(ellipsoid, shiftX, shiftY, shiftZ, rotateX, rotateY, rotateZ, scaleAdjust, primeMeridian);
        }
    }
}
