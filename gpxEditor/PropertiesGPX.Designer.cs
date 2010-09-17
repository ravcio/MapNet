namespace gpxEditor
{
    partial class PropertiesGPX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesGPX));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.txtTimeSpan = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File path:";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(125, 9);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(482, 20);
            this.txtFileName.TabIndex = 1;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdOK.Location = new System.Drawing.Point(532, 65);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Total length:";
            // 
            // txtLen
            // 
            this.txtLen.Location = new System.Drawing.Point(125, 42);
            this.txtLen.Name = "txtLen";
            this.txtLen.ReadOnly = true;
            this.txtLen.Size = new System.Drawing.Size(97, 20);
            this.txtLen.TabIndex = 2;
            // 
            // txtTimeSpan
            // 
            this.txtTimeSpan.Location = new System.Drawing.Point(125, 68);
            this.txtTimeSpan.Name = "txtTimeSpan";
            this.txtTimeSpan.ReadOnly = true;
            this.txtTimeSpan.Size = new System.Drawing.Size(97, 20);
            this.txtTimeSpan.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Time span:";
            // 
            // PropertiesGPX
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdOK;
            this.ClientSize = new System.Drawing.Size(626, 110);
            this.Controls.Add(this.txtTimeSpan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesGPX";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.PropertiesGPX_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLen;
        private System.Windows.Forms.TextBox txtTimeSpan;
        private System.Windows.Forms.Label label3;
    }
}