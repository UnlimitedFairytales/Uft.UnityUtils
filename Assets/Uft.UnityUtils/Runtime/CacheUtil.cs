using System.Collections.Generic;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class CacheUtil
    {
        // Cached

        public static T GetCachedComponent<T>(T cached, Component component) where T : Component
        {
            if (cached == null)
            {
                cached = component.GetComponent<T>();
            }
            return cached;
        }

        public static T GetCachedChildComponent<T>(T cached, Component component, string name) where T : Component
        {
            if (cached == null)
            {
                var children = component.GetComponentsInChildren<T>();
                foreach (var item in children)
                {
                    if (item.name == name)
                    {
                        cached = item;
                        break;
                    }
                }
            }
            return cached;
        }

        public static T[] GetCachedChildrenComponents<T>(T[] cached, Component component) where T : Component
        {
            cached ??= component.GetComponentsInChildren<T>();
            return cached;
        }

        // Created

        public static T GetCreatedObject<T>(T created, T prefab) where T : Object
        {
            if (created == null)
            {
                if (prefab != null)
                {
                    created = Object.Instantiate(prefab);
                }
            }
            return created;
        }

        public static List<T> GetCreatedObjectList<T>(List<T> created, List<T> prefabList) where T : Object
        {
            if (created == null)
            {
                created = new List<T>();
                if (prefabList != null)
                {
                    foreach (var prefab in prefabList)
                    {
                        created.Add(Object.Instantiate(prefab));
                    }
                }
            }
            return created;
        }
    }
}
