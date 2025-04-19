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
            var angle = 0f;
            switch (calcType)
            {
                case CalculationType.Normal:
                    throw new Exception("GetAngle don't accept " + CalculationType.Normal.ToString());
                case CalculationType.XY:
                    angle = Mathf.Atan2(vec.y, vec.x);
                    break;
                case CalculationType.YZ:
                    angle = Mathf.Atan2(vec.z, vec.y);
                    break;
                case CalculationType.ZX:
                    angle = Mathf.Atan2(vec.x, vec.z);
                    break;
            }
            if (!isRadian)
            {
                angle *= Mathf.Rad2Deg;
            }
            return angle;
        }
    }
}