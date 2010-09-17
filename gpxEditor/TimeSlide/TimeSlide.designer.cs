namespace TimeSlideControl
{
    partial class TimeSlide
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TimeSlide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TimeSlide";
            this.Size = new System.Drawing.Size(608, 195);
            this.Load += new System.EventHandler(this.TimeSlide_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimeSlide_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeSlide_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimeSlide_MouseDown);
            this.Resize += new System.EventHandler(this.TimeSlide_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimeSlide_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
