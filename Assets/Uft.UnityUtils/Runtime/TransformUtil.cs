using UnityEngine;

namespace Uft.UnityUtils
{
    public static class TransformUtil
    {
        public static Vector3 GetDirection(this Transform from, Transform to)
        {
            return (to.position - from.position).normalized;
        }

        /// <summary>
        /// RotateX
        /// </summary>
        /// <param name="from"></param>
        /// <param name="angle_deg">positive is yz</param>
        /// <param name="relativeTo"></param>
        public static void RotateX(this Transform from, float angle_deg, Space relativeTo = Space.Self)
        {
            from.Rotate(angle_deg, 0, 0, relativeTo);
        }

        /// <summary>
        /// RotateY
        /// </summary>
        /// <param name="from"></param>
        /// <param name="angle_deg">positive is zx</param>
        /// <param name="relativeTo"></param>
        public static void RotateY(this Transform from, float angle_deg, Space relativeTo = Space.Self)
        {
            from.Rotate(0, angle_deg, 0, relativeTo);
        }

        /// <summary>
        /// RotateZ
        /// </summary>
        /// <param name="from"></param>
        /// <param name="angle_deg">positive is xy</param>
        /// <param name="relativeTo"></param>
        public static void RotateZ(this Transform from, float angle_deg, Space relativeTo = Space.Self)
        {
            from.Rotate(0, 0, angle_deg, relativeTo);
        }

        public static void TranslateX(this Transform from, float x, Space relativeTo = Space.Self)
        {
            from.Translate(x, 0, 0, relativeTo);
        }

        public static void TranslateY(this Transform from, float y, Space relativeTo = Space.Self)
        {
            from.Translate(0, y, 0, relativeTo);
        }

        public static void TranslateZ(this Transform from, float z, Space relativeTo = Space.Self)
        {
            from.Translate(0, 0, z, relativeTo);
        }
    }
}