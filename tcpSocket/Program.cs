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
            ChatReader myClass = new ChatReader();
            myClass.Start();
        }
	}
}
