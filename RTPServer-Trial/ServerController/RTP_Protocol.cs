using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace RTPServer_Trial 
{
    public class RTP_Protocol
    {
        //reference to view
        private RTPServerMainView referenceToView;
        //for encoding strings to bytes
        private static System.Text.ASCIIEncoding encode;
        //for creating packets
        private RTPpacket packet;
        //client ID number
        private int clientID;
        //for gathering images from file
        private FileStream video;
        //for writing images from file to byte[]
        BinaryReader br;
        //pathname to video
        private string videoPath;
        //tracks whether currently streaming
        private bool streaming;

        public RTP_Protocol(int cID, RTPServerMainView refToView)
	    {
            /*Pre : server needs to stream video
             *Post: server is able to stream videos to a remote client*/
            streaming = false;
            referenceToView = refToView;
            encode = new System.Text.ASCIIEncoding();
            packet = new RTPpacket(2, 0, 0, 0, 0, 26, StreamingServer.NameOfServer);
            clientID = cID;
	    }

        public bool videoExists(string videoName)
        {
            /*Pre : remote client has requested a video server attempts to find it
             *Post: true is returned if video exists false if not.*/
            string temp = Path.GetFileName("..\\..\\Videos\\" + videoName);
            if (temp == videoName)
            {
                videoPath = "..\\..\\Videos\\" + videoName;
                return true;
            }
            else
                return false;
        }

        private bool getVideo(string videoName)
        {
            /*Pre : server knows video exists and attempts to open a file and read from it.
             *Post: returns true if file opened successfully, false if not.*/
            try
            {
                video = new FileStream(videoPath, FileMode.Open, FileAccess.Read);
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "New RTSP state: READY \rSession: "+clientID);
                br = new BinaryReader(video);
                return true;
            }
            catch (IOException ioe)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "New RTSP state: FAIL \rSession: " + clientID
                    + "\rReason: " + ioe.ToString() + "\n");
                return false;
            }
        }

        private void writeToView(byte[] outMessage)
        {
            /*Pre : RTP_Protocol wants to write packet info to server 
             *Post: if server has requested header information, then packet header written to screen.*/
            referenceToView.Invoke(referenceToView.changeFrameNumber, packet.getSequenceNumber());
            //if checkHeader is checked
            if ((bool)(referenceToView.Invoke(referenceToView.checkHeader)))
            {
                string header = "";
                for (int i = 0; i < 16; i++)
                {
                    header += Convert.ToString(outMessage[i], 2).PadLeft(8, '0') + " ";
                }
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, (header + "\n"));
            }
        }

        public byte[] pause()
        {
            /*Pre: client sends PAUSE message*/
            /*Post:video paused or error code returned to client*/
            if (streamThread != null)
                videoStreaming = false;
            referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "New RTSP state: READY \rSession: " + clientID);
            return (encode.GetBytes("203:Paused Video"));
        }

        public byte[] play()
        {
            /*Pre: client sends PLAY message*/
            /*Post:video plays or error code returned to client*/
            if (streamThread != null)
                videoStreaming = true;
            referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "New RTSP state: PLAYING \rSession: " + clientID);
            return (encode.GetBytes("202:Playing Video."));
        }

        public byte[] setup(Socket stream, string clientName, string nameOfVideo)
        {
            /*Pre: client sends SETUP message*/
            /*Post:video connection setup or error code returned to client*/
            this.beginStreaming(stream, clientName, nameOfVideo);
            referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "New RTSP state: READY \rSession: " + clientID);
            return (encode.GetBytes("201:Streaming " + nameOfVideo + "."+";2;0;0;0;0;26;"+StreamingServer.NameOfServer+";"));
        }

        public byte[] teardown()
        {
            /*Pre: client sends TEARDOWN message*/
            /*Post:video connection ended or error code returned to client*/
            streaming = false;
            if (streamThread != null)
                streamThread.Abort();
            referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "New RTSP state: FINISHED \rSession: " + clientID);
            return (encode.GetBytes("204:Tearing video down."));
        }

        public byte[] clientError(string invalidMessage)
        {
            /*Pre: client sends unrecognized message*/
            /*Post:an error code is returned to client*/
            return (encode.GetBytes("400: " + invalidMessage.ToString()));
        }

        //for streaming
        private Thread streamThread;
        private Socket streamSocket;

        //private IPAddress clientIP;
        private IPEndPoint clientEndPoint;
        //private NetworkStream ntwkstrm;
        private bool videoStreaming;

        private void beginStreaming(Socket connectedFrom, string clientThreadName, string nameOfVideo)
        {
            /*Pre : remote client has requested to stream a video and the remote clients socket, thread name and the video name are passed.
             *Post: if successful thread for streaming is created.*/
            try
            {
                //create new end point to send to
                clientEndPoint = new IPEndPoint((connectedFrom.RemoteEndPoint as IPEndPoint).Address, 6666);
                //new socket for communicating with remote client
                streamSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //if video can successfully be opened
                if (this.getVideo(nameOfVideo))
                {
                    //create thread
                    ThreadStart runStreamMethod = new ThreadStart(this.stream);
                    streamThread = new Thread(runStreamMethod);
                    streamThread.Name = clientThreadName + nameOfVideo;
                    videoStreaming = false;

                    streamThread.Start();
                }
                else this.clientError("Video does not exist");

            }
            catch (ThreadStartException tse)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, tse.ToString());
            }
            catch (SocketException se)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, se.ToString());
                streamSocket.Close();
            }
            catch (Exception e)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, e.ToString());
            }
        }

        private void stream()
        {
            try
            {

                string header = null;
                string temp = null;
                streaming = true;
                streamSocket.SendTimeout = 15000;

                while (streaming == true)
                {
                    //to stop sending packets when paused.
                    if (videoStreaming == true)
                    {
                        try
                        {
                            //create new packet with image
                            byte[] sending = packet.newPacket(this.nextImage());
                            //write header to view
                            writeToView(sending);
                            //change frame number
                            referenceToView.Invoke(referenceToView.changeFrameNumber, packet.getSequenceNumber());
                            //send packet to client
                            streamSocket.SendTo(sending, clientEndPoint);
                        }
                        catch (SocketException se)
                        {
                            referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "Socket Exception in " +
                                streamThread.Name + ": " +
                                se.ToString());
                            if (streamSocket != null)
                            {
                                streamSocket.Close();
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException tae)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "Thread Exception in " +
                        streamThread.Name + ": " +
                        tae.ToString());
            }
            finally
            {
                //close video file stream
                if (video != null)
                {
                    video.Close();
                    video.Dispose();
                }
            }
        }
        
        private byte[] nextImage()
        {
            /*Pre : client requires a new image
             *Post: the next image in stream returned*/
            int imageLength = 0;
            byte[] lengthOfNextImage;
            byte[] image;

            try
            {
                //get the ascii number of the length
                lengthOfNextImage = br.ReadBytes(5);
                //if a valid length store it in imageLength
                if (Int32.TryParse(encode.GetString(lengthOfNextImage), out imageLength))
                {
                    //read image bytes into image
                    image = br.ReadBytes(imageLength);
                    return image;
                }
                //if not valid restart stream
                //and attempt the same as above
                else
                {
                    br.BaseStream.Position = 0;
                    lengthOfNextImage = new byte[5];
                    br.Read(lengthOfNextImage, 0, 5);
                    try
                    {
                        imageLength = Int32.Parse(encode.GetString(lengthOfNextImage));
                    }
                    catch (FormatException fe)
                    {
                        return null;
                    }
                    image = br.ReadBytes(imageLength);
                    packet.resetSequenceNumber();
                    return image;
                }
            }
            catch (EndOfStreamException eose)
            {
                return (null);
            }
            catch (IndexOutOfRangeException ioore)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, ioore.ToString());
                return (null);
            }
            catch (Exception e)
            {
                referenceToView.Invoke(referenceToView.changeServerStatusTextBox, e.ToString());
                return (null);
            }
        }
    }
}