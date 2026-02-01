#nullable enable

using UnityEngine;

namespace Uft.UnityUtils
{
    public static class CameraUtil
    {
        /// <summary>
        /// Perspectiveモードでピクセル等倍になる距離。d = h / (2 * tan(vFov/2))
        /// </summary>
        /// <param name="h"></param>
        /// <param name="vFov"></param>
        /// <param name="ppu"></param>
        /// <returns></returns>
        public static float GetPixelEqualSizeDistance(int h, float vFov, int ppu = 100)
        {
            return h / (2 * ppu * Mathf.Tan(vFov / 2 * Mathf.Deg2Rad));
        }
        public const float DISTANCE_720P_FOV60_PPU100 = 6.235383f; // 6.23538290725f;
        public const float DISTANCE1080P_FOV60_PPU100 = 9.353075f; // 9.35307436087f;
    }
}
