using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Assets.Uft.UnityUtils.Runtime
{
    /// <summary>
    /// https://docs.unity3d.com/6000.0/Documentation/Manual/player-loop-customizing.html
    /// </summary>
    public static class PlayerLoopUtil
    {
#if !DISABLE_UNITYUTILS_JUST_BEFORE_UPDATE
        public class JustBeforeUpdateType
        {
            public static void Run()
            {
                if (JustBeforeUpdate == null) return;
                foreach (var handler in JustBeforeUpdate.GetInvocationList())
                {
                    try
                    {
                        ((Action)handler)();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
            }
        }

        public static event Action JustBeforeUpdate;
        public static bool _installed;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            JustBeforeUpdate = null;
            _installed = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Install()
        {
            if (_installed) return;

            var loop = PlayerLoop.GetDefaultPlayerLoop();
            Cysharp.Threading.Tasks.PlayerLoopHelper.Initialize(ref loop);

            var customUpdate = new PlayerLoopSystem()
            {
                updateDelegate = JustBeforeUpdateType.Run,
                type = typeof(JustBeforeUpdateType)
            };
            InsertBefore<Update.ScriptRunBehaviourUpdate>(ref loop, customUpdate);
            PlayerLoop.SetPlayerLoop(loop);
            _installed = true;
        }
#endif

        public static bool InsertBefore<TTarget>(ref PlayerLoopSystem root, PlayerLoopSystem node)
        {
            if (root.subSystemList == null) return false;

            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                ref var sub = ref root.subSystemList[i];
                var listInSub = sub.subSystemList;
                if (listInSub != null)
                {
                    for (int j = 0; j < listInSub.Length; j++)
                    {
                        if (listInSub[j].type == typeof(TTarget))
                        {
                            var newListInSub = new List<PlayerLoopSystem>(listInSub);
                            newListInSub.Insert(j, node);
                            sub.subSystemList = newListInSub.ToArray();
                            return true;
                        }
                    }
                }
                var copy = sub;
                if (InsertBefore<TTarget>(ref copy, node))
                {
                    sub = copy;
                    return true;
                }
            }
            return false;
        }
    }
}
