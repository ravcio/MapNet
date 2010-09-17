using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeSlideControl
{
    public partial class TimeSlide : UserControl
    {
        public TimeSlide()
        {
            InitializeComponent();
        }

        public event EventHandler MarkerChanging;

        Bitmap bt_bg = null;
        Graphics g_bg = null;

        bool bg_dirty = true;

        private void TimeSlide_Paint(object sender, PaintEventArgs e)
        {
            if (bt_bg == null)
            {
                // make double buffer
                bt_bg = new Bitmap(Width * 3, Height, e.Graphics);
                g_bg = Graphics.FromImage(bt_bg);
                g_bg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g_bg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            }

            if (bg_dirty)
            {
                g_bg.FillRectangle(new SolidBrush(this.BackColor), 0, 0, Width, Height);
                //g_bg.FillRectangle(new SolidBrush(Color.Red), 0, 0, Width, Height);

                DrawTimeRuler(g_bg, 50);
                DrawData(g_bg, 50);
                bg_dirty = false;
            }

            e.Graphics.DrawImageUnscaled(bt_bg, 0, 0);
            DrawTimeMarker(e.Graphics, 50);
        }


        void DrawData(Graphics g, int yPos)
        {
            // Paint Dataset
            if (plotValuesBlack != null && !movingRuler && !movingMarker)
            {
                for (int i = 0; i < plotValuesBlack.Count; i++)
                {
                    double x = locate(plotValuesBlack[i].Key);
                    double y = this.Height - yPos - plotValuesBlack[i].Value;

                    g.DrawRectangle(Pens.Black, (float)x, (float)y, (float)1, (float)1);
                }
                for (int i = 0; i < plotValuesRed.Count; i++)
                {
                    double x = locate(plotValuesRed[i].Key);
                    double y = this.Height - yPos - plotValuesRed[i].Value;

                    g.DrawRectangle(Pens.Red, (float)x, (float)y, (float)1, (float)1);
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="y">Pixel above bottom line</param>
        void DrawTimeRuler(Graphics g, int y)
        {
            int hourWidth = 120; // pixel width of 1 hour
            TimeSpan timeOfDay = valueStart.TimeOfDay; // remove day info (leave time)
            double startSecond = timeOfDay.TotalSeconds;
            double startHour = timeOfDay.TotalHours;
            int hour = (int)Math.Floor(startHour);
            int leftMargin = -(int)Math.Floor((startHour - hour) * hourWidth);

            int height = this.Height - y;


            int maxJ = this.Width / hourWidth + 2;
            for (int j = 0; j < maxJ; j++)
            {
                if (hour % 2 == 0)
                    g.FillRectangle(Brushes.LightGray, leftMargin + j * 120, height + 20, hourWidth, 10);
                else
                    g.FillRectangle(Brushes.DarkGray, leftMargin + j * 120, height + 20, hourWidth, 10);

                for (int i = 0; i < 12; i++)
                {
                    g.FillRectangle(Brushes.LightGray, leftMargin + j * hourWidth + i * 20, height + 10, 10, 10);
                    g.FillRectangle(Brushes.DarkGray, leftMargin + j * hourWidth + i * 20 + 10, height + 10, 10, 10);
                }

                Font font = new Font("Arial", 8);

                string text = hour.ToString();
                SizeF size = g.MeasureString(text, font);
                g.DrawString(text, font, Brushes.Black, leftMargin + j * hourWidth - size.Width / 2, height + 30);
                hour++;
                if (hour > 24) hour = 1;
            }

            g.DrawLine(Pens.Black, leftMargin, height + 10, this.Width, height + 10);

            Pen p = new Pen(Color.Black, 1);
            float[] f = { 1, 1 };
            p.DashPattern = f;
            g.DrawLine(p, leftMargin, height + 9, this.Width, height + 9);

            float[] f1 = { 1, 29 };
            p.DashPattern = f1;
            g.DrawLine(p, leftMargin, height + 8, this.Width, height + 8);

            float[] f2 = { 1, 119 };
            p.DashPattern = f2;
            g.DrawLine(p, leftMargin, height + 7, this.Width, height + 7);
            g.DrawLine(p, leftMargin, height + 6, this.Width, height + 6);
        }

        void DrawTimeMarker(Graphics g, int y)
        {
            int hourWidth = 120; // pixel width of 1 hour
            TimeSpan timeOfDay = ValueStart.TimeOfDay; // remove day info (leave time)
            double startSecond = timeOfDay.TotalSeconds;
            double startHour = timeOfDay.TotalHours;
            int hour = (int)Math.Floor(startHour);
            int leftMargin = -(int)Math.Floor((startHour - hour) * hourWidth);
            int height = this.Height - y;

            // Paint Marker (on top)
            if (valueMarker > DateTime.MinValue)
            {
                TimeSpan markerPos = valueMarker - valueStart;
                float position = (float)getXPositionFromDate(valueMarker);

                Brush br = new SolidBrush(Color.FromArgb(128, 255, 192, 192));
                g.FillRectangle(br, position - 10, 0, 20, this.Height);

                g.DrawLine(Pens.Red, position, 0, position, this.Height);
            }
        }

        double getXPositionFromDate(DateTime time)
        {
            int hourWidth = 120; // pixel width of 1 hour
            TimeSpan ts = time - valueStart;
            return ts.TotalHours * hourWidth;
        }

        DateTime getDateFromXPosition(double x)
        {
            int hourWidth = 120; // pixel width of 1 hour

            DateTime val = valueStart + new TimeSpan(0, 0, (int)((double)x / hourWidth * 60.0 * 60));
            return val;
        }


        double locate(DateTime t)
        {
            TimeSpan ts = t - valueStart;
            double x = ts.TotalHours * 120;
            return x;
        }

        private void TimeSlide_Load(object sender, EventArgs e)
        {
            DateTime t = new DateTime(1000);
            this.Cursor = Cursors.Hand;
        }

        DateTime valueStart;
        public DateTime ValueStart
        {
            get { return this.valueStart; }
            set
            {
                this.valueStart = value;
                EnsureMarkerVisibleMoveMarker();

                bg_dirty = true;
                this.Invalidate();
            }
        }

        DateTime valueMarker;

        public DateTime ValueMarker
        {
            get { return valueMarker; }
            set
            {
                valueMarker = value;
                // scroll ruler to make marker visible
                EnsureMarkerVisibleMoveRuler();

                this.Invalidate();
            }
        }


        private void TimeSlide_Resize(object sender, EventArgs e)
        {
            bt_bg = null;
            g_bg = null;
            bg_dirty = true;

            this.Invalidate();
        }


        List<KeyValuePair<DateTime, double>> plotValuesBlack = new List<KeyValuePair<DateTime, double>>();
        public List<KeyValuePair<DateTime, double>> PlotValuesBlack
        {
            get 
            {
                bg_dirty = true;
                return plotValuesBlack;
            }
        }

        List<KeyValuePair<DateTime, double>> plotValuesRed = new List<KeyValuePair<DateTime, double>>();
        public List<KeyValuePair<DateTime, double>> PlotValuesRed
        {
            get 
            {
                bg_dirty = true;
                return plotValuesRed;
            }
        }


        private void TimeSlide_MouseMove(object sender, MouseEventArgs e)
        {
            if (movingMarker)
            {
                valueMarker = getDateFromXPosition(e.X - grabPoint);
                this.Invalidate();
                MarkerChanging(this, new EventArgs());
                return;
            }
            else if (movingRuler)
            {
                int hourWidth = 120; // pixel width of 1 hour
                int x = e.X - grabPoint;
                ValueStart = valueStartOryg - new TimeSpan(0, (int)((double)x / hourWidth * 60.0), 0);

                // move marker if out of view
                EnsureMarkerVisibleMoveMarker();

                this.Invalidate();
                return;
            }

            if (e.Button != MouseButtons.None) return;

            if (overMarker(e.X))
            {
                this.Cursor = Cursors.SizeWE;
            }
            else
            {
                this.Cursor = Cursors.Hand;
            }
        }

        private void EnsureMarkerVisibleMoveMarker()
        {
            double xMarker = getXPositionFromDate(valueMarker);
            if (xMarker < 10)
            {
                valueMarker = getDateFromXPosition(10);
                if (MarkerChanging != null)
                {
                    MarkerChanging(this, new EventArgs());
                }
            }
            else if (xMarker > this.Width - 10)
            {
                valueMarker = getDateFromXPosition(this.Width - 10);
                if (MarkerChanging != null)
                {
                    MarkerChanging(this, new EventArgs());
                }
            }
            this.Invalidate();
        }

        private void EnsureMarkerVisibleMoveRuler()
        {
            double xMarker = getXPositionFromDate(valueMarker);
            if (xMarker < 10 || xMarker > this.Width - 10)
            {
                ValueStart = valueMarker - new TimeSpan(2, 0, 0);
                if (MarkerChanging != null)
                {
                    MarkerChanging(this, new EventArgs());
                }
            }
            this.Invalidate();
        }


        bool movingMarker = false;
        bool movingRuler = false;
        int grabPoint;

        DateTime valueStartOryg;

        private void TimeSlide_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (overMarker(e.X))
                {
                    movingMarker = true;
                    grabPoint = e.X - (int)getXPositionFromDate(valueMarker);
                }
                else
                {
                    movingRuler = true;
                    valueStartOryg = valueStart;
                    grabPoint = e.X - (int)getXPositionFromDate(valueStart);
                }
            }
            this.Invalidate();
        }

        bool overMarker(int x)
        {
            TimeSpan markerPos = valueMarker - valueStart;
            float position = (float)getXPositionFromDate(valueMarker);

            return (Math.Abs(x - (int)position) < 10);
        }

        private void TimeSlide_MouseUp(object sender, MouseEventArgs e)
        {
            if (movingMarker)
            {
                valueMarker = getDateFromXPosition(e.X - grabPoint);
                this.Invalidate();
                if (MarkerChanging != null)
                {
                    MarkerChanging(this, new EventArgs());
                }
            }
            else if (movingRuler)
            {
                int hourWidth = 120; // pixel width of 1 hour
                int x = e.X - grabPoint;
                ValueStart = valueStartOryg - new TimeSpan(0, (int)((double)x / hourWidth * 60.0), 0);
                this.Invalidate();
            }
            movingMarker = false;
            movingRuler = false;
        }

    }
}
