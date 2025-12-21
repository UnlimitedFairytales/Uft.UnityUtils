#nullable enable

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Uft.UnityUtils.Common
{
    public static class DictionaryExtension
    {
        /// <summary>valsが足らない場合、default(T)で埋めます（参照型は null、値型は既定値）</summary>
        public static Dictionary<TKey, TVal?> ToDictionary<TKey, TVal>(this List<TKey> keys, List<TVal> vals)
        {
            var dic = new Dictionary<TKey, TVal?>();
            for (int i = 0; i < keys.Count; i++)
            {
                var val = i < vals.Count ? vals[i] : default;
                dic.Add(keys[i], val);
            }
            return dic;
        }

        public static KeyValuePair<List<TKey>, List<TVal>> ToListPair<TKey, TVal>(this IDictionary<TKey, TVal> dic)
        {
            var keys = dic.Keys.ToList();
            var vals = dic.Values.ToList();
            return new KeyValuePair<List<TKey>, List<TVal>>(keys, vals);
        }

        public static void AddOrSet<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key, TVal val)
        {
            dic[key] = val;
        }

        public static bool ContainsAndEquals<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key, TVal val)
        {
            return dic.TryGetValue(key, out var v) && EqualityComparer<TVal>.Default.Equals(v, val);
        }

        public static ReadOnlyDictionary<TKey, TVal> CloneAsReadOnly<TKey, TVal>(this IDictionary<TKey, TVal> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TVal>(new Dictionary<TKey, TVal>(dictionary));
        }
    }
}
