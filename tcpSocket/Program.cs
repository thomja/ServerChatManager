using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

namespace tcpSocket
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            /*
            Console.WriteLine(AddressFamily.InterNetwork);                              //Test different indexes to find correct IPv4
            IPHostEntry myEntry = Dns.GetHostEntry(string.Empty);                       //Correct home index is 3
            Console.WriteLine("My IPV4 is " + myEntry.AddressList[myEntry.AddressList.Length - 3].ToString());
            for (int i = 0; i < myEntry.AddressList.Length; i++)
            {
                Console.WriteLine(myEntry.AddressList[i]);
            }
            */
            
            IPManager IP = new IPManager();
            ChatReader myClass = new ChatReader();
            myClass.Start();
            Console.WriteLine("My local IP Address is: " + myClass.GetIPAddress());
        }
	}
}
