using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace hiMapNet
{
    public class Datum
    {
        double eccentricity;

        public double Eccentricity
        {
            get { return eccentricity; }
            set { eccentricity = value; }
        }
        double flattening;

        public double Flattening
        {
            get { return flattening; }
            set { flattening = value; }
        }
        
        Ellipsoid ellipsoid;

        public Ellipsoid Ellipsoid
        {
            get { return ellipsoid; }
        }
        double primeMeridian;

        public double PrimeMeridian
        {
            get { return primeMeridian; }
        }
        double rotateX;

        public double RotateX
        {
            get { return rotateX; }
        }
        double rotateY;

        public double RotateY
        {
            get { return rotateY; }
        }
        double rotateZ;

        public double RotateZ
        {
            get { return rotateZ; }
        }
        double adjustScale;

        public double AdjustScale
        {
            get { return adjustScale; }
        }
        double semiMajorAxis;

        public double SemiMajorAxis
        {
            get { return semiMajorAxis; }
        }
        double semiMinorAxis;

        public double SemiMinorAxis
        {
            get { return semiMinorAxis; }
        }
        double shiftX;

        public double ShiftX
        {
            get { return shiftX; }
        }
        double shiftY;
        public double ShiftY
        {
            get { return shiftY; }
        }
        double shiftZ;

        public double ShiftZ
        {
            get { return shiftZ; }
        }

        internal Datum(DatumID datumID)
        {
            switch (datumID)
            {
                case DatumID.WGS84:
                    SetEllipsoid(Ellipsoid.GRS80);
                    adjustScale = 0.0;
                    primeMeridian = 0.0;
                    shiftX = 0.0;
                    shiftY = 0.0;
                    shiftZ = 0.0;
                    rotateX = 0.0;
                    rotateY = 0.0;
                    rotateZ = 0.0;

                    break;
                default :
                    Debug.Assert(false);
                    break;
            }
        }

        internal Datum(Ellipsoid ellipsoid,
            double shiftX,
            double shiftY,
            double shiftZ,
            double rotateX,
            double rotateY,
            double rotateZ,
            double scaleAdjust,
            double primeMeridian)
        {
            SetEllipsoid(ellipsoid);
            this.adjustScale = scaleAdjust;
            this.primeMeridian = primeMeridian;
            this.shiftX = shiftX;
            this.shiftY = shiftY;
            this.shiftZ = shiftZ;
            this.rotateX = rotateX;
            this.rotateY = rotateY;
            this.rotateZ = rotateZ;
        }

        public void Clone()
        {
            throw new System.NotImplementedException();
        }

        private void SetEllipsoid(Ellipsoid ellipsoid)
        {
            if (ellipsoid == Ellipsoid.GRS80)
            {
                semiMajorAxis = 6378137;
                semiMinorAxis = 6356752.314;
            }
            else if (ellipsoid == Ellipsoid.Sphere)
            {
                semiMajorAxis = 6378137;
                semiMinorAxis = 6378137;
            }
            else
            {
                Debug.Assert(false);
            }
            this.ellipsoid = ellipsoid;
            this.flattening = (semiMajorAxis - semiMinorAxis) / semiMajorAxis;  // e.g. = 0.0033528107031880554
            this.eccentricity = Math.Sqrt(flattening * (2.0 - flattening));  // e.g. = 0.081819191310869718
        }




    }
}
