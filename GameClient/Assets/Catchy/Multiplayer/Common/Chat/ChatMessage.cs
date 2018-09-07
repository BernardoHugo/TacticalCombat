using System;

namespace Catchy.Multiplayer.Common.Chat
{
    public class ChatMessage
    {
        public DateTime date;

        public string username;

        public string message;

        public ChatMessage(string username, string message)
        {
            this.username = username;
            this.message = message;
        }
    }
}
