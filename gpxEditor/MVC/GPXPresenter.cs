using System;
using System.Collections.Generic;
using System.Text;
using hiMapNet;
using System.Diagnostics;

namespace gpxEditor
{
    public class GPXPresenter
    {
        List<IGPXView> views = new List<IGPXView>();

        // Model
        public GPXFile gpxFile;
        public string lastGpxFilename = "";


        public GPXPresenter()
        {
            gpxFile = new GPXFile();
        }

        internal void registerView(IGPXView gpxView)
        {
            if (!views.Contains(gpxView))
            {
                gpxView.Bind(gpxFile);
                views.Add(gpxView);

                gpxView.ChangedData += new EventHandler(gpxView_Changed);
                gpxView.ChangedLocation += new EventHandler(gpxView_ChangedLocation);

                //gpxMapView.TrkAdded += new TrkEventArg(gpxMapView_TrkAdded);
                //gpxMapView.WptSelected += new WptSelectEventArg(gpxMapView_WptSelected);

                //gpxMapView.UndoRecordBegin += new EventHandler(gpxMapView_UndoRecordBegin);
                //gpxMapView.UndoRecordCommit += new EventHandler(gpxMapView_UndoRecordCommit);



            }
            else
            {
                throw new Exception("View already exists.");
            }
        }

        void gpxView_ChangedLocation(object sender, EventArgs e)
        {
            Debug.Assert((sender as IGPXView) != null);
            EmitNewLocationToAllViewExcept((sender as IGPXView));
        }

        void gpxView_Changed(object sender, EventArgs e)
        {
            Debug.Assert((sender as IGPXView) != null);
            EmitRepaintToAllViewExcept((sender as IGPXView));
        }

        /// <summary>
        /// Send Repaint notification to all views
        /// </summary>
        /// <param name="view">view can be null to make no exception</param>
        void EmitRepaintToAllViewExcept(IGPXView viewSkip)
        {
            foreach (IGPXView view in views)
            {
                if (view != viewSkip)
                {
                    view.Repaint();
                }
            }
        }

        void EmitNewLocationToAllViewExcept(IGPXView viewSkip)
        {
            foreach (IGPXView view in views)
            {
                if (view != viewSkip)
                {
                    view.NewLocation();
                }
            }
        }


/*
        void gpxMapView_UndoRecordCommit(object sender, EventArgs e)
        {
            Debug.Assert((sender as IGPXView) != null);
            foreach (IGPXView view in views)
            {
                if (view != sender)
                {
                    view.UndoCommit();
                }
            }
        }

        void gpxMapView_UndoRecordBegin(object sender, EventArgs e)
        {
            Debug.Assert((sender as IGPXView) != null);
            foreach (IGPXView view in views)
            {
                if (view != sender)
                {
                    view.UndoBegin();
                }
            }
        }

        void gpxMapView_WptSelected(object sender, GpxWpt wpt, bool selected)
        {
            Debug.Assert((sender as IGPXView) != null);
            foreach (IGPXView view in views)
            {
                if (view != sender)
                {
                    view.WptSelect(wpt, selected);
                }
            }
        }

        void gpxMapView_TrkAdded(object sender, GPXTrk trk)
        {
            


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">view that generated event</param>
        /// <param name="e"></param>
        void gpxMapView_WptAdded(object sender, GPXTrk trk)
        {
            // new trk added by one of views
            // update model and other views

            // 

        }
        */



        internal void LoadAndAppend(string fileName)
        {
            // load into Model
            GPXUtils.AppendGPX(gpxFile, fileName);
            lastGpxFilename = fileName;

            EmitRepaintToAllViewExcept(null);
        }

        internal void inverseSelection()
        {
            foreach (GPXTrk trk in gpxFile.trks)
            {
                trk.selected = true;
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    bool allPointsSelected = true;
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        wpt.selected = !wpt.selected;
                        if (wpt.selected == false) allPointsSelected = false;
                    }
                    if (allPointsSelected)
                    {
                        seg.selected = true;
                    }
                }
                trk.selected = !trk.selected;
            }
            EmitRepaintToAllViewExcept(null);
        }

        internal void selectAll()
        {
            foreach (GPXTrk trk in gpxFile.trks)
            {
                trk.selected = true;
                foreach (GPXTrkSeg seg in trk.trkSeg)
                {
                    seg.selected = true;
                    foreach (GpxWpt wpt in seg.wpts)
                    {
                        wpt.selected = true;
                    }
                }
            }
            EmitRepaintToAllViewExcept(null);
        }

        internal void Save(string fileName)
        {
            // save to disk *.gpx file
            GPXUtils.SaveGPXv11(gpxFile, fileName);
        }

        internal void ClearAll()
        {
            while (gpxFile.trks.Count > 0)
                gpxFile.trks.RemoveAt(0);

            EmitRepaintToAllViewExcept(null);
        }
    }
}
