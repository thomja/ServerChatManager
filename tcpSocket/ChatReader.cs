using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

class ChatReader
{
    public InputCleaner clean = new InputCleaner();
    public string myIPAddress;
    public ThreadStart ChatReading;
    public Thread Reader;
    public NetworkStream stream;
    public IPManager addresses = new IPManager();
    public int port = 80;

    public ChatReader()
    {
        this.myIPAddress = GetIPAddress();
        //Start();
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
                // TcpListener server = new TcpListener(port);
                //server = new TcpListener(IPAddress.Parse(this.myIPAddress), 7777); <--- Returns incorrect local IP
                server = new TcpListener(IPAddress.Parse(myIPAddress), port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    // Get a stream object for reading and writing
                    stream = client.GetStream();

                    int i;

					//addresses.AddIP(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
					//((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() <-- Gets IP address!

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        //Cleans info from string
                        data = clean.StringCleaner(data);
						if(data.StartsWith ("CONNECTINGPORT:")) { //&& addresses.findIP (((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) == false){
							data = data.Replace ("CONNECTINGPORT:", "");
                            addresses.AddClients(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), Int32.Parse(data));
						} else {
							addresses.sendMessages(data);
						}
                        break;
                    }

                    // Shutdown and end connection
                    client.Close();
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

    public string GetIPAddress()
    {
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
        return localIP;
    }
}
