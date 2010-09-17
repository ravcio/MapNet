namespace hiMapNet
{
    partial class MapControl
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
            // MapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.DoubleBuffered = true;
            this.Name = "MapControl";
            this.Load += new System.EventHandler(this.MapControl_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MapControl_MouseWheel);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapControl_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapControl_MouseMove);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MapControl_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapControl_MouseDown);
            this.Resize += new System.EventHandler(this.MapControl_Resize);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MapControl_KeyPress);
            this.MouseHover += new System.EventHandler(this.MapControl_MouseHover);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapControl_MouseUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapControl_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
