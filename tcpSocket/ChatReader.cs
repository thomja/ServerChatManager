using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

class ChatReader
{
    public string myIPAddress;
    public ThreadStart ChatReading;
    public Thread Reader = null;
    public NetworkStream stream;
    public int port = 81;
    public List<Thread> myThreads = new List<Thread>();
    public List<NetworkStream> connectedStreams = new List<NetworkStream>();

    public ChatReader()
    {
        myIPAddress = "192.168.0.196";
    }

    public void Start()
    {
        ChatReading = new ThreadStart(Listen);
        Reader = new Thread(ChatReading);
        Reader.Start();
    }

	public void Listen()
    {
        Console.WriteLine("Listening on " + myIPAddress + ":" + port);
        while (true)
        {

            TcpListener server = null;
            try
            {
                server = new TcpListener(IPAddress.Parse("192.168.0.196"), port);
                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    stream = client.GetStream();
                    ChatReading = new ThreadStart(ReadChat);
                    Reader = new Thread(ChatReading);
                    Reader.Start();
                    myThreads.Add(Reader);
                    break;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }
    }

    void ReadChat()
    {
        NetworkStream stream = this.stream;
        connectedStreams.Add(stream);
        int mySpot = myThreads.Count - 1;
        while (true)
        {
            // Get a stream object for reading and writing
            Byte[] bytes = new Byte[256];
            String data = null;
            int i;
            // Loop to receive all the data sent by the client.
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    //Cleans info from string
                    Console.WriteLine(data);
                    for(int p = 0; p < connectedStreams.Count; p++)
                    {
                        if(p != mySpot)
                        {
                            connectedStreams[p].Write(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetBytes(data).Length);
                        }
                    }
                    break;
                }
            } catch(Exception e)
            {
                Console.WriteLine("Client disconnected, closing thread number {0}", mySpot);
                Console.WriteLine(e);
                myThreads[mySpot].Abort();
            }
        }
    }
}
