#nullable enable

using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class CanvasUtil
    {
        /// <summary>子要素含めて探索して該当するCanvas全てにカメラを設定する。</summary>
        /// <remarks>renderMode、planeDistance、worldCamera変更後の再計算タイミングは不定。Transformの各値を直接利用するには、1F待つのが実用解。</remarks>
        public static void SetCameraToCanvas(this GameObject gameObject, Camera camera, bool overlay2Camera = true, float? overlay2CameraDistance = null, bool includeInactive = false)
        {
            var canvases = gameObject.GetComponentsInChildren<Canvas>(includeInactive);
            if (canvases == null) return;

            foreach (var canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    if (!overlay2Camera) continue;
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.planeDistance = overlay2CameraDistance != null ?
                        overlay2CameraDistance.Value :
                        camera.nearClipPlane + 0.01f;
                }
                canvas.worldCamera = camera;
            }
        }
    }
}
