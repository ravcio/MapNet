using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace hiMapNet
{
    public class TextFeature : Feature
    {
        private System.Drawing.Point m_oPosition;

        public System.Drawing.Point Position
        {
            get { return m_oPosition; }
            set { m_oPosition = value; }
        }
        private string m_oText;

        public string Text
        {
            get { return m_oText; }
            set { m_oText = value; }
        }

        public override void CalcMBR()
        {
            int iSize = base.m_oStyle.FontSize;
            base.m_oMBR = new DRect(m_oPosition.X - iSize, m_oPosition.Y - iSize, m_oPosition.X + iSize, m_oPosition.Y + iSize);
        }

        public TextFeature(DPoint oP, string Text, Style oStyle)
        {
            m_oText = Text;
            //m_oPosition = oP;
            base.m_oStyle = oStyle;
            m_oLineEnd = new Point();
            m_iEndType = 0;
        }

        Point m_oLineEnd;
        int m_iEndType;

        public TextFeature(Point oP, string Text, Style oStyle, Point lineEnd, int endType)
        {
            m_oText = Text;
            m_oPosition = oP;
            m_oStyle = oStyle;
            m_oLineEnd = lineEnd;
            m_iEndType = endType;
        }

        public override Feature clone()
        {
            throw new NotImplementedException();
        }
    }
}
