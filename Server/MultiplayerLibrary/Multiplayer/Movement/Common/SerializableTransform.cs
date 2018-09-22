using System;
using Catchy.Multiplayer.Common;

namespace Catchy.Multiplayer.Movement.Common
{
    [Serializable]
    public struct SerializableTransform
    {
        private SerializableVector3 position;

        private SerializableVector3 rotation;
    }
}