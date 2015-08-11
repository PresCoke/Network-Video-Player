using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Text;


namespace RTPServer_Trial
{
    //manages client connections
    public class RTSPClient
    {
        //constant for MegaByte
        private static int MB = 1024 * 1024;
        //CSV string to hold movie titles
        private static string movieList = "video1.mjpeg;video2.mjpeg;";
        //static reference to view
        private static RTPServerMainView referenceToView;
        //static reference to server
        private static StreamingServer referenceToServer;
        //encoding object to transform from bytes to strings and back again - why everyting fails is BECAUSE NOT ASCII
        private static ASCIIEncoding encode;

        //one thread for each client object
        public Thread clientThread;
        //one socket for each client object
        public Socket clientSkt;
        //for writing through socket
        public NetworkStream ntwkstrm;
        //for handling streaming
        public RTP_Protocol streamVideo;
        //to track whether video is streaming and what appropriate response to client is
        private bool videoStreaming, videoPlaying;
        
        //adds reference to view (without creating object)
        public static void addReferenceToView (RTPServerMainView refview)
        {
            referenceToView = refview;
            encode = new ASCIIEncoding();
        }
        //adds reference to server (without creating object)
        public static void addReferenceToServer(StreamingServer refserver)
        {
            referenceToServer = refserver;
        }

        //creates new client object
	    public RTSPClient(Socket connectedFrom, string name)
	    {
            try
            {
                //initialize socket client communicating through
                clientSkt = connectedFrom;
                //intialize stream with new socket
                ntwkstrm = new NetworkStream(clientSkt);
                //create delegate for runClient method
                ThreadStart runClientMethod = new ThreadStart(this.runClient);
                //initialize thread to run runClient
                clientThread = new Thread(runClientMethod);
                clientThread.Name = name;
                
                //start the thread
                clientThread.Start();
            }
            catch (ThreadStartException tse)
            {
                //if thread could not be started notify view
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, ("Thread start exception in RTSPClient constructor: "+tse.ToString()));
            }
            catch (Exception e)
            {
                //if any other exception occured notify view
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, ("Exception in RTSPClient constructor: "+e.ToString()));
            }
	    }
        //creates new client thread returns true if successful false if unsuccessful
        private void runClient()
        {
            /*Pre: thread created and socket initialized in constructor*/
            /*Post:runs until thread aborted.*/
            //recieve buffer
            byte[] recievedBytes = new byte [100];
            //send buffer
            byte[] sendBytes = new byte [100];

            int RTSPSeq = 0;

            videoStreaming = false;
            videoPlaying = false;

            streamVideo = new RTP_Protocol(Int32.Parse(this.clientThread.Name), referenceToView);
            

            string recievedString = "";
            int recievedLength = 0;
            try
            {
                while (true)
                {
                    try
                    {
                        //wait to recieve command from client and store command in recievedBytes
                        //clientSkt.Receive(recievedBytes);
                        if (ntwkstrm.CanRead && ntwkstrm.CanWrite)
                        {
                            //call read which returns number of bytes and stores data in first parameter - recievedBytes
                            recievedLength = ntwkstrm.Read(recievedBytes, 0, recievedBytes.Length);
                            RTSPSeq++;
                            //turn it into a string
                            recievedString = encode.GetString(recievedBytes, 0, recievedLength);

                            //output what the client sent to the view
                            referenceToView.Invoke(referenceToView.changeClientRequestTextBox, (recievedString +
                                "\rCSeq:" + RTSPSeq.ToString() +
                                "\rSession: " + this.clientThread.Name));

                            //send client back the results of parsing the command message
                            sendBytes = this.parseMessage(recievedString, RTSPSeq);
                            
                            ntwkstrm.Write(sendBytes, 0, sendBytes.Length);
                            ntwkstrm.Flush();
                        }
                    }
                    catch (System.IO.IOException)
                    {
                        //if client socket not null
                        if (clientSkt != null)
                        {
                            //if network stream not closed close and dispose it
                            if (ntwkstrm != null) 
                            {
                                ntwkstrm.Close();
                                ntwkstrm.Dispose();
                            }
                        }
                    }
                    catch (SocketException se)
                    {
                        //if client socket not null close it
                        if (clientSkt != null)
                        {
                            clientSkt.Close();
                        }
                        //if network stream not closed close it
                        if (ntwkstrm != null)
                        {
                            ntwkstrm.Close();
                            ntwkstrm.Dispose();
                        }
                        //do not attempt to restore connection - abort thread
                        clientThread.Abort();
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
            catch (ThreadAbortException tae)
            {
                //if client socket not null close it
                if (clientSkt != null) clientSkt.Close();
                //if network stream not closed close it
                if (ntwkstrm != null)
                {
                    ntwkstrm.Close();
                    ntwkstrm.Dispose();
                }
            }
            finally
            {
                //finally after done processing remove client from servers list of clients
                referenceToServer.removeClientThread(this);
            }
        }

        private byte[] parseMessage(/*byte[]*/string inMessage, int seq)
        {
            /*Pre: a byte[] is passed that needs to be parsed.*/
            /*Post:depending on the contents of the message outMessage will contain
             *the success/validity of the command to be relayed back to the client.*/

            //a temporary string of the contents of inMessage
            string videoName = "\0";
            string temp = inMessage;
            //for parsing messages to find a movie name
            //if there is at least one ';'
            if (temp.Contains(";"))
            {
                int i = temp.Length;
                while (i != 0)
                {
                    i--;
                    //find another ';'
                    if (temp[i].Equals(';'))
                    {
                        try
                        {
                            //parse what is between them
                            char[] tempVideoName = new char[temp.Length - i];
                            temp.CopyTo(i + 1, tempVideoName, 0, (temp.Length - i - 1));
                            videoName = tempVideoName[0].ToString();
                            for (int k = 1; k < tempVideoName.Length - 1; k++)
                                videoName += tempVideoName[k].ToString();
                            //then remove it.
                            temp = temp.Remove(i);
                            i = 0;
                        }
                        catch (IndexOutOfRangeException ioore)
                        {
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }

            //new byte array for return value
            byte[] outMessage = new byte[1 * MB];
            byte[] tempMessage = null;
            switch (temp)
            {
                //new client says HELO.
                case "HELO":
                    //server returns a welcome message
                    referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "The client: " +
                        (clientSkt.RemoteEndPoint as IPEndPoint).Address + ":" + clientThread.Name +
                        " has joined.");
                    //tells client what mvoies are available
                    tempMessage = encode.GetBytes("Welcome;"+movieList);
                    return tempMessage;
                //client pauses video
                case "PAUSE":
                    //server attempts to pause transfer
                    if (videoPlaying == false || videoStreaming == false)
                        tempMessage = streamVideo.clientError("Video not capable of PAUSE at this time.");
                    else
                    {
                        videoPlaying = false;
                        tempMessage = this.streamVideo.pause();
                    }
                    break;
                //client plays video
                case "PLAY":
                    //server attempts to play video
                    if (videoStreaming == false || videoPlaying == true)
                        tempMessage = this.streamVideo.clientError("Video not capable of PLAY at this time.");
                    else
                    {
                        videoPlaying = true;
                        tempMessage = this.streamVideo.play();
                    }
                    break;
                //client sets up video
                case "SETUP":
                    //server attempts to setup video
                    bool isVideo = this.streamVideo.videoExists(videoName);
                    if (videoStreaming == true)
                        tempMessage = this.streamVideo.clientError("Must TEARDOWN video first.");
                    else if (videoStreaming==false && isVideo)
                    {
                        videoStreaming = true;
                        tempMessage = this.streamVideo.setup(clientSkt, this.clientThread.Name, videoName);
                    }
                    else
                        tempMessage = this.streamVideo.clientError(videoName+" is not a file.");
                    break;
                //client tears down video
                case "TEARDOWN":
                    //server attempts to close transfer
                    if (videoStreaming == false)
                        tempMessage = this.streamVideo.clientError("Must SETUP a video first.");
                    else
                    {
                        videoStreaming = false;
                        tempMessage = this.streamVideo.teardown();
                    }
                    break;
                 //client closing
                case "CLOSE":
                    if (videoStreaming == true)
                    {
                        videoStreaming = false;
                        tempMessage = this.streamVideo.teardown();
                    }
                    this.clientThread.Abort();
                    break;
                //command not recognized
                default:
                    //server attempts to figure out source of error
                    tempMessage = this.streamVideo.clientError(temp);
                    break;
            }
            //write response message.
            int lengthOfMessage = tempMessage.Length;
            try
            {
                string control = "\rCSeq: " + seq + "\rSession: " + clientThread.Name + "\r";
                tempMessage.CopyTo(outMessage, 0);
                encode.GetBytes(control, 0, control.Length, outMessage, lengthOfMessage);
            }
            catch (Exception e)
            {
            }
            //tell view what server is sending to client
            referenceToView.Invoke(referenceToView.changeServerStatusTextBox, ("Sending client: " + encode.GetString(outMessage)));
            
            return outMessage;
        }
        //Not Used!!
        public void destroyThread()
        {
            try
            {
                clientThread.Abort();
            }
            catch (ThreadAbortException tae)
            {
            }
            catch (ThreadStateException tse)
            {
            }
        }
    }
}