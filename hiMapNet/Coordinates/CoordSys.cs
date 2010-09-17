using System;
using System.Collections.Generic;
using System.Text;

namespace hiMapNet
{
    public class CoordSys
    {
        Datum datum;

        public Datum Datum
        {
            get { return datum; }
        }

        AffineTransform affineTransform;
        public AffineTransform AffineTransform
        {
            get { return affineTransform; }
            set { affineTransform = value; }
        }

        CoordSysType type;

        internal CoordSysType Type
        {
            get { return type; }
            set { type = value; }
        }

        bool Equal(CoordSys coordSys)
        {
            if (type != coordSys.type) return false;
            return true;
        }

        internal CoordSys(CoordSysType type, Datum datum, AffineTransform affineTransform)
        {
            this.type = type;
            this.datum = datum;
            this.affineTransform = affineTransform;
        }

        public CoordSys Clone()
        {
            CoordSys oCS = new CoordSys(type, datum, affineTransform);
            return oCS;
        }
    }
}
