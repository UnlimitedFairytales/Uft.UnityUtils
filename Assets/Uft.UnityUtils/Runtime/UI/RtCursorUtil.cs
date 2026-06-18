#nullable enable

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public static class RtCursorUtil
    {
        static readonly DevLogWithTag DevLog = new("[" + nameof(RtCursorUtil) + "]");

        /// <summary>特定の構造を前提とした一連のメニュー項目に対して、Cursor表示オブジェクトの表示/非表示を切り替えるための関数。EventSystemで選択項目を検索して返す。</summary>
        /// <remarks>
        /// 1. Cursor表示オブジェクトは、各メニュー項目の子オブジェクトとして存在し、同一の操作でアクセスできる必要があります。<br/>
        /// 2. 現在選択中のメニュー項目のCursor表示オブジェクトのみがActiveで、他のメニュー項目のCursor表示オブジェクトは非Activeとします。<br/>
        /// </remarks>
        public static TItem? UpdateCursor<TItem>(
            IReadOnlyList<TItem> items,
            TItem? prevItem,
            Func<TItem, RectTransform>? cursorGetter,
            string doTweenId = "CursorMove",
            float duration = 0.3f,
            string cursorGameObjectName = "Cursor")
            where TItem : Component
            => UpdateCursorInner<TItem, RectTransform>(items, prevItem, null, cursorGetter, doTweenId, duration, cursorGameObjectName);

        /// <summary>特定の構造を前提とした一連のメニュー項目に対して、Cursor表示オブジェクトの表示/非表示を切り替えるための関数。EventSystemで選択項目を検索して返す。</summary>
        /// <remarks>
        /// 1. Cursor表示オブジェクトは、各メニュー項目の子オブジェクトとして存在し、同一の操作でアクセスできる必要があります。<br/>
        /// 2. 現在選択中のメニュー項目のCursor表示オブジェクトのみがActiveで、他のメニュー項目のCursor表示オブジェクトは非Activeとします。<br/>
        /// </remarks>
        public static TItem? UpdateCursor<TItem, TComponent>(
            IReadOnlyList<TItem> items,
            TItem? prevItem,
            Func<TItem, TComponent> componentGetter,
            Func<TItem, RectTransform>? cursorGetter,
            string doTweenId = "CursorMove",
            float duration = 0.3f,
            string cursorGameObjectName = "Cursor")
            where TItem : Component
            where TComponent : Component
            => UpdateCursorInner(items, prevItem, componentGetter, cursorGetter, doTweenId, duration, cursorGameObjectName);

        static TItem? UpdateCursorInner<TItem, TComponent>(IReadOnlyList<TItem> items, TItem? prevItem, Func<TItem, TComponent>? componentGetter, Func<TItem, RectTransform>? cursorGetter, string doTweenId, float duration, string cursorGameObjectName)
            where TItem : Component
            where TComponent : Component
        {
            var selected = componentGetter != null ?
                EventSystemUtil.GetSelectedItem(items, componentGetter) :
                EventSystemUtil.GetSelectedItem(items);
            if (selected != null)
            {
                UpdateCursorTo(items, prevItem, selected, cursorGetter, doTweenId, duration, cursorGameObjectName);
            }
            return selected;
        }

        /// <summary>特定の構造を前提とした一連のメニュー項目に対して、Cursor表示オブジェクトの表示/非表示を切り替えるための関数。</summary>
        /// <remarks>
        /// 1. Cursor表示オブジェクトは、各メニュー項目の子オブジェクトとして存在し、同一の操作でアクセスできる必要があります。<br/>
        /// 2. 現在選択中のメニュー項目のCursor表示オブジェクトのみがActiveで、他のメニュー項目のCursor表示オブジェクトは非Activeとします。<br/>
        /// </remarks>
        public static void UpdateCursorTo<TItem>(
            IReadOnlyList<TItem> items,
            TItem? prevItem,
            TItem? nextItem,
            Func<TItem, RectTransform>? cursorGetter = null,
            string doTweenId = "CursorMove",
            float duration = 0.3f,
            string cursorGameObjectName = "Cursor")
            where TItem : Component
        {
            // Previous value
            RectTransform? prevCursor = null;
            Vector3? prevWorldPos = null;
            if (prevItem != null)
            {
                prevCursor = cursorGetter != null ?
                    cursorGetter(prevItem) :
                    prevItem.transform.GetComponentInChildrenByName<RectTransform>(cursorGameObjectName, true);
                if (prevCursor != null) prevWorldPos = prevCursor.position;
            }

            // Update active
            RectTransform? nextCursor = null;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var isNext = item == nextItem;
                var cursor = cursorGetter != null ?
                    cursorGetter(item) :
                    item.transform.GetComponentInChildrenByName<RectTransform>(cursorGameObjectName, true);
                if (cursor == null) continue;

                cursor.gameObject.SetActive(isNext);
                if (isNext) nextCursor = cursor;
            }
            if (nextCursor == null)
            {
                DevLog.LogWarning("Next cursor not found");
                return;
            }

            // Begin tween
            if (prevWorldPos == null) return;
            if (prevCursor == nextCursor) return;
            var canvas = nextCursor.GetComponentInParent<Canvas>();
            var camera = canvas != null ? canvas.worldCamera : null;
            var fromPos = RectTransformUtil.ToOtherAnchoredPosition(camera, prevWorldPos.Value, nextCursor);
            DOTween.Complete(doTweenId);
            nextCursor.DOAnchorPos(fromPos, duration)
                .From()
                .SetId(doTweenId)
                .SetLink(nextCursor.gameObject, LinkBehaviour.CompleteOnDisable)
                .SetUpdate(UpdateType.Late);
        }
    }
}
