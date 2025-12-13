#nullable enable

using System;
using System.Collections.Generic;

namespace Uft.UnityUtils.Common
{
    public static class ThrowIf
    {
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
