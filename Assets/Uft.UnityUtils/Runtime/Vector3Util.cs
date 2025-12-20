#nullable enable

using System;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class Vector3Util
    {
        public static Vector3 GetDirection(this Vector3 from, Vector3 to, CalculationType calcType = CalculationType.Normal, bool toNormalize = true)
        {
            var vec = to - from;
            switch (calcType)
            {
                case CalculationType.Normal:
                    break;
                case CalculationType.XY:
                    vec.z = 0;
                    break;
                case CalculationType.YZ:
                    vec.x = 0;
                    break;
                case CalculationType.ZX:
                    vec.y = 0;
                    break;
                default:
                    break;
            }
            if (toNormalize)
            {
                vec = vec.normalized;
            }
            return vec;
        }

        public static float GetAngle(this Vector3 from, Vector3 to, CalculationType calcType = CalculationType.XY, bool isRadian = false)
        {
            var vec = GetDirection(from, to, calcType, false);
            var angle = calcType switch
            {
                CalculationType.XY => Mathf.Atan2(vec.y, vec.x),
                CalculationType.YZ => Mathf.Atan2(vec.z, vec.y),
                CalculationType.ZX => Mathf.Atan2(vec.x, vec.z),
                _ => throw new ArgumentException("GetAngle don't accept " + CalculationType.Normal.ToString(), nameof(calcType)),
            };
            if (!isRadian)
            {
                angle *= Mathf.Rad2Deg;
            }
            return angle;
        }
    }
}
