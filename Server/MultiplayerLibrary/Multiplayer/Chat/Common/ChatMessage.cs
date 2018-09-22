using System;

namespace Catchy.Multiplayer.Chat.Common
{
    public struct ChatMessage
    {
        public DateTime date;

        public string username;

        public string message;

        public ChatMessage(string username, string message)
        {
            this.username = username;
            this.message = message;
            this.date = new DateTime();
        }

        public ChatMessage(DateTime date, string username, string message)
        {
            this.date = date;
            this.username = username;
            this.message = message;
        }
    }
}
