using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Data;
using System.Security.Cryptography;
using System.Drawing;

namespace gpxEditor
{
    /// <summary>
    /// Pojemnik na punkty GPS, umo¿liwia serializowanie do pliku KML
    /// Przechowuje listy punktów GPS, rozdzielone na roz³¹czne ci¹gi
    /// </summary>
    public class GPXFile
    {
        public GpxWpt location;

        private List<GPXTrk> m_oTracks = new List<GPXTrk>();

        public List<GPXTrk> trks
        {
            get { return m_oTracks; }
        }

        public int getWptCount()
        {
            int count = 0;
            foreach (GPXTrk trk in m_oTracks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    count += seg.wpts.Count;
                }
            }
            return count;
        }

        public GpxWpt getWpt(int index)
        {
            if (index < 0) throw new Exception("Index out of range.");
            int pos = 0;
            foreach (GPXTrk trk in m_oTracks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    if (index >= pos && index < pos + seg.wpts.Count)
                    {
                        return seg.wpts[index - pos];
                    }
                    pos += seg.wpts.Count;
                }
            }
            throw new Exception("Index out of range.");
        }
    }
}
