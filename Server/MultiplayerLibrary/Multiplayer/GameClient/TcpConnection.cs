using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Catchy.Multiplayer.Common;

namespace Catchy.Multiplayer.GameClient
{
    class TcpConnection
    {
        private Socket sender;

        public Action<string> OnMessageReceived;

        public TcpConnection(string ip, int port)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(ip);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    StartReceiveMessage(sender);
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
            Socket handler = dataState.handler;
            byte[] buffer = dataState.buffer;
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
            }
        }


        public void ShutdownClient()
        {
            // Release the socket.  
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public void SendMessage(byte[] data)
        {
            if (sender.Connected)
            {
                // Send the data through the socket.  
                int bytesSent = sender.Send(data);
            }
        }
    }
}