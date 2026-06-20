#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class ComponentUtil
    {
        public static TComponent Instantiate<TComponent>(this TComponent original, Transform? parent = null, bool worldPositionStays = true, bool? isActive = null)
            where TComponent : Component
        {
            var instantiated = UnityEngine.Object.Instantiate(original);
            if (parent != null) instantiated.transform.SetParent(parent, worldPositionStays);
            if (isActive != null) instantiated.gameObject.SetActive(isActive.Value);
            return instantiated;
        }

        // TODO: sample

        public static TComponent? GetComponentOnSceneRoot<TComponent>(this Component component) where TComponent : Component
        {
            var objects = component.gameObject.scene.GetRootGameObjects();
            var myRoot = component.transform.root.gameObject;
            for (int i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];
                if (obj == myRoot) continue;
                var cmp = obj.GetComponent<TComponent>();
                if (cmp != null) return cmp;
            }
            return null;
        }

        public static TComponent? GetComponentInChildrenByName<TComponent>(this Component component, string name, bool includeInactive = false) where TComponent : Component
        {
            return component.GetComponentsInChildren<TComponent>(includeInactive)
                .FirstOrDefault(c => c.gameObject.name == name);
        }

        public static List<TComponent> GetComponentsInChildrenOrderByName<TComponent>(this Component component, bool includeInactive = false, Func<TComponent, bool>? whereCondition = null) where TComponent : Component
        {
            if (whereCondition == null)
            {
                return component.GetComponentsInChildren<TComponent>(includeInactive)
                    .OrderBy(c => c.gameObject.name)
                    .ToList();
            }
            else
            {
                return component.GetComponentsInChildren<TComponent>(includeInactive)
                    .Where(whereCondition)
                    .OrderBy(c => c.gameObject.name)
                    .ToList();
            }
        }
    }
}
