using System.Net.Sockets;

namespace ConsoleApp1
{
    public class DataState
    {
        public const int BUFFER_SIZE = 1024;

        public Socket handler = null;
        public byte[] buffer = new byte[BUFFER_SIZE];

        public DataState(Socket handler, byte[] buffer)
        {
            this.handler = handler;
            this.buffer = buffer;
        }
    }
}
