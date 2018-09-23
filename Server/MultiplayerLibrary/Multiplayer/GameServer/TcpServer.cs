using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Catchy.Multiplayer.GameServer
{
    class TcpServer
    {
        public Action<string> OnMessageReceived;
        public Action<Socket> OnClientConnect;
        public Action<Socket> OnClientDisconnect;

        public TcpServer(int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                WaitForConnection(listener);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void WaitForConnection(Socket listener)
        {
            Console.WriteLine("Waiting for a connection...");

            AsyncCallback acceptCallback = new AsyncCallback(ClientConnection);
            listener.BeginAccept(acceptCallback, listener);
        }


        private void ClientConnection(IAsyncResult result)
        {
            Socket listener = (Socket)result.AsyncState;
            Socket handler = listener.EndAccept(result);

            if (OnClientConnect != null)
            {
                OnClientConnect(handler);
            }

            WaitForConnection(listener);
            StartReceiveMessage(handler);
        }

        private void StartReceiveMessage(Socket handler)
        {
            // An incoming connection needs to be processed. 
            AsyncCallback messageReceivedCallback = new AsyncCallback(EndReceiveMessage);
            SocketError socketError;

            // Data buffer for incoming data.  
            byte[] buffer = new Byte[1024];
            DataState dataState = new DataState(handler, buffer);

            handler.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, messageReceivedCallback, dataState);
        }

        private void EndReceiveMessage(IAsyncResult asyncResult)
        {
            DataState dataState = (DataState)asyncResult.AsyncState;
            Socket handler = dataState.Handler;
            byte[] buffer = dataState.Buffer;
            int messageSize = handler.EndReceive(asyncResult);

            if (messageSize > 0)
            {
                string message = null;
                message = Encoding.ASCII.GetString(buffer, 0, messageSize);

                StartReceiveMessage(handler);
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(message);
                }

            }
            else
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

                if (OnClientDisconnect != null)
                {
                    OnClientDisconnect(handler);
                }
            }
        }

    }
}
