#nullable enable

using System.Diagnostics;
using UnityEngine;

namespace Uft.UnityUtils
{
    public class DevLogWithTag
    {
        readonly string _tag = "";
        public DevLogWithTag(string tag) => this._tag = tag;

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public void Log(string message, Object? context = null, string? color = null) => DevLog.Log(this._tag + " " + message, context, color);

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public void LogWarning(string message, Object? context = null, string? color = null) => DevLog.LogWarning(this._tag + " " + message, context, color);

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public void LogError(string message, Object? context = null, string? color = null) => DevLog.LogError(this._tag + " " + message, context, color);

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        public void LogException(System.Exception exception, Object? context = null) => DevLog.LogException(exception, context);
    }
}
