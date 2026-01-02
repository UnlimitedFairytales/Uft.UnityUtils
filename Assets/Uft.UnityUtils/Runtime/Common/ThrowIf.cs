#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Uft.UnityUtils.Common
{
    public static class ThrowIf
    {
        /// <summary>
        /// Wrapper property（例: Foo => _foo）を想定。callerMemberName(= Foo)から規約でfield名を推定して例外に含める。<br/>
        /// UnityEngine.Objectに限定していないため、演算子オーバーロードは呼ばれず擬似 null（破棄済み）は検出できない。
        /// </summary>
        public static T Unassigned<T>(T? obj, [CallerMemberName] string? callerMemberName = null)
            where T : class
        {
            if (obj != null) return obj;

            var name = string.IsNullOrEmpty(callerMemberName) ? "unknown" :  Reflec.ToFieldName(callerMemberName);
            throw new InvalidOperationException($"{name} is required. (caller: {callerMemberName ?? "unknown"})");
        }

        public static void NullOrEmpty(string? str, string paramName)
        {
            if (str == null) throw new ArgumentNullException(paramName);
            if (str.Length == 0) throw new ArgumentException("String is empty.", paramName);
        }

        public static void NullOrEmpty<T>(IReadOnlyCollection<T>? collection, string paramName)
        {
            if (collection == null) throw new ArgumentNullException(paramName);
            if (collection.Count == 0) throw new ArgumentException("Collection is empty.", paramName);
        }
    }
}
