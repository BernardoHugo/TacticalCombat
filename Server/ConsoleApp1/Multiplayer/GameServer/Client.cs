using System;
using System.Net;
using System.Net.Sockets;

namespace Catchy.Multiplayer.GameServer
{
    class Client
    {
        public Socket tcpSocket;
        public Socket udpSocket;
        public IPEndPoint iP;
        public int id;

        public Client(int id, IPEndPoint iP )
        {
            this.id = id;
            this.iP = iP;
        }

        public Socket GetSocket(ProtocolType protocolType)
        {
            if (protocolType == ProtocolType.Tcp)
            {
                return tcpSocket;
            }
            else if (protocolType == ProtocolType.Udp)
            {
                return udpSocket;
            }
            else
            {
                return tcpSocket;
            }
        }

        public void SetSocket(Socket socket)
        {
            if (socket.ProtocolType == ProtocolType.Tcp)
            {
                tcpSocket = socket;
            }
            else if (socket.ProtocolType == ProtocolType.Udp)
            {
                 udpSocket = socket;
            }
            else
            {
                throw new InvalidOperationException("Trying to set a socktet with unsupported protocolType: " + socket.ProtocolType);
            }
        }
    }
}
