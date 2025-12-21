#nullable enable

using System;
using System.Collections.Generic;
using System.Data;

namespace Uft.UnityUtils.Common
{
    public class Eval
    {
        public static class Keyword
        {
            public const string TRUE = "TRUE";
            public const string FALSE = "FALSE";
            public const string IS = "IS";
            public const string NULL = "NULL";
            public const string AND = "AND";
            public const string OR = "OR";
            public const string NOT = "NOT";
            public const string LIKE = "LIKE";
            public const string IN = "IN";

            static readonly HashSet<string> _all =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    TRUE, FALSE, IS, NULL, AND, OR, NOT, LIKE, IN
                };
            public static IReadOnlyCollection<string> All => _all;
            public static bool IsKeyword(string token) => !string.IsNullOrWhiteSpace(token) && _all.Contains(token.Trim());
        }
        public static class RegexPattern
        {
            public const string IDENTIFIER = @"[A-Za-z_][A-Za-z0-9_]*";
        }

        readonly object _lock = new();
        readonly DataTable _dataTable = new();

        /// <summary>
        /// 簡易的な式評価。より実用的・高速な機能が欲しい場合は、NCalc（注意: MathHelperがdynamicなため、代替実装が必要）などを使用してください
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool? EvaluateBooleanOrNull(string expression)
        {
            object? result;
            lock (this._lock)
            {
                result = this._dataTable.Compute(expression, null);
            }
            if (result is DBNull || result == null) return null;
            if (result is bool v) return v;
            throw new InvalidOperationException(expression);
        }

        /// <summary>
        /// 簡易的な式評価。より実用的・高速な機能が欲しい場合は、NCalc（注意: MathHelperがdynamicなため、代替実装が必要）などを使用してください
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public double? EvaluateDoubleOrNull(string expression)
        {
            object? result;
            lock (this._lock)
            {
                result = this._dataTable.Compute(expression, null);
            }
            if (result is DBNull || result == null) return null;
            return result switch
            {
                decimal m => (double)m,
                double d => d,
                float f => f,
                int i => i,
                long l => l,
                short s => s,
                byte b => b,
                _ => throw new InvalidOperationException(expression),
            };
        }
    }
}
