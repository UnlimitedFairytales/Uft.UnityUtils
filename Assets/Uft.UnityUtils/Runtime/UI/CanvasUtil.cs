using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class CanvasUtil
    {
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
