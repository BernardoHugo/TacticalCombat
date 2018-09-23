using System;
using UnityEngine.Serialization;

namespace Catchy.Multiplayer.Common
{
    [Serializable]
    public struct SerializableVector3
    {
        [FormerlySerializedAs("x")] public float X;
        [FormerlySerializedAs("y")] public float Y;
        [FormerlySerializedAs("z")] public float Z;

        public SerializableVector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}