using System;
using System.Net.Sockets;
using Catchy.Multiplayer.Chat.Common;
using Catchy.Multiplayer.Common;
using Newtonsoft.Json;

namespace Catchy.Multiplayer.Chat.Client
{
    public class ChatClientHandler : IMessageObserver
    {
        private GameClient.Client _gameClient;

        private const string ChatMessage = "chat_message";

        public Action<ChatMessage> OnChatMessageReceived;

        public ChatClientHandler(GameClient.Client gameClient)
        {
            this._gameClient = gameClient;
            gameClient.AddMessageObserver(ChatMessage, this);
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
            _gameClient.SendMessage(ProtocolType.Tcp, ChatMessage, chatMessageJson);
        }
    }
}
