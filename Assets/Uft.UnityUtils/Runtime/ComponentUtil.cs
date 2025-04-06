using UnityEngine;

namespace Uft.UnityUtils
{
    public static class ComponentUtil
    {
        public static TComponent Instantiate<TComponent>(this TComponent original, Transform parent = null, bool worldPositionStays = true, bool? isActive = null)
            where TComponent : Component
        {
            var instantiated = Object.Instantiate(original);
            if (parent != null) instantiated.transform.SetParent(parent, worldPositionStays);
            if (isActive != null) instantiated.gameObject.SetActive(isActive.Value);
            return instantiated;
        }
    }
}
