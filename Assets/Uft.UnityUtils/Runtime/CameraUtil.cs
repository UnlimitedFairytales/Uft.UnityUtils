using UnityEngine;

namespace Uft.UnityUtils
{
    public static class CameraUtil
    {
        /// <summary>
        /// Perspectiveモード1PPUでピクセル等倍になる距離。d = h / (2 * tan(vFov/2))
        /// </summary>
        /// <param name="h"></param>
        /// <param name="vFov"></param>
        /// <returns></returns>
        public static float GetPixelEqualSizeDistance(int h, float vFov)
        {
            return h / (2 * Mathf.Tan(vFov / 2 * Mathf.Deg2Rad));
        }
        public const float DISTANCE_720P_FOV60 = 623.5383f; // 623.538290725f;
        public const float DISTANCE1080P_FOV60 = 935.3075f; // 935.307436087f;
    }
}
