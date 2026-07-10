#nullable enable

using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Uft.UnityUtils
{
    public static class UnityEventUtil
    {
        public static void Rebind(this UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }

        public static void Rebind<T>(this UnityEvent<T> unityEvent, UnityAction<T> unityAction)
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(unityAction);
        }

        public static UnityAction Rebind<TSender>(this UnityEvent unityEvent, TSender sender, Action<TSender> action)
            where TSender : Selectable
        {
            unityEvent.RemoveAllListeners();
            void unityAction()
            {
                if (sender is UnityEngine.Object obj && !obj) return;
                action(sender);
            }
            unityEvent.AddListener(unityAction);
            return unityAction;
        }

        public static UnityAction<TArg> Rebind<TSender, TArg>(
            this UnityEvent<TArg> unityEvent, TSender sender, Action<TSender, TArg> action)
            where TSender : Selectable
        {
            unityEvent.RemoveAllListeners();
            void unityAction(TArg arg)
            {
                if (sender is UnityEngine.Object obj && !obj) return;
                action(sender, arg);
            }
            unityEvent.AddListener(unityAction);
            return unityAction;
        }
    }
}
