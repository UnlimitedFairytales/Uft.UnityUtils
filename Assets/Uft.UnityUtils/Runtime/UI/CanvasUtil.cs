#nullable enable

using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class CanvasUtil
    {
        public static void SetCameraToCanvas(this GameObject gameObject, Camera camera, bool overlay2Camera = true, bool includeInactive = false)
        {
            var canvases = gameObject.GetComponentsInChildren<Canvas>(includeInactive);
            if (canvases == null) return;

            foreach (var canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    if (!overlay2Camera) continue;
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = camera;
                    canvas.planeDistance = camera.nearClipPlane + 0.01f;
                }
                canvas.worldCamera = camera;
            }
        }
    }
}
