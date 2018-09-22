namespace Catchy.Multiplayer.Common
{
    public interface IMessageObserver
    {
        void OnMessageReceived(string data);
    }
}
