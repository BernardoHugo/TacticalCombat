using System;
using System.Net;
using System.Net.Sockets;

namespace Catchy.Multiplayer.GameServer
{
    public class Client
    {
        public Socket TcpSocket;
        public Socket UdpSocket;
        public IPEndPoint IP;
        public int Id;

        public Client(int id, IPEndPoint iP )
        {
            this.Id = id;
            this.IP = iP;
        }

        public Socket GetSocket(ProtocolType protocolType)
        {
            if (protocolType == ProtocolType.Tcp)
            {
                return TcpSocket;
            }
            else if (protocolType == ProtocolType.Udp)
            {
                return UdpSocket;
            }
            else
            {
                return TcpSocket;
            }
        }

        public void SetSocket(Socket socket)
        {
            if (socket.ProtocolType == ProtocolType.Tcp)
            {
                TcpSocket = socket;
            }
            else if (socket.ProtocolType == ProtocolType.Udp)
            {
                 UdpSocket = socket;
            }
            else
            {
                throw new InvalidOperationException("Trying to set a socktet with unsupported protocolType: " + socket.ProtocolType);
            }
        }
    }
}
