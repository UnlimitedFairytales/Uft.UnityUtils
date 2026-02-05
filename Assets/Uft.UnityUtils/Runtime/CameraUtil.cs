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
        public const float DISTANCE_720P_FOV60_PPU100 = 6.2353829f; // 6.23538290725f;
        public const float DISTANCE1080P_FOV60_PPU100 = 9.3530743f; // 9.35307436087f;
        public const float DISTANCE1080P_FOV45_PPU100 = 13.036753f; // 13.0367532368f;
        public const float DISTANCE1080P_FOV10_PPU100 = 61.722282f; // 61.7222824349f;
    }
}
