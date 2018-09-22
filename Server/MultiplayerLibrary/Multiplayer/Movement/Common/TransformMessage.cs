using Catchy.Multiplayer.Common;

namespace Catchy.Multiplayer.Movement.Common
{
    public struct TransformMessage
    {
        public int id;

        public SerializableTransform transform;

        public TransformMessage(int id, SerializableTransform transform)
        {
            this.id = id;
            this.transform = transform;
        }
    }
}