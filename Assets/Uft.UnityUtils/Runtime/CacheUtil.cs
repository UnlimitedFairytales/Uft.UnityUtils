#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class CacheUtil
    {
        // Cached

        public static T? GetCachedComponent<T>(T? cached, Component component) where T : Component
        {
            if (cached != null) return cached;
            return component.GetComponent<T>();
        }

        public static T? GetCachedChildComponent<T>(T? cached, Component component, bool includeInactive, string? name = null) where T : Component
        {
            if (cached != null) return cached;

            var children = component.GetComponentsInChildren<T>(includeInactive);
            if (children.Length == 0) return null;
            if (children.Length == 1) return children[0];
            if (name != null)
            {
                foreach (var item in children)
                {
                    if (item.name == name) return item;
                }
                return null;
            }
            DevLog.LogWarning($"[{nameof(CacheUtil)}] Name == null and multiple components were found,  {nameof(GetCachedChildComponent)} returns first item.");
            return children[0];
        }

        public static T[] GetCachedChildrenComponents<T>(T[]? cached, Component component, bool includeInactive) where T : Component
        {
            if (cached != null) return cached;

            return component.GetComponentsInChildren<T>(includeInactive);
        }

        // Created

        public static T? GetCreatedObject<T>(T? created, T? prefab) where T : Object
        {
            if (created != null) return created;

            if (prefab != null) return Object.Instantiate(prefab);
            return null;
        }

        public static List<T> GetCreatedObjectList<T>(List<T>? created, List<T>? prefabList) where T : Object
        {
            if (created != null) return created;

            created = new List<T>();
            if (prefabList != null)
            {
                foreach (var prefab in prefabList)
                {
                    if (prefab == null) continue;
                    created.Add(Object.Instantiate(prefab));
                }
            }
            return created;
        }
    }
}
