namespace RTPClient_Trial
{
    partial class RTPClientView
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
            this.PortLabel = new System.Windows.Forms.Label();
            this.ConnectPortTextBox = new System.Windows.Forms.TextBox();
            this.ServerIPLabel = new System.Windows.Forms.Label();
            this.ServerIPAddress = new System.Windows.Forms.TextBox();
            this.ServerResponseLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.VideoScreen = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.setupButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.teardownButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.video1 = new VideoPlayer.Video();
            this.videoName = new System.Windows.Forms.ComboBox();
            this.packetReportCheckBox = new System.Windows.Forms.CheckBox();
            this.printHeaderCheckBox = new System.Windows.Forms.CheckBox();
            this.ServerResponseTextBox = new UnEditableRichTextBox.UERichTextBox();
            this.ClientStateTextBox = new UnEditableRichTextBox.UERichTextBox();
            this.VideoScreen.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(12, 9);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(84, 13);
            this.PortLabel.TabIndex = 0;
            this.PortLabel.Text = "Connect to Port:";
            // 
            // ConnectPortTextBox
            // 
            this.ConnectPortTextBox.Location = new System.Drawing.Point(102, 6);
            this.ConnectPortTextBox.Name = "ConnectPortTextBox";
            this.ConnectPortTextBox.Size = new System.Drawing.Size(75, 20);
            this.ConnectPortTextBox.TabIndex = 1;
            // 
            // ServerIPLabel
            // 
            this.ServerIPLabel.AutoSize = true;
            this.ServerIPLabel.Location = new System.Drawing.Point(183, 9);
            this.ServerIPLabel.Name = "ServerIPLabel";
            this.ServerIPLabel.Size = new System.Drawing.Size(95, 13);
            this.ServerIPLabel.TabIndex = 2;
            this.ServerIPLabel.Text = "Server IP Address:";
            // 
            // ServerIPAddress
            // 
            this.ServerIPAddress.Location = new System.Drawing.Point(284, 6);
            this.ServerIPAddress.Name = "ServerIPAddress";
            this.ServerIPAddress.Size = new System.Drawing.Size(89, 20);
            this.ServerIPAddress.TabIndex = 3;
            // 
            // ServerResponseLabel
            // 
            this.ServerResponseLabel.AutoSize = true;
            this.ServerResponseLabel.Location = new System.Drawing.Point(12, 345);
            this.ServerResponseLabel.Name = "ServerResponseLabel";
            this.ServerResponseLabel.Size = new System.Drawing.Size(92, 13);
            this.ServerResponseLabel.TabIndex = 9;
            this.ServerResponseLabel.Text = "Server Response:";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(491, 448);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 11;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // VideoScreen
            // 
            this.VideoScreen.Controls.Add(this.panel1);
            this.VideoScreen.Controls.Add(this.video1);
            this.VideoScreen.Location = new System.Drawing.Point(12, 25);
            this.VideoScreen.Name = "VideoScreen";
            this.VideoScreen.Size = new System.Drawing.Size(554, 218);
            this.VideoScreen.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.setupButton);
            this.panel1.Controls.Add(this.playButton);
            this.panel1.Controls.Add(this.teardownButton);
            this.panel1.Controls.Add(this.pauseButton);
            this.panel1.Location = new System.Drawing.Point(76, 179);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 29);
            this.panel1.TabIndex = 17;
            // 
            // setupButton
            // 
            this.setupButton.Location = new System.Drawing.Point(8, 3);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(75, 23);
            this.setupButton.TabIndex = 1;
            this.setupButton.Text = "SETUP";
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.setupButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(89, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 2;
            this.playButton.Text = "PLAY";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // teardownButton
            // 
            this.teardownButton.Location = new System.Drawing.Point(251, 3);
            this.teardownButton.Name = "teardownButton";
            this.teardownButton.Size = new System.Drawing.Size(84, 23);
            this.teardownButton.TabIndex = 4;
            this.teardownButton.Text = "TEARDOWN";
            this.teardownButton.UseVisualStyleBackColor = true;
            this.teardownButton.Click += new System.EventHandler(this.teardownButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(170, 3);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.Text = "PAUSE";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // video1
            // 
            this.video1.AutoSize = true;
            this.video1.Location = new System.Drawing.Point(-2, 8);
            this.video1.Name = "video1";
            this.video1.Size = new System.Drawing.Size(555, 173);
            this.video1.TabIndex = 5;
            // 
            // videoName
            // 
            this.videoName.FormattingEnabled = true;
            this.videoName.Location = new System.Drawing.Point(392, 6);
            this.videoName.Name = "videoName";
            this.videoName.Size = new System.Drawing.Size(171, 21);
            this.videoName.TabIndex = 14;
            // 
            // packetReportCheckBox
            // 
            this.packetReportCheckBox.AutoSize = true;
            this.packetReportCheckBox.Location = new System.Drawing.Point(480, 241);
            this.packetReportCheckBox.Name = "packetReportCheckBox";
            this.packetReportCheckBox.Size = new System.Drawing.Size(95, 17);
            this.packetReportCheckBox.TabIndex = 15;
            this.packetReportCheckBox.Text = "Packet Report";
            this.packetReportCheckBox.UseVisualStyleBackColor = true;
            // 
            // printHeaderCheckBox
            // 
            this.printHeaderCheckBox.AutoSize = true;
            this.printHeaderCheckBox.Location = new System.Drawing.Point(480, 277);
            this.printHeaderCheckBox.Name = "printHeaderCheckBox";
            this.printHeaderCheckBox.Size = new System.Drawing.Size(85, 17);
            this.printHeaderCheckBox.TabIndex = 16;
            this.printHeaderCheckBox.Text = "Print Header";
            this.printHeaderCheckBox.UseVisualStyleBackColor = true;
            // 
            // ServerResponseTextBox
            // 
            this.ServerResponseTextBox.Location = new System.Drawing.Point(12, 361);
            this.ServerResponseTextBox.Name = "ServerResponseTextBox";
            this.ServerResponseTextBox.Size = new System.Drawing.Size(462, 110);
            this.ServerResponseTextBox.TabIndex = 13;
            this.ServerResponseTextBox.Text = "";
            // 
            // ClientStateTextBox
            // 
            this.ClientStateTextBox.Location = new System.Drawing.Point(12, 239);
            this.ClientStateTextBox.Name = "ClientStateTextBox";
            this.ClientStateTextBox.Size = new System.Drawing.Size(462, 103);
            this.ClientStateTextBox.TabIndex = 12;
            this.ClientStateTextBox.Text = "";
            // 
            // RTPClientView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 483);
            this.Controls.Add(this.printHeaderCheckBox);
            this.Controls.Add(this.packetReportCheckBox);
            this.Controls.Add(this.videoName);
            this.Controls.Add(this.ServerResponseTextBox);
            this.Controls.Add(this.ClientStateTextBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ServerResponseLabel);
            this.Controls.Add(this.VideoScreen);
            this.Controls.Add(this.ServerIPAddress);
            this.Controls.Add(this.ServerIPLabel);
            this.Controls.Add(this.ConnectPortTextBox);
            this.Controls.Add(this.PortLabel);
            this.Name = "RTPClientView";
            this.Text = "RTP Client Trial";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTPClientView_FormClosing);
            this.VideoScreen.ResumeLayout(false);
            this.VideoScreen.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox ConnectPortTextBox;
        private System.Windows.Forms.Label ServerIPLabel;
        private System.Windows.Forms.TextBox ServerIPAddress;
        private System.Windows.Forms.Label ServerResponseLabel;
        private System.Windows.Forms.Button ConnectButton;
        private UnEditableRichTextBox.UERichTextBox ClientStateTextBox;
        private UnEditableRichTextBox.UERichTextBox ServerResponseTextBox;
        private System.Windows.Forms.Panel VideoScreen;
        private System.Windows.Forms.Button teardownButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.ComboBox videoName;
        private VideoPlayer.Video video1;
        private System.Windows.Forms.CheckBox packetReportCheckBox;
        private System.Windows.Forms.CheckBox printHeaderCheckBox;
        private System.Windows.Forms.Panel panel1;
    }
}

