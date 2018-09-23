using System.Net.Sockets;

namespace Catchy.Multiplayer.GameServer
{
    class DataState
    {
        public const int BufferSize = 1024;

        public Socket Handler = null;
        public byte[] Buffer = new byte[BufferSize];

        public DataState(Socket handler, byte[] buffer)
        {
            this.Handler = handler;
            this.Buffer = buffer;
        }
    }
}
