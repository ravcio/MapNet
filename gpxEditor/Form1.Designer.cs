namespace gpxEditor
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.timeSlide1 = new TimeSlideControl.TimeSlide();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timeSlide1
            // 
            this.timeSlide1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.timeSlide1.Location = new System.Drawing.Point(29, 46);
            this.timeSlide1.ValueMarker = new System.DateTime(((long)(0)));
            this.timeSlide1.Name = "timeSlide1";
            this.timeSlide1.Size = new System.Drawing.Size(694, 151);
            this.timeSlide1.TabIndex = 0;
            this.timeSlide1.ValueStart = new System.DateTime(2010, 8, 28, 11, 40, 49, 0);
            this.timeSlide1.Load += new System.EventHandler(this.timeSlide1_Load);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(648, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 347);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.timeSlide1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TimeSlideControl.TimeSlide timeSlide1;
        private System.Windows.Forms.Button button1;
    }
}