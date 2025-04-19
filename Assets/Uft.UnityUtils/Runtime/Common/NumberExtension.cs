using System;
using System.Globalization;

namespace Uft.UnityUtils.Common
{
    public static class NumberExtension
    {
        /// <summary>
        /// Machine epsilon.<br/>* C#'s float.Epsilon is not machine epsilon.
        /// </summary>
        public static float FloatEpsilon { get { return 0.00000011920929f; } } // 1.1920929e-7

        public static float SafeToFloat(this string text)
        {
            float.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out float returnValue);
            return returnValue;
        }

        public static int SafeToInt(this string text)
        {
            int.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out int returnValue);
            return returnValue;
        }

        public static float Clamp(this float value, float min, float max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static bool Is0fEpsilon(this float value)
        {
            return -FloatEpsilon < value && value < FloatEpsilon;
        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }
    }
}