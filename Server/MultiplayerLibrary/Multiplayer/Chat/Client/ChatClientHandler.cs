using Catchy.Multiplayer.GameClient;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using Catchy.Multiplayer.Chat.Common;
using Catchy.Multiplayer.Common;

namespace Catchy.Multiplayer.Chat
{
    public class ChatHandler : IMessageObserver
    {
        private Client gameClient;

        private const string CHAT_MESSAGE = "chat_message";

        public Action<ChatMessage> OnChatMessageReceived;

        public ChatHandler(Client gameClient)
        {
            this.gameClient = gameClient;
            gameClient.AddMessageObserver(CHAT_MESSAGE, this);
        }

        public void OnMessageReceived(string data)
        {
            ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(data);
            if (OnChatMessageReceived != null)
            {
                OnChatMessageReceived(chatMessage);
            }
        }

        public void SendChatMessage(string username, string message)
        {
            ChatMessage chatMessage = new ChatMessage(username,message);
            string chatMessageJson = JsonConvert.SerializeObject(chatMessage);
            gameClient.SendMessage(ProtocolType.Tcp, CHAT_MESSAGE, chatMessageJson);
        }
    }
}
