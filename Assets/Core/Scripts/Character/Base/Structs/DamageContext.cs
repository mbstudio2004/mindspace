﻿using UnityEngine;

namespace Nocci.Character.Structs
{
    public struct DamageContext
    {
        public static DamageContext Default { get; } = new();

        public readonly ICharacter Source;
        public readonly Vector3 HitPoint;
        public readonly Vector3 HitForce;
        public readonly Vector3 HitNormal;
        public readonly DamageType DamageType;


        public override string ToString()
        {
            return $"DamageContext: {DamageType} - {Source} - {HitPoint} - {HitForce} - {HitNormal}";
        }


        public DamageContext(ICharacter source)
        {
            DamageType = DamageType.Generic;
            HitPoint = HitForce = HitNormal = Vector3.zero;

            Source = source;
        }

        public DamageContext(DamageType damageType, ICharacter source)
        {
            DamageType = damageType;
            HitPoint = HitForce = HitNormal = Vector3.zero;

            Source = source;
        }

        public DamageContext(Vector3 hitPoint, Vector3 hitForce, Vector3 hitNormal, ICharacter source = null)
        {
            DamageType = DamageType.Generic;

            HitPoint = hitPoint;
            HitForce = hitForce;
            HitNormal = hitNormal;

            Source = source;
        }

        public DamageContext(DamageType damageType, Vector3 hitPoint, Vector3 hitDirection, Vector3 hitNormal,
            ICharacter source = null)
        {
            DamageType = damageType;

            HitPoint = hitPoint;
            HitForce = hitDirection;
            HitNormal = hitNormal;

            Source = source;
        }
    }


    public enum DamageType
    {
        Generic,
        Cut,
        Hit,
        Stab,
        Bullet,
        Explosion,
        Fire
    }
}