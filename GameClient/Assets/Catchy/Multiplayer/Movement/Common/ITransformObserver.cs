namespace Catchy.Multiplayer.Movement.Common
{
    public interface ITransformObserver
    {
        void OnTransformReceived(TransformMessage transformMessage);
    }
}