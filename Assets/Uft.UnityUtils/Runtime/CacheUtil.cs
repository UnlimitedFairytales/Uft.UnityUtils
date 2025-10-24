using System.Collections.Generic;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class CacheUtil
    {
        // Cached

        public static T GetCachedComponent<T>(this T cached, Component component) where T : Component
        {
            if (cached == null)
            {
                cached = component.GetComponent<T>();
            }
            return cached;
        }

        public static T GetCachedChildComponent<T>(this T cached, Component component, string name) where T : Component
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

        public static T[] GetCachedChildrenComponents<T>(this T[] cached, Component component) where T : Component
        {
            cached ??= component.GetComponentsInChildren<T>();
            return cached;
        }

        // Created

        public static T GetCreatedObject<T>(this T created, T obj) where T : Object
        {
            if (created == null)
            {
                if (obj != null)
                {
                    created = Object.Instantiate(obj);
                }
            }
            return created;
        }

        public static List<T> GetCreatedObjectList<T>(this List<T> created, List<T> objList) where T : Object
        {
            if (created == null)
            {
                created = new List<T>();
                if (objList != null)
                {
                    foreach (var prefab in objList)
                    {
                        created.Add(Object.Instantiate(prefab));
                    }
                }
            }
            return created;
        }
    }
}
