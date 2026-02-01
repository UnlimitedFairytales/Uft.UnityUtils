#nullable enable

using UnityEngine;

namespace Uft.UnityUtils
{
    public static class CacheUtil
    {
        const string NAME = "[" + nameof(CacheUtil) + "]";

        public static bool isHeavyCallLogged = false;

        // Cached

        public static T? GetCachedComponent<T>(ref T? cached, Component component) where T : Component
        {
            if (cached != null) return cached;
            cached = component.GetComponent<T>();
            if (isHeavyCallLogged) DevLog.Log($"{NAME} {nameof(GetCachedComponent)} cache miss, GetComponent() called.");
            return cached;
        }

        public static T? GetCachedChildComponent<T>(ref T? cached, Component component, bool includeInactive, string? name = null) where T : Component
        {
            if (cached != null) return cached;

            var children = component.GetComponentsInChildren<T>(includeInactive);
            if (isHeavyCallLogged) DevLog.Log($"{NAME} {nameof(GetCachedChildComponent)} cache miss, GetComponentsInChildren() called.");

            if (children.Length == 0) return null;
            if (children.Length == 1 && name == null)
            {
                cached = children[0];
                return cached;
            }
            if (name != null)
            {
                foreach (var item in children)
                {
                    if (item.name == name)
                    {
                        cached = item;
                        return cached;
                    }
                }
                return null;
            }
            DevLog.LogWarning($"{NAME} name == null and multiple components were found, {nameof(GetCachedChildComponent)} returns first item.");
            cached = children[0];
            return cached;
        }

        public static T[] GetCachedChildrenComponents<T>(ref T[]? cached, Component component, bool includeInactive) where T : Component
        {
            if (cached != null) return cached;

            cached = component.GetComponentsInChildren<T>(includeInactive);
            if (isHeavyCallLogged) DevLog.Log($"{NAME} {nameof(GetCachedChildrenComponents)} cache miss, GetComponentsInChildren() called.");
            return cached;
        }

        // Created

        public static T? GetCreatedObject<T>(ref T? created, T prefab) where T : Object
        {
            if (created != null) return created;

            created = Object.Instantiate(prefab);
            if (isHeavyCallLogged) DevLog.Log($"{NAME} {nameof(GetCreatedObject)} instance miss, Instantiate() called.");
            return created;
        }
    }
}
