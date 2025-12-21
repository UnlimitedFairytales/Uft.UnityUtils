#nullable enable

using System;

namespace Uft.UnityUtils.Common
{
    public static class NumberExtension
    {
        /// <summary>
        /// Machine epsilon.<br/>* C#'s float.Epsilon is not machine epsilon.
        /// </summary>
        public const float FLOAT_EPSILON = 0.00000011920929f; // 1.1920929e-7
        public const float DEFAULT_TOLERANCE = 1e-6f;

        public static float SafeToFloat(this string text) => InvariantCultureUtil.FloatTryParse(text, out float r) ? r : 0f;

        public static int SafeToInt(this string text) => InvariantCultureUtil.IntTryParse(text, out int r) ? r : 0;

        public static float Clamp(this float value, float min, float max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static bool Approximately(float a, float b)
        {
            var diff = Math.Abs(a - b);
            var scale = Math.Max(Math.Abs(a), Math.Abs(b));
            return diff < DEFAULT_TOLERANCE * Math.Max(1.0f, scale);
        }

        public static bool ApproximatelyZero(this float value) => Approximately(value, 0f);

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }
    }
}
