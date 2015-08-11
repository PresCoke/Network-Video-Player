using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace RTPServer_Trial
{
    public partial class RTPServerMainView : Form
    {
        //delegate for writing to server status textbox
        public delegate void changeServerStatus(String msg);
        public changeServerStatus changeServerStatusTextBox;
        
        //delegate for writing to client request textbox
        public delegate void changeClientRequest(String msg);
        public changeClientRequest changeClientRequestTextBox;

        //delegate for checking the status of the printHeaderCheckBox
        public delegate bool checkPrintHeaderStatus();
        public checkPrintHeaderStatus checkHeader;

        //delegate for changing the frameNumberTextBox
        public delegate void changeFrameNumberTextBox(int number);
        public changeFrameNumberTextBox changeFrameNumber;

        //mutex's to avoid multiple threads accessing either textboxes at once
        private static System.Threading.Mutex srvTxtBx = new System.Threading.Mutex();
        private static System.Threading.Mutex cltRqTxt = new System.Threading.Mutex();

        //tracks if server is listening
        private bool listening;

        //an instance of the server controller
        private StreamingServer listeningServer;

        public RTPServerMainView()
        {
            InitializeComponent();
            
            //attach delegates to methods
            changeServerStatusTextBox = new changeServerStatus(writeToServerTextBox);
            changeClientRequestTextBox = new changeClientRequest(writeToClientRequestTextBox);
            checkHeader = new checkPrintHeaderStatus(checkPrintHeader);
            changeFrameNumber = new changeFrameNumberTextBox(changeFrameTextBox);

            //passing a reference of the view to the server controller
            listeningServer = new StreamingServer(this);


            listening = false;
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            /*Pre: User clicks "listen" button*/
            /*Post:if text inside portnumbertextbox was valid (i.e. not null and 16bit integer) a new thread is created to listen
             *port provided and accept incoming connection attempts. If null MessageBox displayed informing user to enter a value,
             *if not valid format exception written to serverstatus textbox.
             */
            //if there is text in port number box
            //get IP address of server;
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }

            this.ServerIPAddress.Text = localIP;

            if (PortNumber.Text != null && listening == false)
            {
                try
                {
                    //parse text in port number txtbox to 16 bit integer
                    Int16 portNumber = Int16.Parse(PortNumber.Text);
                    //if successful call listenOnPort method to create new thread to listen for incoming connections
                    ServerStatus.Text += listeningServer.listenOnPort(portNumber) + "\n";
                    listening = true;
                }
                catch (FormatException fe)
                {
                    //if text was not 16 bit integer write to main view
                    writeToServerTextBox("Format exception in MainView: " + fe.ToString());
                }
            }
            else if (listening == true)
                writeToServerTextBox("Already Listening.");
            //if nothing in port number textbox display message box 
            else
                MessageBox.Show("Please enter a port number");
        }

        //method for writing to the servertextbox and adding a newline after every string
        //for some reason newline not added when called in RTSPClient
        private void writeToServerTextBox(string message)
        {
            /*Pre: A thread needs to write to the ServerTextBox and calls this method through a delegate.*/
            /*Post:The supplied text in the parameter message is written to ServerTextBox and a newline charcter is appeneded.*/
            //lock resource
            srvTxtBx.WaitOne();
            this.ServerStatus.Text += message + "\n";
            //unlock resource when done
            srvTxtBx.ReleaseMutex();
        }

        //method for writing to client request textbox has same problem as above method
        private void writeToClientRequestTextBox(string message)
        {
            /*Pre: A thread needs to write to the ClientRequestTextBox and calls this method through a delegate.*/
            /*Post:The supplied text in the parameter message is written to ClientRequestTextBox and a newline charcter is appeneded.*/
            //lock resource
            cltRqTxt.WaitOne();
            this.ClientRequestTextBox.Text += message + "\n";
            //unlock resource when done
            cltRqTxt.ReleaseMutex();
        }

        private bool checkPrintHeader()
        {
            /*Pre : client needs to determine if printheaderCheckBox has been checked
             *Post: the state of the check box is returned*/
            return this.checkBox1.Checked;
        }
        private void changeFrameTextBox(int number)
        {
            /*Pre : client needs to adjust the frame number text box
             *Post: the frame text box now shows the number supplied*/
            this.FrameNumber.Text = number.ToString();
        }

        private void RTPServerMainView_FormClosed(object sender, FormClosedEventArgs e)
        {
            //listeningServer.CloseThreads();
            listeningServer.CloseSocket();
            this.Dispose(true);
        }
    }
}
