using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class CanvasUtil
    {
        /// <summary>
        /// ピクセル等倍になる距離。d = h / (2 * tan(vFov/2))
        /// </summary>
        /// <param name="h"></param>
        /// <param name="vFov"></param>
        /// <returns></returns>
        public static float GetPixelEqualSizeDistance(int h, float vFov)
        {
            return h / (2 * Mathf.Tan(vFov / 2 * Mathf.Deg2Rad));
        }
        public const float DISTANCE_720P_FOV60 = 623.538290725f;
        public const float DISTANCE1080P_FOV60 = 935.307436087f;

        public static void SetCameraToCanvas(this GameObject gameObject, Camera camera, bool overlay2Camera = true)
        {
            if (gameObject == null) return;
            if (camera == null) return;
            var canvases = gameObject.GetComponentsInChildren<Canvas>();
            if (canvases == null) return;

            foreach (var canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    if (!overlay2Camera) break;
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                }
                canvas.worldCamera = camera;
            }
        }
    }
}
