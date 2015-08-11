namespace VideoPlayer
{
    partial class Video
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
            this.components = new System.ComponentModel.Container();
            this.videoPlayBack = new System.Windows.Forms.PictureBox();
            this.displayTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayBack)).BeginInit();
            this.SuspendLayout();
            // 
            // videoPlayBack
            // 
            this.videoPlayBack.BackColor = System.Drawing.SystemColors.ControlText;
            this.videoPlayBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.videoPlayBack.Location = new System.Drawing.Point(0, 0);
            this.videoPlayBack.Name = "videoPlayBack";
            this.videoPlayBack.Size = new System.Drawing.Size(550, 170);
            this.videoPlayBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.videoPlayBack.TabIndex = 0;
            this.videoPlayBack.TabStop = false;
            // 
            // displayTimer
            // 
            this.displayTimer.Interval = 150;
            this.displayTimer.Tick += new System.EventHandler(this.displayTimer_Tick);
            // 
            // Video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.videoPlayBack);
            this.Name = "Video";
            this.Size = new System.Drawing.Size(553, 173);
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayBack)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox videoPlayBack;
        private System.Windows.Forms.Timer displayTimer;
    }
}
