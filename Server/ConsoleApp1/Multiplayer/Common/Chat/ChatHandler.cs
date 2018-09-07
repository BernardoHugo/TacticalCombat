using Catchy.Multiplayer.GameServer;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;

namespace Catchy.Multiplayer.Common.Chat
{
    class ChatHandler : IMessageObserver
    {
        private Server gameServer;

        private const string CHAT_MESSAGE = "chatMessage";

        public ChatHandler(Server gameServer)
        {
            this.gameServer = gameServer;
            gameServer.AddMessageObserver(CHAT_MESSAGE, this);
        }

        public void OnMessageReceived(string data)
        {
            ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(data);
            chatMessage.date = DateTime.Now;
            string chatMessageJson = JsonConvert.SerializeObject(chatMessage);
            gameServer.SendMessageForAllClients(ProtocolType.Tcp, CHAT_MESSAGE, chatMessageJson);
        }
    }
}
