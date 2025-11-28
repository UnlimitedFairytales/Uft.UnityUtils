#nullable enable

using System.Globalization;

namespace Uft.UnityUtils.Common
{
    public static class InvariantCultureUtil
    {
        /// <summary>OK : +1234.5<br/>NG : +1,234.5</summary>
        public static readonly NumberStyles Style = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;

        public static bool IntTryParse(string? s, out int result)
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        public static bool FloatTryParse(string? s, out float result)
        {
            return float.TryParse(s, Style, CultureInfo.InvariantCulture, out result);
        }

        public static bool DoubleTryParse(string? s, out double result)
        {
            return double.TryParse(s, Style, CultureInfo.InvariantCulture, out result);
        }

        public static bool DecimalTryParse(string? s, out decimal result)
        {
            return decimal.TryParse(s, Style, CultureInfo.InvariantCulture, out result);
        }

        public static string ToInvariantString(this float value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ToInvariantString(this double value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ToInvariantString(this decimal value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
