using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class ComponentUtil
    {
        public static TComponent Instantiate<TComponent>(this TComponent original, Transform parent = null, bool worldPositionStays = true, bool? isActive = null)
            where TComponent : Component
        {
            var instantiated = UnityEngine.Object.Instantiate(original);
            if (parent != null) instantiated.transform.SetParent(parent, worldPositionStays);
            if (isActive != null) instantiated.gameObject.SetActive(isActive.Value);
            return instantiated;
        }

        public static List<TComponent> GetComponentsInChildrenOrderByName<TComponent>(this Component component, Func<TComponent, bool> whereCondition = null) where TComponent : Component
        {
            if (whereCondition == null)
            {
                return component.GetComponentsInChildren<TComponent>()
                    .OrderBy(c => c.gameObject.name)
                    .ToList();
            }
            else
            {
                return component.GetComponentsInChildren<TComponent>()
                    .Where(whereCondition)
                    .OrderBy(c => c.gameObject.name)
                    .ToList();
            }
        }

        public static List<TComponent> GetComponentsInChildrenByName<TComponent>(this Component component, string name) where TComponent : Component
        {
            return component.GetComponentsInChildren<TComponent>()
                .Where(c => c.gameObject.name == name)
                .ToList();
        }
    }
}
