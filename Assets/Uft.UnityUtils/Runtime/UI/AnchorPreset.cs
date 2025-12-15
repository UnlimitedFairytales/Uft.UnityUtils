#nullable enable

using System;
using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll
    }

    public static class AnchorPresetUtil
    {
        public static bool TryParseLooseAnchorPreset(string? value, out AnchorPreset result)
        {
            value ??= "";
            if (value.Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                result = AnchorPreset.TopCenter;
                return true;
            }
            else if (value.Equals("left", StringComparison.OrdinalIgnoreCase))
            {
                result = AnchorPreset.MiddleLeft;
                return true;
            }
            else if (value.Equals("center", StringComparison.OrdinalIgnoreCase))
            {
                result = AnchorPreset.MiddleCenter;
                return true;
            }
            else if (value.Equals("right", StringComparison.OrdinalIgnoreCase))
            {
                result = AnchorPreset.MiddleRight;
                return true;
            }
            else if (value.Equals("bottom", StringComparison.OrdinalIgnoreCase))
            {
                result = AnchorPreset.BottomCenter;
                return true;
            }
            return Enum.TryParse(value, true, out result);
        }

        public static Vector2 GetPivot(this AnchorPreset anchorPreset)
        {
            return anchorPreset switch
            {
                AnchorPreset.TopLeft => new Vector2(0, 1),
                AnchorPreset.TopCenter => new Vector2(0.5f, 1),
                AnchorPreset.TopRight => new Vector2(1, 1),
                AnchorPreset.MiddleLeft => new Vector2(0, 0.5f),
                AnchorPreset.MiddleCenter => new Vector2(0.5f, 0.5f),
                AnchorPreset.MiddleRight => new Vector2(1, 0.5f),
                AnchorPreset.BottomLeft => new Vector2(0, 0),
                AnchorPreset.BottomCenter => new Vector2(0.5f, 0),
                AnchorPreset.BottomRight => new Vector2(1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(anchorPreset)),
            };
        }
    }
}
