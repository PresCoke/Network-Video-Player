using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace RTPClient_Trial
{
    class ClientController
    {
        //constant for MegaByte
        private static int MB = 1024 * 1024;
        //streams
        private NetworkStream ntwkstrm;
        //to encode/decode messages
        private ASCIIEncoding encode;

        //socket for connection to server
        private Socket connected;
        //sent/recieved message buffers
        private byte[] inMessage;
        private byte[] outMessage;

        //reference to view
        private static RTPClientView referenceToView;

        //streams video
        private RTP_Protocol streamVideo;

        public ClientController(RTPClientView reference)
        {
            //initialize message buffers to be 1 MB
            inMessage = new byte[1 * MB];
            outMessage = new byte[1 * MB];
            //add reference to view
            referenceToView = reference;
            encode = new ASCIIEncoding();
        }

        public void connectToServer(IPAddress ip, Int16 portNumber)
        {
            /*Pre: attempts to connect to server with ipaddress and port number specified by user*/
            /*Post:client connects to server or error message written to view*/

            try
            {
                //remote end point
                IPEndPoint serverEndPoint = new IPEndPoint(ip, portNumber);
                //new tcp socket of stream type
                connected = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //attempt to connect to server
                connected.Connect(serverEndPoint);
                ntwkstrm = new NetworkStream(connected);
                //send greeting
                string message = "HELO";
                this.sendMessageToServer(message);
                //recieve message from server
                message = recieveMessageFromServer();
                int messageLength = message.Length;
                for (int i = 0; i < messageLength; i++)
                {
                    if (message[i] == ';')
                    {
                        for (int j = i + 1; j < messageLength; j++)
                        {
                            if (message[j] == ';')
                            {
                                try
                                {
                                    string videoName = "\0";
                                    char[] tempVideoName = new char[j - i];
                                    message.CopyTo(i + 1, tempVideoName, 0, (j - i - 1));
                                    videoName = tempVideoName[0].ToString();
                                    for (int k = 1; k < tempVideoName.Length - 1; k++)
                                        videoName += tempVideoName[k].ToString();
                                    referenceToView.Invoke(referenceToView.addMovie, videoName);
                                    j = messageLength;
                                }
                                catch (Exception e)
                                {
                                    
                                }
                            }
                        }
                    }
                    referenceToView.Invoke(referenceToView.makeControlsVisible, true);
                }
                //display message in server response text box
                referenceToView.Invoke(referenceToView.changeServerResponseTextBox, message);
            }
            catch (SocketException se)
            {
                //if socket connection fails
                //and socket is not null, close socket
                if (connected != null)
                    connected.Close();
                //close network stream if not closed already
                if (ntwkstrm != null)
                    ntwkstrm.Close();
                referenceToView.setConnected(false);
                //tell user why socket failed
                referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "Socket exception in connectToServer: "+se.ToString());
            }
            catch (Exception e)
            {
                //any other exception tell user why it failed
                referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "Exception in connectToServer: "+e.ToString());
            }
        }


        //may need to add some control strings such as: video name, etc.
        public void setup(string videoName)
        {
            /*Pre: user wants to setup video and presses setup button*/
            /*Post:string containing necessary control information for setting up video stream
             *passed to sendMessageToServer*/

            sendMessageToServer("SETUP;"+videoName);
            //recieve control message and write it to output
            string message = recieveMessageFromServer();
            if (message.Contains("201"))
            {
                int[] RTPinfo = new int [7];
                int counter = 0;
                int messageLength = message.Length;
                for (int i = 0; i < messageLength; i++)
                {
                    if (message[i] == ';')
                    {
                        for (int j = i + 1; j < messageLength; j++)
                        {
                            if (message[j] == ';')
                            {
                                try
                                {
                                    string tempNumber = "\0";
                                    char[] tempIntInfo = new char[j - i];
                                    message.CopyTo(i + 1, tempIntInfo, 0, (j - i - 1));
                                    tempNumber = tempIntInfo[0].ToString();
                                    for (int k = 1; k < tempIntInfo.Length - 1; k++)
                                        tempNumber += tempIntInfo[k].ToString();
                                    RTPinfo[counter] = Int32.Parse(tempNumber);
                                    i = j - 1;
                                    j = messageLength;
                                    counter++;
                                }
                                catch (Exception e)
                                {

                                }
                            }
                        }
                    }
                }
                streamVideo = new RTP_Protocol(RTPinfo);
                RTP_Protocol.addReferenceToView(referenceToView);
                streamVideo.setup(this.connected);
            }
            referenceToView.Invoke(referenceToView.changeServerResponseTextBox, message);
        }

        public void play()
        {
            /*Pre: user wants to play video*/
            /*Post:string containing necessary control information for playing video stream 
             *passed to sendMessageToServer*/

            sendMessageToServer("PLAY");
            //recieve control message and write it to output
            string message = recieveMessageFromServer();
            referenceToView.Invoke(referenceToView.changeServerResponseTextBox, message);
            referenceToView.Invoke(referenceToView.startMovie);
        }

        public void pause()
        {
            /*Pre: user wants to pause video*/
            /*Post:string containing necessary control information for pausing video stream
             *passed to sendMessageToServer*/

            sendMessageToServer("PAUSE");
            //recieve control message and write it to output
            string message = recieveMessageFromServer();
            referenceToView.Invoke(referenceToView.changeServerResponseTextBox, message);
            referenceToView.Invoke(referenceToView.pauseMovie);
        }

        public void teardown()
        {
            /*Pre: user wants to stop streaming video*/
            /*Post:string containing necessary control information for stopping video stream
             *passed to sendMessageToServer*/
            referenceToView.Invoke(referenceToView.tearDownV);
            sendMessageToServer("TEARDOWN");
            //recieve control message and write it to output
            string message = recieveMessageFromServer();
            referenceToView.Invoke(referenceToView.changeServerResponseTextBox, message);
        }

        public void close()
        {
            /*Pre : client closed window notify server
             *Post: server notified*/
            this.sendMessageToServer("CLOSE");
        }

        private void sendMessageToServer(string msg)
        {
            /*Pre: message that needs to be sent to server supplied in msg parameter*/
            /*Post:message sent or error message written to view*/

            try
            {
                if (ntwkstrm.CanWrite)
                {
                    //encode string into byte array and store in outMessage
                    outMessage = encode.GetBytes(msg);
                    //write sent string to view
                    referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "Sending server: " + msg);
                    //send the message
                    ntwkstrm.Write(outMessage, 0, msg.Length);
                    ntwkstrm.Flush();
                }
            }
            catch (IOException ioe)
            {
                //close the socket (if it hasn't been already)
                if (connected != null)
                    connected.Close();
                //close network stream if not closed already
                if (ntwkstrm != null)
                    ntwkstrm.Close();
                referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "IO exception in sendMessageToServer: " + ioe.ToString());
            }
            catch (FormatException fe)
            {
                //if something wasn't formatted correctly tell the view what
                referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "Format exception in sendMessageToServer: " + fe.ToString());
            }
            catch (SocketException se)
            {
                //if socket fails tell view why
                referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "Socket exception in sendMessageToServer: " + se.ToString());
                //close the socket (if it hasn't been already)
                if (connected != null)
                    connected.Close();
                //close network stream if not closed already
                if (ntwkstrm != null)
                    ntwkstrm.Close();
                referenceToView.setConnected(false);
            }
            catch (Exception e)
            {
                //any other exception - tell the view
                referenceToView.Invoke(referenceToView.changeClientStatusTextBox, "Exception in sendMessageToServer: " + e.ToString());
            }
        }
        private string recieveMessageFromServer()
        {
            /*Pre : client sent message to server and expects ACK
             *Post: ACK recieved or cannot read from server*/
            try
            {
                if (ntwkstrm.CanRead)
                {
                    int size = ntwkstrm.Read(inMessage, 0, inMessage.Length);
                    return (encode.GetString(inMessage, 0, size));
                }
                else
                {
                    return ("Cannot read from server.");
                }
            }
            catch (IOException ioe)
            {
                //close network stream if not closed already
                if (ntwkstrm != null)
                {
                    ntwkstrm.Close();
                    ntwkstrm.Dispose();
                    ntwkstrm = new NetworkStream(connected);
                }
                return (ioe.ToString());
            }
            catch (FormatException fe)
            {
                //if something wasn't formatted correctly tell the view what
                return (fe.ToString());
            }
            catch (SocketException se)
            {
                //close the socket (if it hasn't been already)
                if (connected != null)
                    connected.Close();
                //close network stream if not closed already
                if (ntwkstrm != null)
                    ntwkstrm.Close();
                referenceToView.setConnected(false);
                return (se.ToString());
            }
            catch (Exception e)
            {
                //any other exception - tell the view
                return (e.ToString());
            }
        }
    }
}
