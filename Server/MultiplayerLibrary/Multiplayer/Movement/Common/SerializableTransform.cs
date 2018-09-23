using System;
using Catchy.Multiplayer.Common;
using UnityEngine.Serialization;

namespace Catchy.Multiplayer.Movement.Common
{
    [Serializable]
    public struct SerializableTransform
    {
        public SerializableVector3 Position;

        public SerializableVector3 Rotation;
    }
}