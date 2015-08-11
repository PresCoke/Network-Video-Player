using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

//main server manager: manages incoming requests and passes them to the client manager or regects them.
namespace RTPServer_Trial
{
    public class StreamingServer
    {
        //for creating random numbers
        private static Random r;

        public static int NameOfServer;
        //a static reference to the view for calling delegates to write to appropriate text box
        private static RTPServerMainView referenceToView;

        //a delegate for removing a client from the server
        public delegate void removeClientFromServer(RTSPClient clnt);
        public removeClientFromServer removeClientThread;

        //a constant describing the maximum number of allowable clients
        private static int MAXCLIENTS = 5;
        //a linked list of RTSPClient's i.e. client threads
        private static LinkedList<RTSPClient> clientList = new LinkedList<RTSPClient>();

        //the socket the server is listening for incoming connections from.
        Socket tcpServer;
        Thread listeningOnPort;
        private bool listening;

	    public StreamingServer(RTPServerMainView reference)
	    {
            listening = false;
            r = new Random();
            //name server
            NameOfServer = r.Next(10000);
            //reference to the view
            referenceToView = reference;
            //propogating reference to view to other controller through static method
            RTSPClient.addReferenceToView(reference);
            //adding removeCLient method to delegate
            removeClientThread = new removeClientFromServer(removeClient);
            //adds reference to server for client threads
            RTSPClient.addReferenceToServer(this);
	    }
        //creates a new socket that listens on port: [portNumber] and listens for a connection
        //exits on error and returns error string
        //otherwise returns control string(???)
        public string listenOnPort(Int16 portNumber)
        {
            /*Pre: User supplies 16 bit inetger port number to listen on.*/
            /*Post:A new thread is created to constantly listen for new connections.
             *returns a string to inform the user how successful the method was.*/
            try
            {
                //create new endpoint listening for any IPAdress on portNumber
                IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
                //create a new TCP socket of streamtype(?)
                tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //bind the socket to the endpoint
                tcpServer.Bind(listenEndPoint);
                try
                {
                    //create delegate for thread method
                    ThreadStart acceptConnectionMethod = new ThreadStart(this.acceptConnection);
                    //create new thread that runs acceptConnection
                    listeningOnPort = new Thread(acceptConnectionMethod);
                    //start the thread
                    listeningOnPort.Start();
                    listeningOnPort.IsBackground = true;
                }
                catch (ThreadStartException tse)
                {
                    //if thread could not be started
                    return ("Main listen Thread Start failed in StreamingServer with: "+tse.ToString());
                }
                catch (Exception te)
                {
                    //if some other exception occured
                    return ("Main listen Thread failed in StreamingServer with: " + te.ToString());
                }
                return ("Listening on Port:" + portNumber + " with socket type: " +tcpServer.SocketType);
            }
            catch (SocketException se)
            {
                //if socket could not be created
                return ("Main Socket failed in StreamingServer with: "+se.ToString());
            }
        }
        private void acceptConnection()
        {
            /*Pre: the listenOnPort method created a thread.*/
            /*Post:the method will continually look for new connections*/
            listening = true;
            while (listening == true)
            {
                //blocking - the socket starts listening
                tcpServer.Listen(int.MaxValue);
                try
                {
                    //wait for connection attempted, store socket info inside tryToConnect
                    Socket tryToConnect = tcpServer.Accept();
                    //if number of clients does not exceed MAXCLIENTS
                    int numClients = clientList.Count;
                    if (numClients < MAXCLIENTS && tryToConnect != null)
                    {
                        /***ADD Name for client - hopefully something random***/
                        //create a new client object (with thread) with socket information passed to constructor
                        RTSPClient client = new RTSPClient(tryToConnect, (r.Next(10000) + r.Next(10)).ToString());
                        //and add client object to linked list
                        clientList.AddLast(client);
                        //tell main view that client was added
                        referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "Client added.");
                    }
                    else
                    {
                        //otherwise tell main view client was not added
                        referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "Client could not be added.");
                    }
                }
                catch (SocketException se)
                {
                    //if socket fails close it
                    if (tcpServer != null)
                        tcpServer.Close();
                    //and abort thread
                    if (listeningOnPort != null)
                        listeningOnPort.Abort();
                }
                catch (ThreadAbortException tae)
                {
                    //if thread is attempted to abort check if socket is closed if not close it.
                    if (tcpServer != null) 
                        tcpServer.Close();
                }
                catch (Exception e)
                {
                }
            }
        }

        public void removeClient(RTSPClient removeThisClient)
        {
            /*Pre: a client object to be removed is supplied*/
            /*Post:the client object has been removed from the linked list making room for other clients*/
            try
            {
                //if client object is in the list
                if (clientList.Contains(removeThisClient))
                {
                    //remove the object from the list
                    clientList.Remove(removeThisClient);
                }
            }
            catch (ArgumentException ae)
            {
                //if a bad argument was supplied tell the main view what caused it
                if (referenceToView != null)
                    referenceToView.Invoke(referenceToView.changeServerStatusTextBox, "Argument exception in RemoveClient StreamingServer: "+ae.ToString());
            }
        }
        
        public void CloseSocket()
        {
            listening = false;
        }
    }
}
