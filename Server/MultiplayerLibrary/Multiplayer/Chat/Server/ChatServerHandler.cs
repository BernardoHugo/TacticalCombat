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
        private Server _gameServer;

        private const string ChatMessage = "chat_message";

        public ChatServerHandler(Server gameServer)
        {
            _gameServer = gameServer;
            gameServer.AddMessageObserver(ChatMessage, this);
        }

        public void OnMessageReceived(string data)
        {
            ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(data);
            chatMessage.Date = DateTime.Now;
            
            Console.WriteLine($"[ChatServerHandler] OnMessageReceived - Date: {chatMessage.Date}, Username: {chatMessage.Username}, Message: {chatMessage.Message} ");
            
            string chatMessageJson = JsonConvert.SerializeObject(chatMessage);
            _gameServer.SendMessageForAllClients(ProtocolType.Tcp, ChatMessage, chatMessageJson);
        }
    }
}
