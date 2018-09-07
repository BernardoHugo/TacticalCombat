namespace Catchy.Multiplayer.Common
{
    class ServerMessage
    {
        public string name;
        public string data;

        public ServerMessage(string name, string data)
        {
            this.name = name;
            this.data = data;
        }
    }
}
