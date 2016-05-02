using System;
using System.Text;
using System.Net.Sockets;


class ChatSender
{
    IPManager myManager;

    public ChatSender()
    {
        return;
    }

    public ChatSender(IPManager myManager)
    {
        this.myManager = myManager;
    }


	public void SendChat(string chat, string IPAddress, Int32 port)
    {
        InputCleaner cleaner = new InputCleaner();
        while (true)
        {
            string myMessage = chat;
            try
            {
                if (myMessage != "")
                {
                    // Create a TcpClient.
                    // Note, for this client to work you need to have a TcpServer 
                    // connected to the same address as specified by the server, port
                    // combination.
                    TcpClient client = new TcpClient(IPAddress, port);

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = Encoding.ASCII.GetBytes("Replying: " + myMessage);

                    // Get a client stream for reading and writing.
                    //  Stream stream = client.GetStream();
                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);
                    // Close everything.
                    stream.Close();
                    client.Close();
                    myMessage = "";
                    return;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                //Console.WriteLine("SocketException: {0}", e); Add this if you want more info as to why the server can't connect to the client
                Console.WriteLine("REMOVING CLIENT: " + IPAddress + ":" + port + " unable to connect.");

                myManager.RemoveIP(IPAddress, port);
            }
            return;
        }
    }
}