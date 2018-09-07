using Catchy.Multiplayer.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Catchy.Multiplayer.GameClient
{
    public class Client : MonoBehaviour
    {
        [SerializeField]
        private string serverIp;

        [SerializeField]
        private int serverPort;

        private TcpConnection tcpConnection;

        private Dictionary<string, IMessageObserver> messageObservers;

        private void Awake()
        {
            messageObservers = new Dictionary<string, IMessageObserver>();
            tcpConnection = new TcpConnection(serverIp, serverPort);
            tcpConnection.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(string messageJson)
        {
            ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(messageJson);
            if (messageObservers.ContainsKey(message.name))
            {
                messageObservers[message.name].OnServerMessageReceived(message.data);
            }
            else
            {
                Console.WriteLine("[Client] OnMessageReceived: No observer for " + message.name + " message");
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
            tcpConnection.SendMessage(message);
        }
    }
}