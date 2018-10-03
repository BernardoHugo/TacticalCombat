using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Catchy.Multiplayer.Chat;
using Catchy.Multiplayer.Common;
using Catchy.Multiplayer.Movement.Server;

namespace Catchy.Multiplayer.GameServer
{
    public class Server
    {
        private Dictionary<int, Client> _clients = new Dictionary<int, Client>();
        private Dictionary<string, IMessageObserver> _messageObservers;

        private int _lastClientId;

        private TcpServer _tcpServer;

        private ChatServerHandler _chatServerHandler;
        private TransformServerHandler _transformServerHandler;

        private const int ServerPort = 5050;

        public Server()
        {
            _messageObservers = new Dictionary<string, IMessageObserver>();

            _tcpServer = new TcpServer(ServerPort);
            _tcpServer.OnMessageReceived += OnMessageReceived;
            _tcpServer.OnClientConnect += OnClientConnect;
            _tcpServer.OnClientDisconnect += OnClientDisconnect;

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            _chatServerHandler = new ChatServerHandler(this);
            _transformServerHandler = new TransformServerHandler(this);
        }

        public void OnClientConnect(Socket clientSocket)
        {
            IPEndPoint iP = clientSocket.RemoteEndPoint as IPEndPoint;
            _lastClientId++;

            Client newClient = new Client(_lastClientId, iP);
            newClient.SetSocket(clientSocket);
            _clients.Add(_lastClientId, newClient);
        }

        public void OnClientDisconnect(Socket clientSocket)
        {
            int id = _clients.First(x => x.Value.TcpSocket == clientSocket).Key;
            _clients.Remove(id);
        }

        private void OnMessageReceived(string messageJson)
        {
            ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(messageJson);
            if (_messageObservers.ContainsKey(message.Name))
            {
                _messageObservers[message.Name].OnMessageReceived(message.Data);
            }
            else
            {
                Console.WriteLine($"[GameServer] OnMessageReceived: No observer for {message.Name} message");
            }
        }

        public void AddMessageObserver(string messageName, IMessageObserver messageObserver)
        {
            if (!_messageObservers.ContainsKey(messageName))
            {
                _messageObservers.Add(messageName, messageObserver);
            }
            else
            {
                Console.WriteLine($"[GameServer] AddMessageObserver: Trying to add an observer to {messageName} but that already has observer");
            }
        }

        public void SendMessageForAllClients(ProtocolType protocolType, string name, string data)
        {
            foreach (KeyValuePair<int, Client> client in _clients)
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
            if (!_clients.ContainsKey(targetId))
            {
                return;
            }

            Socket handler = _clients[targetId].GetSocket(protocolType);
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
