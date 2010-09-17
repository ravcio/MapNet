﻿
namespace GMap.NET.WindowsForms
{
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using System.Windows.Forms;
   using GMap.NET.ObjectModel;
   using System;

   /// <summary>
   /// GMap.NET overlay
   /// </summary>
   public class GMapOverlay
   {
      bool isVisibile = true;

      /// <summary>
      /// is overlay visible
      /// </summary>
      public bool IsVisibile
      {
         get
         {
            return isVisibile;
         }
         set
         {
            if(value != isVisibile)
            {
               isVisibile = value;
               if(isVisibile)
               {
                  Control.HoldInvalidation = true;
                  ForceUpdate();
                  Control.Refresh();
               }
               else
               {
                  if(!Control.HoldInvalidation)
                  {
                     Control.Invalidate();
                  }
               }
            }
         }
      }

      /// <summary>
      /// overlay Id
      /// </summary>
      public string Id;

      /// <summary>
      /// list of markers, should be thread safe
      /// </summary>
      public readonly ObservableCollectionThreadSafe<GMapMarker> Markers = new ObservableCollectionThreadSafe<GMapMarker>();

      /// <summary>
      /// list of routes, should be thread safe
      /// </summary>
      public readonly ObservableCollectionThreadSafe<GMapRoute> Routes = new ObservableCollectionThreadSafe<GMapRoute>();

      /// <summary>
      /// list of polygons, should be thread safe
      /// </summary>
      public readonly ObservableCollectionThreadSafe<GMapPolygon> Polygons = new ObservableCollectionThreadSafe<GMapPolygon>();

      internal GMapControl Control;

      public GMapOverlay(GMapControl control, string id)
      {
         if(control == null)
         {
            throw new Exception("GMapControl in GMapOverlay can't be null");
         }

         Control = control;
         Id = id;
         Markers.CollectionChanged += new NotifyCollectionChangedEventHandler(Markers_CollectionChanged);
         Routes.CollectionChanged += new NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
         Polygons.CollectionChanged += new NotifyCollectionChangedEventHandler(Polygons_CollectionChanged);
      }

      void Polygons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if(e.NewItems != null)
         {
            foreach(GMapPolygon obj in e.NewItems)
            {
               if(obj != null)
               {
                  obj.Overlay = this;
                  Control.UpdatePolygonLocalPosition(obj);
               }
            }
         }

         if(!Control.HoldInvalidation)
         {
            Control.Core_OnNeedInvalidation();
         }
      }

      void Routes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if(e.NewItems != null)
         {
            foreach(GMapRoute obj in e.NewItems)
            {
               if(obj != null)
               {
                  obj.Overlay = this;
                  Control.UpdateRouteLocalPosition(obj);
               }
            }
         }

         if(!Control.HoldInvalidation)
         {
            Control.Core_OnNeedInvalidation();
         }
      }

      void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if(e.NewItems != null)
         {
            foreach(GMapMarker obj in e.NewItems)
            {
               if(obj != null)
               {
                  obj.Overlay = this;
                  Control.UpdateMarkerLocalPosition(obj);
               }
            }
         }

         if(e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
         {
#if !PocketPC
            if(Control.IsMouseOverMarker)
            {
               Control.IsMouseOverMarker = false;
               Control.Cursor = Cursors.Default;
            }
#endif
         }

         if(!Control.HoldInvalidation)
         {
            Control.Core_OnNeedInvalidation();
         }
      }

      /// <summary>
      /// updates local positions of objects
      /// </summary>
      internal void ForceUpdate()
      {
         foreach(GMapMarker obj in Markers)
         {
            if(obj.IsVisible)
            {
               Control.UpdateMarkerLocalPosition(obj);
            }
         }

         foreach(GMapPolygon obj in Polygons)
         {
            if(obj.IsVisible)
            {
               Control.UpdatePolygonLocalPosition(obj);
            }
         }

         foreach(GMapRoute obj in Routes)
         {
            if(obj.IsVisible)
            {
               Control.UpdateRouteLocalPosition(obj);
            }
         }
      }

      /// <summary>
      /// draw routes, override to draw custom
      /// </summary>
      /// <param name="g"></param>
      protected virtual void DrawRoutes(Graphics g)
      {
#if !PocketPC
         foreach(GMapRoute r in Routes)
         {
            if(r.IsVisible)
            {
               using(GraphicsPath rp = new GraphicsPath())
               {
                  for(int i = 0; i < r.LocalPoints.Count; i++)
                  {
                     GMap.NET.Point p2 = r.LocalPoints[i];

                     if(i == 0)
                     {
                        rp.AddLine(p2.X, p2.Y, p2.X, p2.Y);
                     }
                     else
                     {
                        System.Drawing.PointF p = rp.GetLastPoint();
                        rp.AddLine(p.X, p.Y, p2.X, p2.Y);
                     }
                  }

                  if(rp.PointCount > 0)
                  {
                     g.DrawPath(r.Stroke, rp);
                  }
               }
            }
         }
#else
         foreach(GMapRoute r in Routes)
         {
            if(r.IsVisible)
            {
               Point[] pnts = new Point[r.LocalPoints.Count];
               for(int i = 0; i < r.LocalPoints.Count; i++)
               {
                  Point p2 = new Point(r.LocalPoints[i].X, r.LocalPoints[i].Y);
                  pnts[pnts.Length - 1 - i] = p2;
               }

               if(pnts.Length > 0)
               {
                  g.DrawLines(r.Stroke, pnts);
               }
            }
         }
#endif
      }

      /// <summary>
      /// draw polygons, override to draw custom
      /// </summary>
      /// <param name="g"></param>
      protected virtual void DrawPolygons(Graphics g)
      {
#if !PocketPC
         foreach(GMapPolygon r in Polygons)
         {
            if(r.IsVisible)
            {
               using(GraphicsPath rp = new GraphicsPath())
               {
                  for(int i = 0; i < r.LocalPoints.Count; i++)
                  {
                     GMap.NET.Point p2 = r.LocalPoints[i];

                     if(i == 0)
                     {
                        rp.AddLine(p2.X, p2.Y, p2.X, p2.Y);
                     }
                     else
                     {
                        System.Drawing.PointF p = rp.GetLastPoint();
                        rp.AddLine(p.X, p.Y, p2.X, p2.Y);
                     }
                  }

                  if(rp.PointCount > 0)
                  {
                     rp.CloseFigure();

                     g.FillPath(r.Fill, rp);

                     g.DrawPath(r.Stroke, rp);
                  }
               }
            }
         }
#else
         foreach(GMapPolygon r in Polygons)
         {
            if(r.IsVisible)
            {
               Point[] pnts = new Point[r.LocalPoints.Count];
               for(int i = 0; i < r.LocalPoints.Count; i++)
               {
                  Point p2 = new Point(r.LocalPoints[i].X, r.LocalPoints[i].Y);
                  pnts[pnts.Length - 1 - i] = p2;
               }

               if(pnts.Length > 0)
               {
                  g.FillPolygon(r.Fill, pnts);
                  g.DrawPolygon(r.Stroke, pnts);
               }
            }
         }
#endif
      }

      /// <summary>
      /// renders objects and routes
      /// </summary>
      /// <param name="g"></param>
      public virtual void Render(Graphics g)
      {
         if(Control != null)
         {
            if(Control.RoutesEnabled)
            {
               DrawRoutes(g);
            }

            if(Control.PolygonsEnabled)
            {
               DrawPolygons(g);
            }

            if(Control.MarkersEnabled)
            {
               // markers
               foreach(GMapMarker m in Markers)
               {
                  if(m.IsVisible && (m.DisableRegionCheck || Control.Core.CurrentRegion.Contains(m.LocalPosition.X, m.LocalPosition.Y)))
                  {
                     m.OnRender(g);
                  }
               }

               // tooltips above
               foreach(GMapMarker m in Markers)
               {
                  if(m.ToolTip != null && m.IsVisible && Control.Core.CurrentRegion.Contains(m.LocalPosition.X, m.LocalPosition.Y))
                  {
                     if(!string.IsNullOrEmpty(m.ToolTipText) && (m.ToolTipMode == MarkerTooltipMode.Always || (m.ToolTipMode == MarkerTooltipMode.OnMouseOver && m.IsMouseOver)))
                     {
                        m.ToolTip.Draw(g);
                     }
                  }
               }
            }
         }
      }
   }
}