using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Catchy.Multiplayer.Chat;
using Catchy.Multiplayer.Common;

namespace Catchy.Multiplayer.GameServer
{
    public class Server
    {
        private Dictionary<int, Client> clients = new Dictionary<int, Client>();
        private Dictionary<string, IMessageObserver> messageObservers;

        private int lastClientID;

        private TcpServer tcpServer;

        private ChatServerHandler _chatServerHandler;

        private const int SERVER_PORT = 5050;

        public Server()
        {
            messageObservers = new Dictionary<string, IMessageObserver>();

            tcpServer = new TcpServer(SERVER_PORT);
            tcpServer.OnMessageReceived += OnMessageReceived;
            tcpServer.OnClientConnect += OnClientConnect;
            tcpServer.OnClientDisconnect += OnClientDisconnect;

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            _chatServerHandler = new ChatServerHandler(this);
        }

        public void OnClientConnect(Socket clientSocket)
        {
            IPEndPoint iP = clientSocket.RemoteEndPoint as IPEndPoint;
            lastClientID++;

            Client newClient = new Client(lastClientID, iP);
            newClient.SetSocket(clientSocket);
            clients.Add(lastClientID, newClient);
        }

        public void OnClientDisconnect(Socket clientSocket)
        {
            int id = clients.First(x => x.Value.tcpSocket == clientSocket).Key;
            clients.Remove(id);
        }

        private void OnMessageReceived(string messageJson)
        {
            ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(messageJson);
            if (messageObservers.ContainsKey(message.name))
            {
                messageObservers[message.name].OnMessageReceived(message.data);
            }
            else
            {
                Console.WriteLine($"[GameServer] OnMessageReceived: No observer for {message.name} message");
            }
        }

        public void AddMessageObserver(string messageName, IMessageObserver messageObserver)
        {
            if (!messageObservers.ContainsKey(messageName))
            {
                messageObservers.Add(messageName, messageObserver);
            }
            else
            {
                Console.WriteLine($"[GameServer] AddMessageObserver: Trying to add an observer to {messageName} but that already has observer");
            }
        }

        public void SendMessageForAllClients(ProtocolType protocolType, string name, string data)
        {
            foreach (KeyValuePair<int, Client> client in clients)
            {
                SendMessage(client.Key, protocolType, name, data);
            }
        }

        public void SendMessageForAList(List<int> targets, ProtocolType protocolType, string name, string data)
        {
            foreach (int target in targets)
            {
                SendMessage(target, protocolType, name, data);
            }
        }

        public void SendMessage(int targetId, ProtocolType protocolType, string name, string data)
        {
            if (!clients.ContainsKey(targetId))
            {
                return;
            }

            Socket handler = clients[targetId].GetSocket(protocolType);
            if (handler.Connected)
            {
                ServerMessage message = new ServerMessage(name, data);
                string json = JsonConvert.SerializeObject(message);

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.UTF8.GetBytes(json);

                // Send the data through the socket.  
                int bytesSent = handler.Send(msg);
            }

        }
    }
}
