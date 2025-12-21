#nullable enable

using System;
using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class RectTransformUtil
    {
        /// <summary>
        /// rtの現在位置からoffsetした相対位置のワールド座標を返す。<br/>
        /// 補足:<br/>
        /// この関数はサイズ調整は関与しない。<br/>
        /// 例えば SpriteRenderer を配置することを考えた場合、<br/>
        /// - 「Canvas の RectTransform に適用されている Transform のスケール」<br/>
        /// - 「Sprite の pixelsPerUnit」<br/>
        /// の関係によって、SpriteRenderer側とCanvas内の要素の見た目のサイズ感は変化する。
        /// </summary>
        public static Vector3 GetWorldPosition(this RectTransform rt, Canvas canvasWithCamera, Vector2 offset, bool offsetsByCanvasSpace = true)
        {
            if (canvasWithCamera.renderMode == RenderMode.ScreenSpaceOverlay) throw new ArgumentException($"{nameof(canvasWithCamera)} is ScreenSpaceOverlay and cannot use worldCamera.");
            if (offset == Vector2.zero) return rt.position;

            var camera = canvasWithCamera.worldCamera;
            if (camera == null) throw new ArgumentException($"{nameof(canvasWithCamera)} does not have camera.");

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
