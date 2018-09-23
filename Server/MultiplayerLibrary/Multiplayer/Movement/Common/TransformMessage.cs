using Catchy.Multiplayer.Common;

namespace Catchy.Multiplayer.Movement.Common
{
    public struct TransformMessage
    {
        public int Id;

        public SerializableTransform Transform;

        public TransformMessage(int id, SerializableTransform transform)
        {
            this.Id = id;
            this.Transform = transform;
        }
    }
}