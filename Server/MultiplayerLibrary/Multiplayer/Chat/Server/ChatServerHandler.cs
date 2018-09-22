using System;
using System.Net.Sockets;
using Catchy.Multiplayer.Chat.Common;
using Catchy.Multiplayer.Common;
using Catchy.Multiplayer.GameServer;
using Newtonsoft.Json;

namespace Catchy.Multiplayer.Chat
{
    class ChatServerHandler : IMessageObserver
    {
        private Server gameServer;

        private const string CHAT_MESSAGE = "chatMessage";

        public ChatServerHandler(Server gameServer)
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
