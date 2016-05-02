using System;
using System.Collections.Generic;
using tcpSocket;

class IPManager
{
    public List<string> IPAddresses = new List<string>();
	public List<int> ports = new List<int> ();
    public ChatSender send;
    //Experimental
    public List<Clients> myClients = new List<Clients>();
    //Experimental

    public IPManager()
    {
        send = new ChatSender(this);
    }

    public void RemoveIP(string IPAddress, int port)
    {
        for(int i = 0; i < myClients.Count; i++)
        {
            if(myClients[i].clientIP == IPAddress)
            {
                if(myClients[i].ports.Count == 1)
                {
                    myClients.Remove(myClients[i]);
                } else
                {
                    for(int o = 0; o < myClients[i].ports.Count; o++)
                    {
                        if(myClients[i].ports[o] == port)
                        {
                            myClients[i].ports.Remove(myClients[i].ports[o]);
                        }
                    }
                }
            }
        }
    }

    public void sendMessages(string message)
    {
        Console.WriteLine("Sending: " + message);
        //for (int i = 0; i < myClients.Count; i++)
        for(int i = 0; i < myClients.Count; i++)
        {
            //send.SendChat(message, IPAddresses[i], ports[i]);
            for(int o = 0; o < myClients[i].ports.Count; o++)
            {
                send.SendChat(message, myClients[i].clientIP, myClients[i].ports[o]);
            }
        }
    }

	public bool findIP(string myIP){
		for (int i = 0; i < IPAddresses.Count; i++) {
			if (IPAddresses [i] == myIP) {
				return true;
			}
		}
		return false; //Is true for debugging purposes
	}
    //Experimental
    public void AddClients(string address, Int32 ports)
    {
        bool foundIP = false;
        foreach(Clients client in myClients)
        {
            if(client.clientIP == address)
            {
                client.ports.Add(ports);
                foundIP = true;
                Console.WriteLine("New port added to: " + client.clientIP);
            }
        }
        if (!foundIP)
        {
            List<int> myPort = new List<int>();
            myPort.Add(ports);
            Clients newClient = new Clients(address, myPort);
            myClients.Add(newClient);
            Console.WriteLine("-----NEW CLIENT-----");
            Console.WriteLine("Added client: " + myClients[0].clientIP);
            Console.WriteLine("Client port: " + myClients[0].ports[0]);
            Console.WriteLine("-----NEW CLIENT-----");
        }
    }
    //Experimental
}
