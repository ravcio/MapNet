using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;
using System.Diagnostics;

namespace hiMapNet
{
    public class CoordConverter
    {
        CoordSys csSource;
        CoordSys csTarget;

	    const double SemiMajorAxis_WGS84 = 6378137;
        const double SemiMinorAxis_WGS84 = 6356752.314;

        public AffineTransform atMaster;

        double x;

        public double X
        {
            get { return x; }
        }
        double y;

        public double Y
        {
            get { return y; }
        }

        public void Init(CoordSys source, CoordSys target)
        {
            if (source == null) throw new ArgumentNullException("source must not be null.");
            if (target == null) throw new ArgumentNullException("target must not be null.");

            atMaster = new AffineTransform();
            atMaster = source.AffineTransform.Inverse();
            atMaster = atMaster.Compose(target.AffineTransform);

            csSource = source;
            csTarget = target;
        }

        public void Convert(double x, double y)
        {
            double xOut = Double.NaN;
            double yOut = Double.NaN;
            if (csSource.Type == csTarget.Type)
            {
                atMaster.Convert(x, y, out xOut, out yOut);
                this.x = xOut;
                this.y = yOut;
                return;
            }

            double xIn = x;
            double yIn = y;

            csSource.AffineTransform.ConvertInverse(xIn, yIn, out xOut, out yOut);

            xIn = xOut;
            yIn = yOut;

            // source to wgs84
            if (csSource.Type == CoordSysType.LatLong)
            {
                xOut = makeRadians(x);
                yOut = makeRadians(y);
            }
            else if (csSource.Type == CoordSysType.Mercator)
            {
                if (csSource.Datum.SemiMajorAxis == csSource.Datum.SemiMinorAxis)
                {
                    double a = csSource.Datum.SemiMajorAxis;
                    double e = csSource.Datum.Eccentricity;
                    double pm = csSource.Datum.PrimeMeridian;
                    ConvertMercatortoLLSphere(a, e, pm, out xOut, out yOut, xIn, yIn);
                }
                else
                {
                    double a = csSource.Datum.SemiMajorAxis;
                    double e = csSource.Datum.Eccentricity;
                    double pm = csSource.Datum.PrimeMeridian;
                    //ConvertMercatortoLL(a, e, pm, out xOut, out yOut, xIn, yIn);
                    ConvertMercatortoLLSphere(a, e, pm, out xOut, out yOut, xIn, yIn); // TODO - correct
                }
            }
            else
            {
                Debug.Assert(false);
            }

            // this is wgs84 [rad]
            xIn = xOut;
            yIn = yOut;

            // wgs84 to target
            if (csTarget.Type == CoordSysType.LatLong)
            {
                xOut = makeDeg(xIn);
                yOut = makeDeg(yIn);
            }
            else if (csTarget.Type == CoordSysType.Mercator)
            {
                if (csTarget.Datum.SemiMajorAxis == csTarget.Datum.SemiMinorAxis)
                {
                    double a = csTarget.Datum.SemiMajorAxis;
                    double e = csTarget.Datum.Eccentricity;
                    double pm = csTarget.Datum.PrimeMeridian;
                    ConvertLLtoMercatorSphere(a, e, pm, xIn, yIn, out xOut, out yOut);
                }
                else
                {
                    /*
                    double a = csTarget.Datum.SemiMajorAxis;
                    double e = csTarget.Datum.Eccentricity;
                    double pm = csTarget.Datum.PrimeMeridian;
                    ConvertLLtoMercator(a, e, pm, xIn, yIn, out xOut, out yOut);
                    */
                    Datum datumSphere = CoordSysFactory.CreateDatum(Ellipsoid.Sphere, 0, 0, 0, 0, 0, 0, 0, 0);
                    //DatumConvertion(datumSphere, xIn, yIn, out xOut, out yOut, false);
                    //xIn = xOut;
                    //yIn = yOut;
                    double a = datumSphere.SemiMajorAxis;
                    double e = datumSphere.Eccentricity;
                    double pm = datumSphere.PrimeMeridian;
                    ConvertLLtoMercator(a, e, pm, xIn, yIn, out xOut, out yOut);




                    //ConvertLLtoMercatorSphere(a, e, pm, xIn, yIn, out xOut, out yOut); // TODO
                }
            }
            else
            {
                Debug.Assert(false);
            }

            xIn = xOut;
            yIn = yOut;

            csTarget.AffineTransform.Convert(xIn, yIn, out xOut, out yOut);

            xIn = xOut;
            yIn = yOut;

            atMaster.Convert(xIn, yIn, out xOut, out yOut);

            this.x = xOut;
            this.y = yOut;
        }

        public void ConvertInverse(double x, double y)
        {
            double xOut = Double.NaN;
            double yOut = Double.NaN;

            if (csSource.Type == csTarget.Type)
            {
                atMaster.ConvertInverse(x, y, out xOut, out yOut);
                this.x = xOut;
                this.y = yOut;
                return;
            }

            double xIn = x;
            double yIn = y;
            atMaster.ConvertInverse(xIn, yIn, out xOut, out yOut);
            xIn = xOut;
            yIn = yOut;

            csTarget.AffineTransform.ConvertInverse(xIn, yIn, out xOut, out yOut);
            xIn = xOut;
            yIn = yOut;

            // target to wgs84
            if (csTarget.Type == CoordSysType.LatLong)
            {
                xOut = makeRadians(xIn);
                yOut = makeRadians(yIn);
            }
            else if (csTarget.Type == CoordSysType.Mercator)
            {
                if (csTarget.Datum.SemiMajorAxis == csTarget.Datum.SemiMinorAxis)
                {
                    double a = csTarget.Datum.SemiMajorAxis;
                    double e = csTarget.Datum.Eccentricity;
                    double pm = csTarget.Datum.PrimeMeridian;
                    ConvertMercatortoLLSphere(a, e, pm, out xOut, out yOut, xIn, yIn);
                }
                else
                {
                    double a = csTarget.Datum.SemiMajorAxis;
                    double e = csTarget.Datum.Eccentricity;
                    double pm = csTarget.Datum.PrimeMeridian;
                    //                    ConvertMercatortoLL(a, e, pm, out xOut, out yOut, xIn, yIn);
                    ConvertMercatortoLLSphere(a, e, pm, out xOut, out yOut, xIn, yIn);
                }
            }
            else
            {
                Debug.Assert(false);
            }
            xIn = xOut;
            yIn = yOut;

            // wgs84 to source
            if (csSource.Type == CoordSysType.LatLong)
            {
                xOut = makeDeg(xIn);
                yOut = makeDeg(yIn);
            }
            else if (csSource.Type == CoordSysType.Mercator)
            {
                if (csSource.Datum.SemiMajorAxis == csSource.Datum.SemiMinorAxis)
                {
                    double a = csSource.Datum.SemiMajorAxis;
                    double e = csSource.Datum.Eccentricity;
                    double pm = csSource.Datum.PrimeMeridian;
                    ConvertLLtoMercatorSphere(a, e, pm, xIn, yIn, out xOut, out yOut);
                }
                else
                {
                    double a = csSource.Datum.SemiMajorAxis;
                    double e = csSource.Datum.Eccentricity;
                    double pm = csSource.Datum.PrimeMeridian;
                    //                    ConvertLLtoMercator(a, e, pm, xIn, yIn, out xOut, out yOut);
                    ConvertLLtoMercatorSphere(a, e, pm, xIn, yIn, out xOut, out yOut);
                }
            }
            else
            {
                Debug.Assert(false);
            }

            this.x = xOut;
            this.y = yOut;
        }


        // Konwertuje wys, szer na odwzorowanie Merkator-a
        //' da = e.g. 6377397.155
        //' df = e.g. 1 / 299.1528128
        //' dE = eccentricity
        //' dLonOfOrigin = longitude of origin (rad)
        //' dfi = latitude (rad)  (Y)
        //' dLambda = logitude (rad)  (X)
        //' out: dEasting = x
        //' out: dNorthing = y
        private void ConvertLLtoMercator(double dA, double dE, double dLonOfOrigin, double dLambda, double dfi, out double dEasting, out double dNorthing)
        {
            dEasting = dA * (dLambda - dLonOfOrigin);

            double e = dE;
            double e2 = e * e;
            double e4 = e2 * e2;
            double e6 = e2 * e4;
            //double e8 = e4*e4;
            double A = -(e2 + e4 / 4.0 + e6 / 8.0);
            double B = +(e4 / 12.0 + e6 / 16.0);
            double C = -(e6 / 80.0);
            double D = 0.0;
            double AA = A + 3 * B + 5 * C + 7 * D;
            double BB = -4 * B - 20 * C - 56 * D;
            double CC = -16 * C + 112 * D;
            double DD = -64 * D;
            double sinY = Math.Sin(dfi);
            double sinY2 = sinY * sinY;
            dNorthing = Math.Log(Math.Tan(Math.PI / 4 + dfi / 2.0));
            dNorthing = dNorthing + sinY * (AA + sinY2 * (BB + sinY2 * (CC + DD * sinY2)));
            dNorthing = dNorthing * dA;
        }

        // Konwertuje odwzorowanie Merkator-a na wys, szer
        //' da =6377397.155
        //' df = 1 / 299.1528128
        //' dE = eccentricity
        //' dLonOfOrigin = longitude of origin (rad)
        //' out: dfi = latitude (rad)  (Y)
        //' out: dLambda = logitude (rad)  (X)
        //' dEasting = x
        //' dNorthing = y
        private void ConvertMercatortoLL(double dA, double dE, double dLonOfOrigin, out double dLambda, out double dfi, double dEasting, double dNorthing)
        {
            dLambda = dEasting / dA + dLonOfOrigin;

            double t = Math.Exp(-dNorthing / dA);
            double thi = Math.PI / 2 - 2.0 * Math.Atan(t);
            double e = dE;
            double e2 = e * e;
            double e4 = e2 * e2;
            double e6 = e2 * e4;
            double e8 = e4 * e4;

            double A = e2 / 2 + 5 * e4 / 24 + e6 / 12 + 13 * e8 / 360;
            double B = 7 * e4 / 48 + 29 * e6 / 240 + 811 * e8 / 11520;
            double C = 7 * e6 / 120 + 81 * e8 / 1120;
            double D = 4279 * e8 / 161280;

            double AA = A - C;
            double BB = 2 * B - 4 * D;
            double CC = 4 * C;
            double DD = 8 * D;

            double sin2Y = Math.Sin(2 * thi);
            double cos2Y = Math.Cos(2 * thi);

            dfi = thi + sin2Y * (AA + cos2Y * (BB + cos2Y * (CC + DD * cos2Y)));
        }

        public double makeRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        public double makeDeg(double rad)
        {
            return rad / Math.PI * 180.0;
        }

        private void ConvertLLtoMercatorSphere(double dA, double dE, double dLonOfOrigin, double dLambda, double dfi, out double dEasting, out double dNorthing)
        {
            //if (dfi > ..  // check if dfi is within the allowed range
            dEasting = dA * (dLambda - dLonOfOrigin);
            dNorthing = dA * Math.Log(Math.Tan(Math.PI / 4.0 + dfi / 2.0));
        }

        private void ConvertMercatortoLLSphere(double dA, double dE, double dLonOfOrigin, out double dLambda, out double dfi, double dEasting, double dNorthing)
        {
            dLambda = dEasting / dA + dLonOfOrigin;
            dfi = Math.PI / 2.0 - 2.0 * Math.Atan(Math.Exp(-dNorthing / dA));
        }

        // 2003-10-20 by Rav
        // converts coordinates from WGS84 to given datum (forward)
        // in: dLange, dBreite (rad) expressed in Datum WGS84
        // in: bForward - true=forward convertion (WGS84 to datum), false=datum to WGS84
        // out: dLange_out, dBreite_out expressed in Datum datum
        public void DatumConvertion(Datum datum, double dLange, double dBreite, out double dLange_out, out double dBreite_out, bool bForward)
        {
            double l1;    // ' WGS84 L
            double b1;    // ' WGS B
            double h1;    // ' WGS Height

            double l2;    // ' DHDN(Bessel) L
            double b2;    // ' DHDN(Bessel) B
            double h2;    // ' DHDN(Bessel) Height

            double dA;
            double dB;
            double eq;
            double n;
            double Xq;
            double Yq;
            double Zq;
            double x;
            double y;
            double z;

            l1 = dLange;
            b1 = dBreite;
            h1 = 0;

            if (bForward == true)
            {
                dA = SemiMajorAxis_WGS84;
                dB = SemiMinorAxis_WGS84;
            }
            else
            {
                dA = datum.SemiMajorAxis;
                dB = datum.SemiMinorAxis;
            }

            eq = (dA * dA - dB * dB) / (dA * dA);  // e^2;
            n = dA / Math.Sqrt(1.0 - eq * Math.Sin(b1) * Math.Sin(b1));  // v
            Xq = (n + h1) * Math.Cos(b1) * Math.Cos(l1);
            Yq = (n + h1) * Math.Cos(b1) * Math.Sin(l1);
            Zq = ((1.0 - eq) * n + h1) * Math.Sin(b1);

            HelmertTransformation(datum, Xq, Yq, Zq, out x, out y, out z);

            if (bForward == true)
            {
                dA = datum.SemiMajorAxis;
                dB = datum.SemiMinorAxis;
            }
            else
            {
                dA = SemiMajorAxis_WGS84;
                dB = SemiMinorAxis_WGS84;
            }

            eq = (dA * dA - dB * dB) / (dA * dA);  // e^2;

            BLRauenberg(x, y, z, out b2, out l2, out h2, dA, eq);

            dLange_out = l2;  // result
            dBreite_out = b2;   // result
        }

        private void HelmertTransformation(Datum datum, double x, double y, double z, out double xo, out double yo, out double zo)
        {
            double scaleAdjust = 1.0 / (1.0 - datum.AdjustScale / 1000000.0);  // precalc

            xo = datum.ShiftX + (scaleAdjust * (1.0 * x + datum.RotateZ * y - datum.RotateY * z));
            yo = datum.ShiftY + (scaleAdjust * (-datum.RotateZ * x + 1.0 * y + datum.RotateX * z));
            zo = datum.ShiftZ + (scaleAdjust * (datum.RotateY * x - datum.RotateX * y + 1.0 * z));
        }

        private void BLRauenberg(double x, double y, double z, out double b, out double l, out double H, double a, double eq)
        {
            double f;
            double f1;
            double f2;
            double ft;
            double p;

            f = makeRadians(50.0);
            p = z / Math.Sqrt(x * x + y * y);

            do
            {
                f1 = newF(f, x, y, p, a, eq);
                f2 = f;
                f = f1;
                ft = makeRadians(f1);
            } while ((Math.Abs(f2 - f1) >= 0.000000001));

            b = f;
            l = Math.Atan(y / x);
            H = Math.Sqrt(x * x + y * y) / Math.Cos(f1) - a / Math.Sqrt(1 - eq * Math.Sin(f1) * Math.Sin(f1));
        }

        private double newF(double f, double x, double y, double p, double a, double eq)
        {
            double zw;
            double nnq;

            zw = a / Math.Sqrt(1 - eq * Math.Sin(f) * Math.Sin(f));
            nnq = 1 - eq * zw / (Math.Sqrt(x * x + y * y) / Math.Cos(f));
            return Math.Atan(p / nnq);
        }
    }
}
