namespace RTPServer_Trial
{
    partial class RTPServerMainView
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
            this.PortNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listenButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ServerIPAddress = new System.Windows.Forms.Label();
            this.FrameNumber = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ServerStatus = new UnEditableRichTextBox.UERichTextBox();
            this.ClientRequestTextBox = new UnEditableRichTextBox.UERichTextBox();
            this.SuspendLayout();
            // 
            // PortNumber
            // 
            this.PortNumber.Location = new System.Drawing.Point(118, 14);
            this.PortNumber.MaximumSize = new System.Drawing.Size(100, 4);
            this.PortNumber.MinimumSize = new System.Drawing.Size(100, 4);
            this.PortNumber.Name = "PortNumber";
            this.PortNumber.Size = new System.Drawing.Size(100, 20);
            this.PortNumber.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Listen on Port:";
            // 
            // listenButton
            // 
            this.listenButton.Location = new System.Drawing.Point(228, 12);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(75, 23);
            this.listenButton.TabIndex = 2;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.listenButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server IP Address:";
            // 
            // ServerIPAddress
            // 
            this.ServerIPAddress.AutoSize = true;
            this.ServerIPAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ServerIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerIPAddress.Location = new System.Drawing.Point(118, 47);
            this.ServerIPAddress.MinimumSize = new System.Drawing.Size(60, 0);
            this.ServerIPAddress.Name = "ServerIPAddress";
            this.ServerIPAddress.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ServerIPAddress.Size = new System.Drawing.Size(60, 15);
            this.ServerIPAddress.TabIndex = 4;
            // 
            // FrameNumber
            // 
            this.FrameNumber.AutoSize = true;
            this.FrameNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FrameNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrameNumber.Location = new System.Drawing.Point(429, 47);
            this.FrameNumber.MaximumSize = new System.Drawing.Size(100, 0);
            this.FrameNumber.MinimumSize = new System.Drawing.Size(50, 0);
            this.FrameNumber.Name = "FrameNumber";
            this.FrameNumber.Size = new System.Drawing.Size(50, 15);
            this.FrameNumber.TabIndex = 5;
            this.FrameNumber.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(377, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Frame #";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(380, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(85, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Print Header";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 265);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Client Requests";
            // 
            // ServerStatus
            // 
            this.ServerStatus.Location = new System.Drawing.Point(18, 63);
            this.ServerStatus.Name = "ServerStatus";
            this.ServerStatus.Size = new System.Drawing.Size(461, 199);
            this.ServerStatus.TabIndex = 11;
            this.ServerStatus.Text = "";
            // 
            // ClientRequestTextBox
            // 
            this.ClientRequestTextBox.Location = new System.Drawing.Point(15, 281);
            this.ClientRequestTextBox.Name = "ClientRequestTextBox";
            this.ClientRequestTextBox.Size = new System.Drawing.Size(464, 182);
            this.ClientRequestTextBox.TabIndex = 12;
            this.ClientRequestTextBox.Text = "";
            // 
            // RTPServerMainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 475);
            this.Controls.Add(this.ClientRequestTextBox);
            this.Controls.Add(this.ServerStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FrameNumber);
            this.Controls.Add(this.ServerIPAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listenButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PortNumber);
            this.MinimizeBox = false;
            this.Name = "RTPServerMainView";
            this.Text = "RTP Server Trial";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RTPServerMainView_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PortNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ServerIPAddress;
        private System.Windows.Forms.Label FrameNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label6;
        private UnEditableRichTextBox.UERichTextBox ServerStatus;
        private UnEditableRichTextBox.UERichTextBox ClientRequestTextBox;
    }
}

