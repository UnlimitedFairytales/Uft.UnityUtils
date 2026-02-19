#nullable enable

using UnityEngine.Events;

namespace Assets.Uft.UnityUtils.Runtime
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
    }
}
