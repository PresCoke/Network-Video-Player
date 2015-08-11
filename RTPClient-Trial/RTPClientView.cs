using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoPlayer;


namespace RTPClient_Trial
{
    public partial class RTPClientView : Form
    {
        //delegate for adding text to clientStatusTextBox
        public delegate void clntStTxtBx(string msg);
        public clntStTxtBx changeClientStatusTextBox;
        //delegate for adding text to ServerResponseText
        public delegate void srvRsTxtBx(string msg);
        public srvRsTxtBx changeServerResponseTextBox;
        //delegate for adding movies to slect movie box
        public delegate void addMovieToTextBoxt(string movie);
        public addMovieToTextBoxt addMovie;
        //delegate for adding image to video player
        public delegate void addFrameToVideoPlayer(Bitmap bmp);
        public addFrameToVideoPlayer addFrame;
        //delegate for starting video player
        public delegate void startMoviePlayBack();
        public startMoviePlayBack startMovie;
        //delegate for pausing video player
        public delegate void pauseMoviePlayBack();
        public pauseMoviePlayBack pauseMovie;
        //delegate for tearing down videoplayer
        public delegate void teardownMovie();
        public teardownMovie tearDownV;
        //delegate for getting dimensions of video player
        public delegate int[] getDimensions();
        public getDimensions getVideoDimensions;
        //delegeate for checking the print header checkbox
        public delegate bool headerCheckBox();
        public headerCheckBox checkHeaderCheckBox;
        //delegate for checking print packet report check box
        public delegate bool reportCheckBox();
        public reportCheckBox checkReportCheckBox;
        //delegate for making control panel visible
        public delegate void makeVisible(bool isVisible);
        public makeVisible makeControlsVisible;

        private bool connected;
        private ClientController control;

        public RTPClientView()
        {
            InitializeComponent();

            //set changeClientStatusTextBox to call writeToClientStateTextBox method
            changeClientStatusTextBox = new clntStTxtBx(writeToClientStateTextBox);
            //set changeClientServerResponseTextBox to call writeToServerResponseTextBox method
            changeServerResponseTextBox = new srvRsTxtBx(writeToServerResponseTextBox);
            //attaching delegates to methods
            addMovie = new addMovieToTextBoxt(addMovieToSelectableTextBox);
            addFrame = new addFrameToVideoPlayer(addFrameToMovie);
            startMovie = new startMoviePlayBack(commencePlayBack);
            pauseMovie = new pauseMoviePlayBack(stopPlayBack);
            tearDownV = new teardownMovie(tearDown);
            checkHeaderCheckBox = new headerCheckBox(checkPrintHeaderCheckBox);
            checkReportCheckBox = new reportCheckBox(checkPacketReportCheckBox);
            getVideoDimensions = new getDimensions(getVideoPlayerDimensions);
            makeControlsVisible = new makeVisible(makePanel1Visible);
            //tell view client started
            writeToClientStateTextBox("Client Started.");
            //initialize controller
            control = new ClientController(this);
            connected = false;
            //initially hide controls
            panel1.Visible = false;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            /*Pre: user presses connect button*/
            /*Post:user connected to server or error written to client state textbox*/
            try
            {
                //if either IP address or port number text boxes are blank
                if (ServerIPAddress.Text == null || ConnectPortTextBox.Text == null)
                {
                    //tell user to fill in all fields
                    MessageBox.Show("Please enter text for all fields.");
                }
                else if (connected == true)
                {
                    writeToClientStateTextBox("Already Connected.");
                }
                else if (ServerIPAddress.Text != null && ConnectPortTextBox != null && connected == false)
                {
                    //otherwise
                    try
                    {
                        //to parse contents of IP address text box into IPAddress type
                        System.Net.IPAddress servIP = System.Net.IPAddress.Parse(ServerIPAddress.Text);
                        //to parse contents of port text box into 16 bit integer
                        Int16 port = Int16.Parse(ConnectPortTextBox.Text);
                        connected = true;
                        //attempt to connect to the server with the supplied port number and IP address
                        control.connectToServer(servIP, port);
                        
                    }
                    catch (FormatException fe)
                    {
                        //if either of the ip address or port number failed to parse tell user
                        writeToClientStateTextBox("Format exception: " + fe.ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                //catch any other exception and tell user
                ClientStateTextBox.Text += exc.ToString() + "\n";
            }
        }

        //writes to client state text box and appends new line character
        private void writeToClientStateTextBox(string message)
        {
            this.ClientStateTextBox.Text += message + "\n";
        }
        //writes to server response text box and appends new line character
        private void writeToServerResponseTextBox(string message)
        {
            this.ServerResponseTextBox.Text += message + "\n";
        }
        //adds movie to selectable text box and automatically makes it selected
        private void addMovieToSelectableTextBox(string movie)
        {
            this.videoName.Items.Add(movie);
            this.videoName.SelectedItem = movie;
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            //call setup method
            string selected;
            selected = (string) videoName.SelectedItem;
            if (selected != null)
                control.setup(selected);
            else
                writeToClientStateTextBox("Please select a movie.");
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            //call play method
            control.play();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            //call control method
            control.pause();
        }

        private void teardownButton_Click(object sender, EventArgs e)
        {
            //call teardown method
            control.teardown();
        }

        public void setConnected(bool value)
        {
            connected = value;
        }

        //facades for talking to videoPlayer
        private void addFrameToMovie(Bitmap bmp)
        {
            this.video1.addBitmapToEnd(bmp);
        }
        private void stopPlayBack()
        {
            this.video1.pausePlayBack();
        }
        private void commencePlayBack()
        {
            this.video1.startPlayBack();
        }
        private void tearDown()
        {
            this.video1.teardownVideo();
        }
        private int[] getVideoPlayerDimensions()
        {
            int[] dim = new int[2];
            dim[0] = this.video1.Height;
            dim[1] = this.video1.Width;
            return dim;
        }

        //returns status of check boxes
        private bool checkPrintHeaderCheckBox()
        {
            return this.printHeaderCheckBox.Checked;
        }
        private bool checkPacketReportCheckBox()
        {
            return this.packetReportCheckBox.Checked;
        }
        //on close call control.close
        private void RTPClientView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.control.close();
        }
        //makes controls visible
        private void makePanel1Visible(bool isVisible)
        {
            panel1.Visible = isVisible;
        }
    }
}
