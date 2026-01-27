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

        /// <summary>
        /// World座標をRectTransformのLocal座標に変換する。
        /// </summary>
        /// <remarks>
        /// - World -> Screen -> RectTransformLocalと2段階変換して求めている。<br/>
        /// - CanvasScalerも自動的に考慮される。<br/>
        /// - 典型的な用途では、targetRectのPivot位置と計算結果を使用する対象のPivot位置の実座標を同一にしておく。(このようにしておくと、単にanchoredPositionに計算結果を渡せば良い。)<br/>
        /// </remarks>
        /// <param name="worldCamera">World -> Screen のためのカメラ</param>
        /// <param name="worldPos">元のWorld座標</param>
        /// <param name="canvas">Screen -> RectTransformLocal のための Canvas</param>
        /// <param name="targetRect">Screen -> RectTransformLocal のための rect。通常は、座標設定したいノードの親ノードのRectTransformを与える。</param>
        /// <param name="localPoint">tagetRectのPivot位置からのRect相対座標。通常は、親(targetRect)と子(座標指定したいノード)のPivot位置を同一座標になるようにしておく。この場合なら、そのままanchoredPositionに渡せる値になる。</param>
        /// <returns></returns>
        public static bool WorldToLocalPointInRect(this Camera worldCamera, Vector3 worldPos, Canvas canvas, RectTransform targetRect, out Vector2 localPoint)
        {
            localPoint = Vector2.zero;

            // 1. world -> screen
            Vector3 screen = worldCamera.WorldToScreenPoint(worldPos);
            if (screen.z < 0f) return false;

            // 2. screen -> rectLocal
            Camera? canvasCamera = canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetRect,
                screen,
                canvasCamera,
                out localPoint);
        }
    }
}
