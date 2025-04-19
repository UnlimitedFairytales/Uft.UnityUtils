using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Uft.UnityUtils.Common
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TVal> ToDictionary<TKey, TVal>(this List<TKey> keys, List<TVal> vals)
        {
            if (keys == null) return new Dictionary<TKey, TVal>();

            var dic = new Dictionary<TKey, TVal>();
            for (int i = 0; i < keys.Count; i++)
            {
                var val = vals != null && i < vals.Count ?
                    vals[i] :
                    default;
                dic.Add(keys[i], val);
            }
            return dic;
        }

        public static KeyValuePair<List<TKey>, List<TVal>> ToListPair<TKey, TVal>(this IDictionary<TKey, TVal> dic)
        {
            if (dic == null) return new KeyValuePair<List<TKey>, List<TVal>>(new List<TKey>(), new List<TVal>());

            var keys = dic.Keys.ToList();
            var vals = dic.Values.ToList();
            return new KeyValuePair<List<TKey>, List<TVal>>(keys, vals);
        }

        public static void AddOrSet<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key, TVal val)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = val;
            }
            else
            {
                dic.Add(key, val);
            }
        }

        public static bool ContainsAndEquals<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key, TVal val) where TVal : IEquatable<TVal>
        {
            if (dic.ContainsKey(key))
            {
                return dic[key].Equals(val);
            }
            return false;
        }

        public static ReadOnlyDictionary<TKey, TVal> CloneAsReadOnly<TKey, TVal>(this IDictionary<TKey, TVal> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TVal>(new Dictionary<TKey, TVal>(dictionary));
        }
    }
}