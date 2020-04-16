﻿using System.Net.Sockets;

namespace Bonsai.Osc.Net
{
    public class UdpConfiguration : TransportConfiguration
    {
        public int Port { get; set; }

        public string RemoteHostName { get; set; }

        public int RemotePort { get; set; }

        internal override ITransport CreateTransport()
        {
            var udpClient = new UdpClient(Port);
            if (RemotePort > 0) udpClient.Connect(RemoteHostName, RemotePort);
            return new UdpTransport(udpClient);
        }
    }
}
