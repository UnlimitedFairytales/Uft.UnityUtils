#nullable enable

using System.Diagnostics;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class DevLog
    {
        public const string red = "red";
        public const string lime = "lime";
        public const string blue = "blue";

        public const string cyan = "cyan";
        public const string magenta = "magenta";
        public const string yellow = "yellow";

        public const string maroon = "maroon";
        public const string green = "#008000";
        public const string navy = "navy";

        public const string teal = "teal";
        public const string purple = "purple";
        public const string olive = "olive";

        public const string white = "white";
        public const string silver = "silver";
        public const string grey = "grey";
        public const string black = "black";

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public static void Log(string message, Object? context = null, string? color = null)
        {
            message = FixColorTag(message, color);
            UnityEngine.Debug.Log(message, context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public static void LogWarning(string message, Object? context = null, string? color = null)
        {
            message = FixColorTag(message, color);
            UnityEngine.Debug.LogWarning(message, context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public static void LogError(string message, Object? context = null, string? color = null)
        {
            message = FixColorTag(message, color);
            UnityEngine.Debug.LogError(message, context);
        }

        static string FixColorTag(string message, string? color)
        {
            if (!string.IsNullOrWhiteSpace(color))
            {
                message = $"<color={color}>{message}</color>";
            }
            return message;
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public static void LogException(System.Exception exception, Object? context = null)
        {
            UnityEngine.Debug.LogException(exception, context);
        }
    }
}
