using System;
using UnityEngine;

namespace Nocci.Player.Base
{
    public struct SpawnPointData
    {
        public bool Equals(SpawnPointData other)
        {
            return Position.Equals(other.Position) && Rotation.Equals(other.Rotation);
        }

        public override bool Equals(object obj)
        {
            return obj is SpawnPointData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Rotation);
        }

        public Vector3 Position;
        public Quaternion Rotation;

        public static SpawnPointData Default { get; } = new(Vector3.zero, Quaternion.identity);


        public SpawnPointData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public bool IsDefault()
        {
            return this == Default;
        }

        public static bool operator ==(SpawnPointData thisSpawnPoint, SpawnPointData spawnPoint)
        {
            var isEqual = (thisSpawnPoint.Position - spawnPoint.Position).sqrMagnitude < 0.1f;
            isEqual &= Quaternion.Angle(thisSpawnPoint.Rotation, spawnPoint.Rotation) < 0.1f;

            return isEqual;
        }

        public static bool operator !=(SpawnPointData thisSpawnPoint, SpawnPointData spawnPoint)
        {
            var isNotEqual = (thisSpawnPoint.Position - spawnPoint.Position).sqrMagnitude > 0.1f;
            isNotEqual |= Quaternion.Angle(thisSpawnPoint.Rotation, spawnPoint.Rotation) > 0.1f;

            return isNotEqual;
        }
    }
}