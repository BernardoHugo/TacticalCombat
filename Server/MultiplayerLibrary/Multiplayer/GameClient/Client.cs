using Catchy.Multiplayer.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Catchy.Multiplayer.GameClient
{
    public class Client
    {        
        private string _serverIp;

        private int _serverPort;

        private TcpConnection _tcpConnection;

        private Dictionary<string, IMessageObserver> _messageObservers;

        private List<ServerMessage> _serverMessageQueue;

        public Client(string serverIp, int serverPort)
        {
            this._serverIp = serverIp;
            this._serverPort = serverPort;
            
            _messageObservers = new Dictionary<string, IMessageObserver>();
            _serverMessageQueue = new List<ServerMessage>();
            _tcpConnection = new TcpConnection(serverIp, serverPort);
            _tcpConnection.OnMessageReceived += OnMessageReceived;
        }

        private void Update()
        {
            lock (_serverMessageQueue)
            {
                while (_serverMessageQueue.Count > 0)
                {
                    ServerMessage message = _serverMessageQueue[0];
                    if (_messageObservers.ContainsKey(message.Name))
                    {
                        _messageObservers[message.Name].OnMessageReceived(message.Data);
                    }
                    else
                    {
                        Console.WriteLine("[Client] OnMessageReceived: No observer for " + message.Name + " message");
                    }
                    _serverMessageQueue.RemoveAt(0);
                }
            }
        }

        private void OnMessageReceived(string messageJson)
        {
            lock (_serverMessageQueue)
            {
                ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(messageJson);
                _serverMessageQueue.Add(message);
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
                Console.WriteLine("[Client] AddMessageObserver: Trying to add an observer to " + messageName + " but that already has observer");
            }
        }


        public void SendMessage(ProtocolType protocolType, string name, string data)
        {
            ServerMessage serverMessage = new ServerMessage(name, data);
            string json = JsonConvert.SerializeObject(serverMessage);

            // Encode the data string into a byte array.  
            byte[] message = Encoding.UTF8.GetBytes(json);

            if (protocolType == ProtocolType.Tcp)
            {
                SendTcpMessage(message);
            }
        }

        private void SendTcpMessage(byte[] message)
        {
            _tcpConnection.SendMessage(message);
        }
    }
}