using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        private static List<Socket> clients = new List<Socket>();

        static void Main(string[] args)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5050);

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

            while (true)
            {
                Console.WriteLine("\nPress ENTER to continue...");
                Console.Read();
            }
        }

        private static void WaitForConnection(Socket listener)
        {
            Console.WriteLine("Waiting for a connection...");

            AsyncCallback acceptCallback = new AsyncCallback(ClientConnection);
            listener.BeginAccept(acceptCallback, listener);
        }


        private static void ClientConnection(IAsyncResult result)
        {
            Socket listener = (Socket)result.AsyncState;
            Socket handler = listener.EndAccept(result);

            clients.Add(handler);
            WaitForConnection(listener);

            StartReceiveMessage(handler);
        }

        private static void StartReceiveMessage(Socket handler)
        {
            // An incoming connection needs to be processed. 
            AsyncCallback messageReceivedCallback = new AsyncCallback(EndReceiveMessage);
            SocketError socketError;

            // Data buffer for incoming data.  
            byte[] buffer = new Byte[1024];
            DataState dataState = new DataState(handler, buffer);

            handler.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, messageReceivedCallback, dataState);
        }

        private static void EndReceiveMessage(IAsyncResult asyncResult)
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
                OnMessageReceived(message);
                
            }
            else
            {
                clients.Remove(handler);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }

        private static void OnMessageReceived(string message)
        {
            // Show the data on the console.  
            Console.WriteLine("Text received : {0}", message);

            for (int i = 0; i < clients.Count; i++)
            {
                SendMessage(clients[i], message);
            }


        }

        private static void SendMessage(Socket handler, string message)
        {
            if (handler.Connected)
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.ASCII.GetBytes(message);

                // Send the data through the socket.  
                int bytesSent = handler.Send(msg);
            }

        }

    }
}
