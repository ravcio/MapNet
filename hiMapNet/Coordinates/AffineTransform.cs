using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    public class AffineTransform
    {
        double a, b, c, d, e, f;

        public double F
        {
            get { return f; }
            set { f = value; }
        }

        public double E
        {
            get { return e; }
            set { e = value; }
        }

        public double D
        {
            get { return d; }
            set { d = value; }
        }

        public double C
        {
            get { return c; }
            set { c = value; }
        }

        public double B
        {
            get { return b; }
            set { b = value; }
        }

        public double A
        {
            get { return a; }
            set { a = value; }
        }

        public AffineTransform()
        {
            a = e = 1;
            b = c = d = f = 0;
        }

        public void Convert(double x, double y, out double xOut, out double yOut)
        {
            xOut = x * a + y * b + c;
            yOut = x * d + y * e + f;
        }

        public void ConvertInverse(double x, double y, out double xOut, out double yOut)
        {
            double determinant = a * e - b * d;
            xOut = 0;
            yOut = 0;
            if (determinant != 0)
            {
                xOut = (x - c) / determinant * e - (y - f) / determinant * b;
                yOut = -(x - c) / determinant * d + (y - f) / determinant * a;
            }
            else
            {
                throw new Exception("Can not inverse");
            }
        }

        public AffineTransform Compose(AffineTransform at)
        {
            if (at == null) throw new ArgumentNullException("at must not be null.");
            AffineTransform res = new AffineTransform();
            res.a = at.a * a + at.b * d;
            res.b = at.a * b + at.b * e;
            res.c = at.a * c + at.b * f + at.c;
            res.d = at.d * a + at.e * d;
            res.e = at.d * b + at.e * e;
            res.f = at.d * c + at.e * f + at.f;
            return res;
        }

        public AffineTransform Inverse()
        {
            double det = Determinant();
            if (det == 0) return null;
            double invDeterminant = 1 / det;
            AffineTransform res = new AffineTransform();
            res.a = invDeterminant * e;
            res.b = -invDeterminant * b;
            res.c = -invDeterminant * (e * c - b * f);
            res.d = -invDeterminant * d;
            res.e = invDeterminant * a;
            res.f = invDeterminant * (d * c - a * f);
            return res;
        }

        double Determinant()
        {
            return a * e - b * d;
        }

        public void Reset()
        {
            a = e = 1;
            b = c = d = f = 0;
        }

        public void Set(double a, double b, double c, double d, double e, double f)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }

        public void MultiplyInPlace(double xScale, double yScale)
        {
            a = a * xScale;
            e = e * yScale;
        }

        public void OffsetInPlace(double xOffset, double yOffset)
        {
            c = c + xOffset;
            f = f + yOffset;
        }

        internal AffineTransform Clone()
        {
            AffineTransform at = new AffineTransform();
            at.Set(a, b, c, d, e, f);
            return at;
        }

        public bool IsRotating()
        {
            return !(d == 0 && b == 0);
        }

        public bool IsScaling()
        {
            return !(a == 1 && e == 1);
        }

        public bool IsOffsetting()
        {
            return !(c == 0 && f == 0);
        }
    }
}
