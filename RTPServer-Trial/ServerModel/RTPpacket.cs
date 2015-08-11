using System;
namespace RTPServer_Trial
{
    public class RTPpacket
    {
        //for writing byte[]
        private static System.Text.ASCIIEncoding encode;
        //variables that do not change for every packet
        private int version, contributingSources, payloadType, syncrhonizationSource, padding, extension, marker;
        //variables that change with each packet
        private ushort sequence; //to fit into two bytes
        
        public RTPpacket(int ver, int pad, int ext, int numContribs, int mark, int payload, int scyncronization)
	    {
            /*Pre : constructor taking static portions of packet as parameters
             *Post: RTP_Packet object constructed with the ability to construct new RTP packets*/
            encode = new System.Text.ASCIIEncoding();
            version = ver;
            padding = pad;
            extension = ext;
            contributingSources = numContribs;
            marker = mark;
            payloadType = payload;
            syncrhonizationSource = scyncronization;
            sequence = 0;
	    }

        public int getSequenceNumber()
        {
            /*Pre : client needs current sequence number
             *Post: sequence number returned*/
            return this.sequence;
        }
        public void resetSequenceNumber()
        {
            /*Pre : client wants to reset sequence number to re-use same RTP_Packet object
             *Post: sequence number reset*/
            this.sequence = 0;
        }
        public byte[] newPacket(byte[] payload)
        {
            /*Pre : client wants to create a new packet with a body of parameter payLoad
             *Post: a packet created with appropriate header information and a message body of payLoad*/
            try
            {
                try
                {
                    //increment sequence number for packet header
                    sequence++;
                }
                catch (OverflowException oe)
                {
                    //incase of overflow reset sequence
                    sequence = 0;
                }
                //create new packet to contain header and payload
                byte[] packet = new byte[16 + (payload.Length)];
                //create byte[] for header
                byte[] header = createPacketHeader();
                //copy header and payLoad to packet
                header.CopyTo(packet, 0);
                payload.CopyTo(packet, 16);
                //return packet
                return packet;
            }
            catch (NullReferenceException nre)
            {
                //if null reference exception occured return byte[] with error string.
                string error = nre.ToString();
                byte[] errorPacket = new byte[error.Length];
                encode.GetBytes(error.ToCharArray(), 0, error.Length, errorPacket, 0);
                return errorPacket;
            }
            catch (Exception e)
            {
                //if exception occured return byte[] with error string
                string error = e.ToString();
                byte[] errorPacket = new byte[error.Length];
                encode.GetBytes(error.ToCharArray(), 0, error.Length, errorPacket, 0);
                return errorPacket;
            }
        }
        private byte[] createPacketHeader()
        {
            /*Pre : client has asked for a new packet and newPacket has called createPacketheader
             *Post: a byte[] containing all header information is returned*/
            //byte 1 of header (i.e. version, padding, extension, and CC)
            byte header1Pt1 = new byte();
            //byte 2 of header (i.e marker and pay load type)
            byte header1Pt2 = new byte();
            //bytes 3 and 4 of headr (i.e. sequence number)
            byte[] header1Pt3 = new byte[2];
            //byte[4] for timestamp
            byte[] timeStamp = new byte[4];
            //byte[4] for sync source
            byte[] SSRC = new byte [4];
            //byte[4] for contrib. src's
            byte[] CRSC = new byte[4];
            //byte[] to contain packet header
            byte[] newPacket = new byte[16];
            try
            {
                //creates a header based on emulating the binary value of each byte where byte is the sume of:
                //value*(lowest occupied place value of field)
                //v=2*64=128 + P=0*32=0 + X=0*16=0 + CC=0000 = 128
                header1Pt1 =(byte) ((version * 64) + (padding * 32) + (((int)extension) * 16) + contributingSources);
                //M=0*128 + PT=26 = 26
                header1Pt2 = (byte)((marker * 128) + payloadType);
                //sequence varies
                header1Pt3 = BitConverter.GetBytes(sequence);

                timeStamp = BitConverter.GetBytes((Int32)(DateTime.Now.ToBinary()));

                SSRC = BitConverter.GetBytes(syncrhonizationSource);

                CRSC = BitConverter.GetBytes(contributingSources);

                newPacket[0]  = header1Pt1;
                newPacket[1]  = header1Pt2;
                newPacket[2]  = header1Pt3[0];
                newPacket[3]  = header1Pt3[1];
                newPacket[4]  = timeStamp[0];
                newPacket[5]  = timeStamp[1];
                newPacket[6]  = timeStamp[2];
                newPacket[7]  = timeStamp[3];
                newPacket[8]  = SSRC[0];
                newPacket[9]  = SSRC[1];
                newPacket[10] = SSRC[2];
                newPacket[11] = SSRC[3];
                newPacket[12] = CRSC[0];
                newPacket[13] = CRSC[1];
                newPacket[14] = CRSC[2];
                newPacket[15] = CRSC[3];
            }
            catch (IndexOutOfRangeException ioore)
            {
                System.Console.Out.WriteLine(ioore.ToString());
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e.ToString());
            }
            return newPacket;
        }

        public byte[] unPackPayload(byte[] packet)
        {
            /*Pre : a RTP packet needs to be stripped of it's header
             *Post: the pay load of the packet is returned*/
            try
            {
                //variable to contain returned pay load
                byte[] payLoad = new byte[packet.Length - 16];
                for (int i = 16; i < packet.Length; i++)
                {
                    payLoad[i - 16] = packet[i];
                }
                return payLoad;
            }
            catch (ArgumentException ae)
            {
                //if fail do nothing
                return null;
            }
            catch (Exception e)
            {
                //if fail do nothing
                return null;
            }
        }
    }
}