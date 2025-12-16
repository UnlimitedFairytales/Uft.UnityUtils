#nullable enable

using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public static class TextUtil
    {
        enum Vertical
        {
            Top = 1,
            Middle = 2,
            Bottom = 3,
        }

        enum Horizontal
        {
            Left = 1,
            Center = 2,
            Right = 3,
        }

        public static Text CreateTextWithCanvas(string text, int width = 400, int height = 400)
        {
            var canvasGo = new GameObject("DynamicCanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var textGo = new GameObject("DynamicText");
            textGo.transform.SetParent(canvasGo.transform, false);
            var result = textGo.AddComponent<Text>();
            result.font = (Font)Resources.GetBuiltinResource(typeof(Font), "LegacyRuntime.ttf"); // 2022.3.1以降はこちら指定
            result.color = (new Color(50 / 255f, 50 / 255f, 50 / 255f, 1));
            result.rectTransform.sizeDelta = new Vector2(width, height);
            result.text = text;
            return result;
        }

        public static void Arrange(this Text text, AnchorPreset anchorPreset = AnchorPreset.MiddleLeft, bool adjustsPivot = true, bool adjustsVerticalTextAlign = true)
        {
            switch (anchorPreset)
            {
                case (AnchorPreset.TopLeft):
                    SetDetailA(text, new Vector2(0, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.TopCenter):
                    SetDetailA(text, new Vector2(0.5f, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.TopRight):
                    SetDetailA(text, new Vector2(1, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.MiddleLeft):
                    SetDetailA(text, new Vector2(0, 0.5f), adjustsPivot, adjustsVerticalTextAlign, Vertical.Middle);
                    break;
                case (AnchorPreset.MiddleCenter):
                    SetDetailA(text, new Vector2(0.5f, 0.5f), adjustsPivot, adjustsVerticalTextAlign, Vertical.Middle);
                    break;
                case (AnchorPreset.MiddleRight):
                    SetDetailA(text, new Vector2(1, 0.5f), adjustsPivot, adjustsVerticalTextAlign, Vertical.Middle);
                    break;
                case (AnchorPreset.BottomLeft):
                    SetDetailA(text, new Vector2(0, 0), adjustsPivot, adjustsVerticalTextAlign, Vertical.Bottom);
                    break;
                case (AnchorPreset.BottomCenter):
                    SetDetailA(text, new Vector2(0.5f, 0), adjustsPivot, adjustsVerticalTextAlign, Vertical.Bottom);
                    break;
                case (AnchorPreset.BottomRight):
                    SetDetailA(text, new Vector2(1, 0), adjustsPivot, adjustsVerticalTextAlign, Vertical.Bottom);
                    break;
                case (AnchorPreset.HorStretchTop):
                    SetDetailB(text, new Vector2(0, 1), new Vector2(1, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.HorStretchMiddle):
                    SetDetailB(text, new Vector2(0, 0.5f), new Vector2(1, 0.5f), adjustsPivot, adjustsVerticalTextAlign, Vertical.Middle);
                    break;
                case (AnchorPreset.HorStretchBottom):
                    SetDetailB(text, new Vector2(0, 0), new Vector2(1, 0), adjustsPivot, adjustsVerticalTextAlign, Vertical.Bottom);
                    break;
                case (AnchorPreset.VertStretchLeft):
                    SetDetailB(text, new Vector2(0, 0), new Vector2(0, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.VertStretchCenter):
                    SetDetailB(text, new Vector2(0.5f, 0), new Vector2(0.5f, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.VertStretchRight):
                    SetDetailB(text, new Vector2(1, 0), new Vector2(1, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
                case (AnchorPreset.StretchAll):
                    SetDetailB(text, new Vector2(0, 0), new Vector2(1, 1), adjustsPivot, adjustsVerticalTextAlign, Vertical.Top);
                    break;
            }
        }

        static void SetDetailA(Text text, Vector2 v, bool adjustsPivot, bool adjustsVerticalAlign, Vertical textAlign)
        {
            var source = text.rectTransform;
            source.anchorMin = v;
            source.anchorMax = v;
            if (adjustsPivot)
            {
                source.pivot = v;
            }
            if (adjustsVerticalAlign)
            {
                var srcAlign = text.alignment;
                var hor =
                    (srcAlign == TextAnchor.LowerLeft || srcAlign == TextAnchor.MiddleLeft || srcAlign == TextAnchor.UpperLeft) ? Horizontal.Left :
                    (srcAlign == TextAnchor.LowerCenter || srcAlign == TextAnchor.MiddleCenter || srcAlign == TextAnchor.UpperCenter) ? Horizontal.Center :
                    Horizontal.Right;
                if (hor == Horizontal.Left)
                {
                    text.alignment =
                        textAlign == Vertical.Top ? TextAnchor.UpperLeft :
                        textAlign == Vertical.Middle ? TextAnchor.MiddleLeft :
                        TextAnchor.LowerLeft;
                }
                if (hor == Horizontal.Center)
                {
                    text.alignment =
                        textAlign == Vertical.Top ? TextAnchor.UpperCenter :
                        textAlign == Vertical.Middle ? TextAnchor.MiddleCenter :
                        TextAnchor.LowerCenter;
                }
                if (hor == Horizontal.Right)
                {
                    text.alignment =
                        textAlign == Vertical.Top ? TextAnchor.UpperRight :
                        textAlign == Vertical.Middle ? TextAnchor.MiddleRight :
                        TextAnchor.LowerRight;
                }
            }
        }

        static void SetDetailB(Text text, Vector2 min, Vector2 max, bool adjustsPivot, bool adjustsVerticalAlign, Vertical textAlign)
        {
            var source = text.rectTransform;
            source.anchorMin = min;
            source.anchorMax = max;
            if (adjustsPivot)
            {
                source.pivot = new Vector2((min.x + max.x) / 2.0f, (min.y + max.y) / 2.0f);
            }
            if (min.x != max.x)
            {
                source.sizeDelta = new Vector2(0, source.sizeDelta.y);
            }
            if (min.y != max.y)
            {
                source.sizeDelta = new Vector2(source.sizeDelta.x, 0);
            }
            if (adjustsVerticalAlign)
            {
                var srcAlign = text.alignment;
                var hor =
                    (srcAlign == TextAnchor.LowerLeft || srcAlign == TextAnchor.MiddleLeft || srcAlign == TextAnchor.UpperLeft) ? Horizontal.Left :
                    (srcAlign == TextAnchor.LowerCenter || srcAlign == TextAnchor.MiddleCenter || srcAlign == TextAnchor.UpperCenter) ? Horizontal.Center :
                    Horizontal.Right;
                if (hor == Horizontal.Left)
                {
                    text.alignment =
                        textAlign == Vertical.Top ? TextAnchor.UpperLeft :
                        textAlign == Vertical.Middle ? TextAnchor.MiddleLeft :
                        TextAnchor.LowerLeft;
                }
                if (hor == Horizontal.Center)
                {
                    text.alignment =
                        textAlign == Vertical.Top ? TextAnchor.UpperCenter :
                        textAlign == Vertical.Middle ? TextAnchor.MiddleCenter :
                        TextAnchor.LowerCenter;
                }
                if (hor == Horizontal.Right)
                {
                    text.alignment =
                        textAlign == Vertical.Top ? TextAnchor.UpperRight :
                        textAlign == Vertical.Middle ? TextAnchor.MiddleRight :
                        TextAnchor.LowerRight;
                }
            }
        }
    }
}
