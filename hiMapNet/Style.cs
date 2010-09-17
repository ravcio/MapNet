using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class Style
    {
        private Color m_oLineColor = Color.Black;
        private Color m_oBackColor = Color.Black;
        private Color m_oForeColor = Color.Black;
        private int m_iFillPattern = 1;
        private int m_iLinePattern = 3;
        private int m_iLineWidth = 1;

        private int m_iFontSize; // rozmiar napisu w jednostkach int

        public int FontSize
        {
            get { return m_iFontSize; }
            set { m_iFontSize = value; }
        }
        private int iAngle; // angle in 1/10th of degree

        public int Angle
        {
            get { return iAngle; }
            set { iAngle = value; }
        }
        private int iAttrib;

        public int Attrib
        {
            get { return iAttrib; }
            set { iAttrib = value; }
        }
        private int m_iJustify; //Text attributes: 1=bold, 2=italic, 4=underline, 8=strikethrough, 32=shadow, 256=box, 512=halo, 1024=all caps, 2048=expand (dbl. space). Mix attribute by making a sum value.

        public int Justify
        {
            get { return m_iJustify; }
            set { m_iJustify = value; }
        }
        private string m_sFontName;

        public string FontName
        {
            get { return m_sFontName; }
            set { m_sFontName = value; }
        }


        public Style()
        {

        }

        public Style(Color LineColor)
        {
            m_oLineColor = LineColor;
            m_oForeColor = LineColor;
            m_iLinePattern = 2;
        }


        public Color LineColor
        {
            get
            {
                return m_oLineColor;
            }
            set
            {
                m_oLineColor = value;
            }
        }

        public Color RegionColor
        {
            get
            {
                return m_oForeColor;
            }
            set
            {
                m_oForeColor = value;
            }
        }

        public Color RegionBackColor
        {
            get
            {
                return m_oBackColor;
            }
            set
            {
                m_oBackColor = value;
            }
        }

        public int LineWidth
        {
            get
            {
                return m_iLineWidth;
            }
            set
            {
                m_iLineWidth = value;
            }
        }

        public int LinePattern
        {
            get
            {
                return m_iLinePattern;
            }
            set
            {
                m_iLinePattern = value;
            }
        }

        public int FillPattern
        {
            get
            {
                return m_iFillPattern;
            }
            set
            {
                m_iFillPattern = value;
            }
        }
    }
}
