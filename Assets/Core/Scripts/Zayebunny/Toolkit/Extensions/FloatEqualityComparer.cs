using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Nocci.Zayebunny.Extensions
{
    [UsedImplicitly]
    internal sealed class FloatEqualityComparer : EqualityComparer<float>
    {
        public override bool Equals(float x, float y)
        {
            return GetEquatable(x) == GetEquatable(y);
        }

        public override int GetHashCode(float f)
        {
            return GetEquatable(f).GetHashCode();
        }

        private static float GetEquatable(float f)
        {
            return MathF.Round(f, 3);
        }
    }
}