namespace Catchy.Multiplayer.Common
{
    class ServerMessage
    {
        public string Name;
        public string Data;

        public ServerMessage(string name, string data)
        {
            this.Name = name;
            this.Data = data;
        }
    }
}
