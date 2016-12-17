using System;

class ChatReader
{
    public string myIPAddress;

    public ChatReader()
    {
        this.myIPAddress = GetIPAddress();
        Start();
    }

    public void Start()
    {
        ThreadStart ChatReading = new ThreadStart(Listen);
        Thread Reader = new Thread(ChatReading);
        Reader.Start();
    }

    public void Listen()
    {
        TcpListener server = null;
        try
        {
            // TcpListener server = new TcpListener(port);
            server = new TcpListener(IPAddress.Parse(this.myIPAddress), 7777);

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
                NetworkStream stream = client.GetStream();


                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);
                    break;
                }

                // Shutdown and end connection
                client.Close();
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
