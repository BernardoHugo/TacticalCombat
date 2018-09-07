namespace Catchy.Multiplayer.Common
{
    interface IMessageObserver
    {
        void OnMessageReceived(string data);
    }
}
