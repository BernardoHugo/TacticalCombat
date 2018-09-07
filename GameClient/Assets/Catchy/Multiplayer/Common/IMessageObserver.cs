namespace Catchy.Multiplayer.Common
{
    public interface IMessageObserver
    {
        void OnServerMessageReceived(string data);
    }
}
