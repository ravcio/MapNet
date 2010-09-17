using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class SymbolFeature : Feature
    {
        public double x, y;
        public uint color;
        public int size = 6;

        public SymbolFeature(double x, double y)
        {
            this.x = x;
            this.y = y;
            CalcMBR();
        }

        public SymbolFeature(double x, double y, uint color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            CalcMBR();
        }

        public SymbolFeature(SymbolFeature symbol)
        {
            this.x = symbol.x;
            this.y = symbol.y;
            this.color = symbol.color;
            CalcMBR();
        }

        public override void CalcMBR()
        {
            base.m_oMBR = new DRect(x, y, x, y);
        }

        public virtual int marginBounds()
        {
            return size;
        }

        public override Feature clone()
        {
            return new SymbolFeature(this);
        }
    }
}
