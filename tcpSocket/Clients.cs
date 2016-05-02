using System;
using System.Collections.Generic;


namespace tcpSocket
{
    class Clients
    {
        public string clientIP;
        public List<int> ports = new List<int>();

        public Clients(string address, List<int> ports)
        {
            clientIP = address;
            this.ports = ports;
        }
    }
}
