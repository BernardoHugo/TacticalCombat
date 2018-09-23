using System;

namespace Catchy.Multiplayer.Chat.Common
{
    public struct ChatMessage
    {
        public DateTime Date;

        public string Username;

        public string Message;

        public ChatMessage(string username, string message)
        {
            this.Username = username;
            this.Message = message;
            this.Date = new DateTime();
        }

        public ChatMessage(DateTime date, string username, string message)
        {
            this.Date = date;
            this.Username = username;
            this.Message = message;
        }
    }
}
