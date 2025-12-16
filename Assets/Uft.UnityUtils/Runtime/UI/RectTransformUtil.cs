#nullable enable

using System;
using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class RectTransformUtil
    {
        public static Vector3 GetWorldPosition(this RectTransform rt, Canvas canvasWithCamera, Vector2 offset, bool offsetsByCanvasSpace = true)
        {
            if (canvasWithCamera.renderMode == RenderMode.ScreenSpaceOverlay) throw new ArgumentException($"[{nameof(RectTransformUtil)}] {nameof(canvasWithCamera)} is ScreenSpaceOverlay and cannot use worldCamera.");
            if (offset == Vector2.zero) return rt.position;

            var camera = canvasWithCamera.worldCamera;
            if (camera == null) throw new ArgumentException($"[{nameof(RectTransformUtil)}] {nameof(canvasWithCamera)} does not have camera.");

            var screenPos = RectTransformUtility.WorldToScreenPoint(camera, rt.position);
            if (!offsetsByCanvasSpace)
            {
                screenPos += offset;
            }
            var canvasRect = (RectTransform)canvasWithCamera.transform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                camera,
                out Vector2 canvasLocal);
            if (offsetsByCanvasSpace)
            {
                canvasLocal += offset;
            }
            return canvasRect.TransformPoint(canvasLocal);
        }
    }
}
