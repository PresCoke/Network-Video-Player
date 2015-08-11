using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace RTPClient_Trial
{
    public class RTP_Protocol
    {
        //static variable for MB
        private static int MB = 1024 * 1024;
        //for gathering packets from server
        private Thread streamThread;
        private Socket streamSocket;
        private EndPoint listenTo;
        private IPEndPoint bindEP;

        //for parsing packets recieved from server
        private int[] packetInfo;
        private RTPpacket packet;
        private MemoryStream workerStream;
        private static RTPClientView referenceToView;
        private int[] dimensions;


        private bool streaming;

        //add reference to the view
        public static void addReferenceToView (RTPClientView refToView)
        {
            referenceToView = refToView;
        }

        public RTP_Protocol(int[] RTPPacketInformation)
	    {
            /*Pre : client needs to start recieving images from a stream
             *Post: an object for creating and parsing streams is created*/

            streaming = false;
            packetInfo = RTPPacketInformation;
            //create RTP packet object
            packet = new RTPpacket(RTPPacketInformation[0], RTPPacketInformation[1],
                RTPPacketInformation[2], RTPPacketInformation[3], RTPPacketInformation[4],
                RTPPacketInformation[5], RTPPacketInformation[6]);
            //create new end point
            bindEP = new IPEndPoint(IPAddress.Any, 6666);
	    }

        public void setup(Socket listeningSocket)
        {
            /*Pre : client requests a video stream from a server
             *Post: the setup process is begun*/
            try
            {
                //gets dimensions of video player
                dimensions = (int[]) referenceToView.Invoke(referenceToView.getVideoDimensions);
                //creates new IPEndPoint
                IPEndPoint temp = new IPEndPoint((listeningSocket.RemoteEndPoint as IPEndPoint).Address, 6666);
                //stores endpoint
                listenTo = temp as EndPoint;
                //creates UDP socket
                streamSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //bind it to any IP address on port: 6666
                streamSocket.Bind(bindEP);
                //probably not neccessary
                streamSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, BitConverter.GetBytes(10000));
                
                this.beginStreaming();
            }
            catch (SocketException se)
            {

            }
        }

        private void beginStreaming()
        {
            /*Pre : client needs to begin streaming
             *Post: thread that listens for and parses packets is created*/
            try
            {
                //currently streaming
                streaming = true;
                ThreadStart runStreamVideo = new ThreadStart(streamVideo);
                streamThread = new Thread(runStreamVideo);
                streamThread.Start();
                //important for killing threads at close
                streamThread.IsBackground = true;
            }
            catch (ThreadStartException tse)
            {
                //stream unsuccessful
                streaming = false;
            }
            catch (SocketException se)
            {
                //stream unsuccessful
                streaming = false;
            }
            catch (Exception e)
            {
            }
        }

        private void streamVideo()
        {
            /*Pre : client created a thread for streaming video
             *Post: a method that listens for and parses RTP packets*/
            try
            {
                try
                {
                    int numImages = 0;
                    byte[] recievedPacket = new byte[1*MB];
                    Bitmap nextImage;

                    while (streaming == true)
                    {
                        //get packet
                        streamSocket.ReceiveFrom(recievedPacket, ref listenTo);
                        //print header (if neccessary)
                        this.printHeaders(recievedPacket);
                        //strip header and make an image
                        nextImage = this.makeImage(packet.unPackPayload(recievedPacket));
                        //add image to video player
                        referenceToView.Invoke(referenceToView.addFrame, nextImage);
                    }
                }
                catch (SocketException se)
                {
                    //no longer streaming
                    streaming = false;
                    if (streamSocket != null)
                    {
                        streamSocket.Disconnect(false);
                    }
                    streamThread.Abort();
                }
            }
            catch (ThreadAbortException tae)
            {
                if (streamSocket != null)
                    streamSocket.Close();
            }
        }
        public void teardown()
        {
            /*Pre : client requested to tear down video
             *Post: movie paused and excess images removed*/
            referenceToView.Invoke(referenceToView.pauseMovie);
            referenceToView.Invoke(referenceToView.tearDownV);
            streaming = false;
        }
        private Bitmap makeImage(byte[] img)
        {
            /*Pre : a byte[] is supplied
             *Post: if successfully parsed an image is returned, if not null returned*/
            try
            {
                workerStream = new MemoryStream(img);
                Bitmap bmp= new Bitmap(workerStream);
                return bmp;
            }
            catch (ArgumentException ae)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private void printHeaders(byte[] packet)
        {
            /*Pre : a packet is supplied
             *Post: if either print packet report or print header checkbox selected print header in the two ways*/
            byte work;
            string packetSummary = null;
            try
            {
                if ((bool)referenceToView.Invoke(referenceToView.checkReportCheckBox))
                {
                    work = packet[0];
                    packetSummary = "Version: " + packetInfo[0].ToString() + " ";
                    packetSummary += "Padding: " + packetInfo[1].ToString() + " ";
                    packetSummary += "Extension: " + packetInfo[2].ToString() + " ";
                    packetSummary += "Contributing Sources: " + packetInfo[3].ToString() + "\n";
                    work = packet[1];
                    packetSummary += "Marker: " + packetInfo[4].ToString() + " ";
                    packetSummary += "Payload Type: " + packetInfo[5].ToString() + " ";
                    packetSummary += "Sequence: " + (BitConverter.ToInt16(packet, 2)).ToString() + "\n";
                    packetSummary += "Timestamp: " + BitConverter.ToString(packet, 4, 4) + "\n";
                    packetSummary += "Synchronization Source: " + (BitConverter.ToInt32(packet, 8)).ToString() + "\n";
                    packetSummary += "Contributing Sources: " + (BitConverter.ToInt32(packet, 12)).ToString() + "\n";

                    referenceToView.Invoke(referenceToView.changeClientStatusTextBox, packetSummary + "\n");
                }
                packetSummary = null;
                if ((bool)referenceToView.Invoke(referenceToView.checkHeaderCheckBox))
                {
                    string temp;
                    for (int i = 0; i < 16; i++)
                    {
                        temp = Convert.ToString(packet[i], 2);
                        packetSummary += temp.PadLeft(8, '0') + " ";
                    }
                    referenceToView.Invoke(referenceToView.changeClientStatusTextBox, packetSummary + "\n\n");
                }
            }
            catch (ArgumentException ae)
            {
            }
            catch (FormatException fe)
            {
            }
            catch (Exception e)
            {
            }
        }
    }
}