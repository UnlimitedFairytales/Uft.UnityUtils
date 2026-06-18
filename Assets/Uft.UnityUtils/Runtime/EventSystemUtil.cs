#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Uft.UnityUtils
{
    public static class EventSystemUtil
    {
        public static GameObject? Selected => EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;

        public static bool SelectedIsChanged(GameObject? previousSelectedGameObject) => Selected != previousSelectedGameObject;

        public static TItem? GetSelectedItem<TItem>(IReadOnlyList<TItem> itemList) where TItem : Component
            => GetSelectedItemInner<TItem, Component>(itemList, null);

        public static TItem? GetSelectedItem<TItem, TComponent>(IReadOnlyList<TItem> itemList, Func<TItem, TComponent> componentGetter)
            where TItem : Component
            where TComponent : Component
            => GetSelectedItemInner(itemList, componentGetter);

        static TItem? GetSelectedItemInner<TItem, TComponent>(IReadOnlyList<TItem> itemList, Func<TItem, TComponent>? componentGetter)
            where TItem : Component
            where TComponent : Component
        {
            var currentGo = Selected;
            if (currentGo == null) return null;

            for (int i = 0; i < itemList.Count; i++)
            {
                var go = componentGetter != null ? componentGetter(itemList[i]).gameObject : itemList[i].gameObject;
                if (go == currentGo)
                {
                    return itemList[i];
                }
            }
            return null;
        }
    }
}
