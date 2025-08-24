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

        public static List<TComponent> GetComponentsInChildrenOrderByName<TComponent>(this Component that, Func<TComponent, bool> whereCondition = null) where TComponent : Component
        {
            if (whereCondition == null)
            {
                return that.GetComponentsInChildren<TComponent>()
                    .OrderBy(component => component.gameObject.name)
                    .ToList();
            }
            else
            {
                return that.GetComponentsInChildren<TComponent>()
                    .Where(whereCondition)
                    .OrderBy(component => component.gameObject.name)
                    .ToList();
            }
        }

        public static List<TComponent> GetComponentsInChildrenByName<TComponent>(this Component that, string name) where TComponent : Component
        {
            return that.GetComponentsInChildren<TComponent>()
                .Where(component => component.gameObject.name == name)
                .ToList();
        }
    }
}
