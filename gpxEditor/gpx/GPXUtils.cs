using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Windows.Forms;
using hiMapNet;
using System.Drawing;

namespace gpxEditor
{
    /// <summary>
    /// Czyta plik GPX (do dom xml) i przenosi punkty GPS do klasy GPXFile
    /// Moves data from GPXFile to and from Map Layers
    /// </summary>
    public static class GPXUtils
    {

        public static void AppendGPX(GPXFile gpxFile, string fileNameGPX)
        {
            XmlDocument oDoc = new XmlDocument();
            oDoc.Load(fileNameGPX);

            // load v.1.0 elements
            DecodeGPX10(gpxFile, oDoc);
            // load v.1.1 elements
            DecodeGPX11(gpxFile, oDoc);
        }

        public static void SaveGPXv11(GPXFile gpxFile, string fileNameGPX)
        {
            XmlDocument oDoc = new XmlDocument();

            const string namespaceUri = "http://www.topografix.com/GPX/1/1";

            XmlNode nodeRoot = oDoc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            oDoc.AppendChild(nodeRoot);

            XmlNode nodeGpx = oDoc.CreateElement("", "gpx", namespaceUri);
            oDoc.AppendChild(nodeGpx);

            XmlAttribute att = oDoc.CreateAttribute("version");
            att.Value = "1.1";
            nodeGpx.Attributes.Append(att);

            att = oDoc.CreateAttribute("xmlns:xsi");
            att.Value = "http://www.w3.org/2001/XMLSchema-instance";
            nodeGpx.Attributes.Append(att);

            att = oDoc.CreateAttribute("xsi:schemaLocation");
            att.Value = "http://www.topografix.com/GPX/1/1/gpx.xsd";
            nodeGpx.Attributes.Append(att);

            XmlNode nodeTrk = oDoc.CreateElement("", "trk", namespaceUri);
            nodeGpx.AppendChild(nodeTrk);

            for (int iTrk = 0; iTrk < gpxFile.trks.Count; iTrk++)
            {
                GPXTrk trk = gpxFile.trks[iTrk];

                for (int iSeg = 0; iSeg < trk.trkSeg.Count; iSeg++)
                {
                    GPXTrkSeg seg = trk.trkSeg[iSeg];

                    XmlNode nodeTrkseg = oDoc.CreateElement("", "trkseg", namespaceUri);

                    if (seg.wpts.Count > 0)
                    {
                        nodeTrk.AppendChild(nodeTrkseg);

                        for (int iWpt = 0; iWpt < seg.wpts.Count; iWpt++)
                        {
                            GpxWpt wp = seg.wpts[iWpt];

                            XmlNode nodeTrkpt = oDoc.CreateElement("", "trkpt", namespaceUri);

                            XmlAttribute attTrkpt;
                            attTrkpt = oDoc.CreateAttribute("lat");
                            attTrkpt.Value = Convert.ToString(wp.lat, CultureInfo.InvariantCulture);
                            nodeTrkpt.Attributes.Append(attTrkpt);
                            attTrkpt = oDoc.CreateAttribute("lon");
                            attTrkpt.Value = Convert.ToString(wp.lon, CultureInfo.InvariantCulture);
                            nodeTrkpt.Attributes.Append(attTrkpt);

                            if (wp.ele > -500)
                            {
                                XmlNode nodeEle = oDoc.CreateElement("", "ele", namespaceUri);
                                nodeEle.InnerText = Convert.ToString(wp.ele, CultureInfo.InvariantCulture);
                                nodeTrkpt.AppendChild(nodeEle);
                            }

                            if (wp.time != null)
                            {
                                if (wp.time > DateTime.MinValue)
                                {
                                    XmlNode nodeTime = oDoc.CreateElement("", "time", namespaceUri);
                                    nodeTime.InnerText = XmlConvert.ToString(wp.time, "yyyy-MM-ddTHH:mm:ss");
                                    nodeTrkpt.AppendChild(nodeTime);
                                }
                            }

                            nodeTrkseg.AppendChild(nodeTrkpt);
                        }
                    }
                }
            }

            oDoc.Save(fileNameGPX);

            // validate resulting file
            //...
            GPXValidator validator = new GPXValidator();
            string error = validator.validate(fileNameGPX);
            if (error != "") MessageBox.Show(error);
        }


        private static void DecodeGPX10(GPXFile gpxFile, XmlDocument oDoc)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(oDoc.NameTable);
            manager.AddNamespace("oo", "http://www.topografix.com/GPX/1/0");
                                      //http://www.topografix.com/GPX/1/0

            XmlNodeList oTracks = oDoc.SelectNodes("oo:gpx/oo:trk", manager);
            foreach (XmlNode xmltrk in oTracks)
            {
                GPXTrk gpxtrk = new GPXTrk();
                gpxtrk.parent = gpxFile;
                gpxFile.trks.Add(gpxtrk);

                XmlNode oName = xmltrk.SelectSingleNode("oo:name", manager);
                if (oName != null)
                {
                    gpxtrk.name = oName.InnerText;
                }
                //...
                XmlNodeList oTrkSeg = xmltrk.SelectNodes("oo:trkseg", manager);
                foreach (XmlNode xmlseg in oTrkSeg)
                {
                    GPXTrkSeg gpxseg = new GPXTrkSeg();
                    gpxseg.parent = gpxtrk;
                    gpxtrk.trkSeg.Add(gpxseg);
                    XmlNodeList oTrackPoints = xmlseg.SelectNodes("oo:trkpt", manager);

                    foreach (XmlNode oTrkPt in oTrackPoints)
                    {
                        string sLat = oTrkPt.Attributes.GetNamedItem("lat").Value;
                        string sLon = oTrkPt.Attributes.GetNamedItem("lon").Value;
                        XmlNode nodeTime = oTrkPt.SelectSingleNode("oo:time", manager);
                        string sTime = "";
                        if (nodeTime != null)
                        {
                            sTime = nodeTime.InnerText;
                        }
                        XmlNode oN = oTrkPt.SelectSingleNode("oo:ele", manager);
                        string sEle = "-9999";
                        if (oN != null)
                        {
                            sEle = oN.InnerText;
                        }
                        string sSpeed = "0";
                        oN = oTrkPt.SelectSingleNode("oo:speed", manager);
                        if (oN != null)
                        {
                            sSpeed = oN.InnerText;
                        }

                        double dLat = Convert.ToDouble(sLat, CultureInfo.InvariantCulture);
                        double dLon = Convert.ToDouble(sLon, CultureInfo.InvariantCulture);
                        DateTime dDTime;
                        DateTime dDTime_utc;
                        if (sTime != "")
                        {
                            //DateTime t1 = DateTime.Parse("2009-07-05T09:06:07Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                            //DateTime t2 = DateTime.Parse("2009-07-05T09:06:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

                            dDTime = DateTime.Parse(sTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                            dDTime_utc = dDTime.ToUniversalTime();
                        }
                        else
                        {
                            dDTime = DateTime.MinValue;
                            dDTime_utc = DateTime.MinValue;
                        }
                        double dEle = Convert.ToDouble(sEle, CultureInfo.InvariantCulture);
                        double dSpeed = Convert.ToDouble(sSpeed, CultureInfo.InvariantCulture);

                        GpxWpt wpt = new GpxWpt(dLon, dLat, dEle, new DateTime(dDTime.Ticks, DateTimeKind.Utc), dSpeed);
                        wpt.parent = gpxseg;
                        gpxseg.wpts.Add(wpt);
                    }
                }
            }
            return;
        }


        private static void DecodeGPX11(GPXFile gpxFile, XmlDocument oDoc)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(oDoc.NameTable);
            manager.AddNamespace("oo", "http://www.topografix.com/GPX/1/1");

            XmlNodeList oTracks = oDoc.SelectNodes("oo:gpx/oo:trk", manager);
            foreach (XmlNode xmltrk in oTracks)
            {
                GPXTrk gpxtrk = new GPXTrk();
                gpxtrk.parent = gpxFile;
                gpxFile.trks.Add(gpxtrk);

                XmlNode nameNode = xmltrk.SelectSingleNode("oo:name", manager);
                if (nameNode != null) gpxtrk.name = nameNode.InnerText;
                XmlNode cmtNode = xmltrk.SelectSingleNode("oo:cmt", manager);
                if (cmtNode != null) gpxtrk.cmt = cmtNode.InnerText;
                XmlNode descNode = xmltrk.SelectSingleNode("oo:desc", manager);
                if (descNode != null) gpxtrk.desc = descNode.InnerText;

                XmlNodeList oTrkSeg = xmltrk.SelectNodes("oo:trkseg", manager);
                foreach (XmlNode xmlseg in oTrkSeg)
                {
                    GPXTrkSeg gpxseg = new GPXTrkSeg();
                    gpxseg.parent = gpxtrk;
                    gpxtrk.trkSeg.Add(gpxseg);
                    XmlNodeList oTrackPoints = xmlseg.SelectNodes("oo:trkpt", manager);

                    foreach (XmlNode oTrkPt in oTrackPoints)
                    {
                        string sLat = oTrkPt.Attributes.GetNamedItem("lat").Value;
                        string sLon = oTrkPt.Attributes.GetNamedItem("lon").Value;

                        string sTime = "";
                        XmlNode nodeTime = oTrkPt.SelectSingleNode("oo:time", manager);
                        if (nodeTime != null)
                        {
                            sTime = nodeTime.InnerText;
                        }

                        XmlNode oN = oTrkPt.SelectSingleNode("oo:ele", manager);
                        string sEle = "-9999";
                        if (oN != null)
                        {
                            sEle = oN.InnerText;
                        }
                        string sSpeed = "0";
                        oN = oTrkPt.SelectSingleNode("oo:speed", manager);
                        if (oN != null)
                        {
                            sSpeed = oN.InnerText;
                        }

                        double dLat = Convert.ToDouble(sLat, CultureInfo.InvariantCulture);
                        double dLon = Convert.ToDouble(sLon, CultureInfo.InvariantCulture);

                        DateTime dDTime;
                        DateTime dDTime_utc;
                        if (sTime != "")
                        {
                            //DateTime t1 = DateTime.Parse("2009-07-05T09:06:07Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                            //DateTime t2 = DateTime.Parse("2009-07-05T09:06:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

                            dDTime = DateTime.Parse(sTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                            dDTime_utc = dDTime.ToUniversalTime();
                        }
                        else
                        {
                            dDTime = DateTime.MinValue;
                            dDTime_utc = DateTime.MinValue;
                        }

                        double dEle = Convert.ToDouble(sEle, CultureInfo.InvariantCulture);
                        double dSpeed = Convert.ToDouble(sSpeed, CultureInfo.InvariantCulture);


                        GpxWpt wpt = new GpxWpt(dLon, dLat, dEle, dDTime_utc, dSpeed);
                        wpt.parent = gpxseg;
                        gpxseg.wpts.Add(wpt);
                    }
                }
            }
            return;
        }



        /// <summary>
        /// create features on layer based on gpxFile object
        /// </summary>
        /// <param name="gpxFile"></param>
        /// <param name="oLayerGPXPolylines"></param>
        /// <param name="oLayerGPXSymbols"></param>
        public static void makeMapLayersFromGPX(GPXFile gpxFile, LayerVectors oLayerGPXPolylines, LayerVectors oLayerGPXSymbols)
        {
            oLayerGPXSymbols.FeaturesClear();
            oLayerGPXPolylines.FeaturesClear();

            // fill layers with data
            if (gpxFile != null)
            {
                for (int iTrk = 0; iTrk < gpxFile.trks.Count; iTrk++)
                {
                    GPXTrk trk = gpxFile.trks[iTrk];

                    for (int iSeg = 0; iSeg < trk.trkSeg.Count; iSeg++)
                    {
                        GPXTrkSeg seg = trk.trkSeg[iSeg];
                        List<DPoint> oPoints1 = new List<DPoint>();

                        for (int iWpt = 0; iWpt < seg.wpts.Count; iWpt++)
                        {
                            GpxWpt wpt = seg.wpts[iWpt];

                            DPoint oPoint = new DPoint(wpt.lon, wpt.lat);
                            oPoint.Selected = wpt.selected;

                            oPoints1.Add(oPoint);

                            // add gpx point
                            Feature oSymbol = FeatureFactory.CreateSymbol(wpt.lon, wpt.lat, (uint)0xFF00FF00);
                            oSymbol.Tag = oPoint;
                            oLayerGPXSymbols.FeaturesAdd(oSymbol);

                            // link point to symbol and symbol to point
                            oPoint.Tag = oSymbol;

                            // fill data
                            oSymbol.setField("lat", wpt.lat);
                            oSymbol.setField("lon", wpt.lon);
                            oSymbol.setField("ele", wpt.ele);
                            oSymbol.setField("time", wpt.time);
                        }
                        if (oPoints1.Count > 0)
                        {
                            PolylineFeature oPolyline1 = FeatureFactory.CreatePolyline(oPoints1, new Style(Color.Green));
                            oLayerGPXPolylines.FeaturesAdd(oPolyline1);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// rebuild gpxFile from Map Layers
        /// </summary>
        /// <param name="oLayerGPXPolylines"></param>
        /// <param name="oLayerGPXSymbols"></param>
        /// <returns></returns>
        public static GPXFile makeGPXfromMapLayers(LayerVectors oLayerGPXPolylines, LayerVectors oLayerGPXSymbols)
        {
            GPXFile gpxFile = new GPXFile();

            GPXTrk trk = new GPXTrk();
            gpxFile.trks.Add(trk);

            for (int i = 0; i < oLayerGPXPolylines.FeaturesCount; i++)
            {
                GPXTrkSeg seg = new GPXTrkSeg();

                Feature f = oLayerGPXPolylines.FeatureGet(i);
                if (f is PolylineFeature)
                {
                    PolylineFeature oPolyline = (PolylineFeature)f;

                    for (int ipart = 0; ipart < oPolyline.m_oParts.Count; ipart++)
                    {
                        Part part = oPolyline.m_oParts[ipart];
                        for (int ipoint = 0; ipoint < part.Points.Count; ipoint++)
                        {
                            DPoint layerPoint = part.Points[ipoint];
                            SymbolFeature symbol = (SymbolFeature)layerPoint.Tag;

                            double lon = layerPoint.X;
                            double lat = layerPoint.Y;
                            double ele = (double)symbol.getField("ele");
                            DateTime time = (DateTime)symbol.getField("time");

                            GpxWpt wpt = new GpxWpt(lon, lat, ele, time, -99);
                            wpt.selected = layerPoint.Selected;

                            seg.wpts.Add(wpt);
                        }
                    }
                }
                if (seg.wpts.Count > 0)
                {
                    trk.trkSeg.Add(seg);
                }
            }
            return gpxFile;
        }

        internal static double CalcLength(GPXFile gpxFile)
        {
            double len = 0;
            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    if (seg.wpts.Count > 1)
                    {
                        for (int i = 1; i < seg.wpts.Count; i++)
                        {
                            GpxWpt wpt1 = seg.wpts[i - 1];
                            GpxWpt wpt2 = seg.wpts[i];

                            len = len + distance(wpt1.lon, wpt1.lat, wpt2.lon, wpt2.lat);
                        }
                    }
                }
            }
            return len;
        }

        /// <summary>
        /// Distance in meters between point 1 and point 2.
        /// Coordinates expressed in WGS84 [deg]
        ///
        /// double dist = distance(-86.67, 36.12,  -118.40, 33.94); // =2886km
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        static double distance(double x1_wgs84, double y1_wgs84, double x2_wgs84, double y2_wgs84)
        {
            // przelicz stopnie na radiany
            x1_wgs84 = x1_wgs84 / 180.0 * Math.PI;
            y1_wgs84 = y1_wgs84 / 180.0 * Math.PI;
            x2_wgs84 = x2_wgs84 / 180.0 * Math.PI;
            y2_wgs84 = y2_wgs84 / 180.0 * Math.PI;

            double Rx2 = 6371744.54 * 2;
            double distance;
            double sin_y = Math.Sin((y1_wgs84 - y2_wgs84) / 2);
            double sin_x = Math.Sin((x1_wgs84 - x2_wgs84) / 2);
            double sqrsin_y = sin_y * sin_y;
            double sqrsin_x = sin_x * sin_x;
            double aa = sqrsin_y + Math.Cos(y1_wgs84) * Math.Cos(y2_wgs84) * sqrsin_x;

            if (aa >= 0 && aa <= 1)
            {
                distance = Rx2 * Math.Asin(Math.Sqrt(aa));
            }
            else
            {
                distance = 0;
            }
            return distance;
        }


        internal static string TimeSpan(GPXFile gpxFile)
        {
            DateTime minTime = DateTime.MinValue;
            DateTime maxTime = DateTime.MinValue;

            string message = "";
            foreach (GPXTrk trk in gpxFile.trks)
            {
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        if (wpt.time != DateTime.MinValue)
                        {
                            if (minTime == DateTime.MinValue) minTime = wpt.time;
                            if (maxTime == DateTime.MinValue) maxTime = wpt.time;
                            if (wpt.time < minTime) minTime = wpt.time;
                            if (wpt.time > maxTime) maxTime = wpt.time;
                        }
                    }
                }
            }

            if (minTime != DateTime.MinValue && maxTime != DateTime.MinValue)
            {
                TimeSpan ts = maxTime - minTime;
                message = ts.ToString();
            }
            else
            {
                message = "no available";
            }
            return message;
        }
    }
}
