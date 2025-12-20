#nullable enable

// #define DISABLE_UNITYUTILS_JUST_BEFORE_UPDATE

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Uft.UnityUtils
{
    /// <summary>
    /// https://docs.unity3d.com/6000.0/Documentation/Manual/player-loop-customizing.html
    /// </summary>
    public static class PlayerLoopUtil
    {
        const string NAME = "[" + nameof(PlayerLoopUtil) + "]";

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

        public static event Action? JustBeforeUpdate;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Reset()
        {
            JustBeforeUpdate = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Install()
        {
            var loop = PlayerLoop.GetDefaultPlayerLoop();
            if (ContainsType(loop, typeof(JustBeforeUpdateType))) return;

            Cysharp.Threading.Tasks.PlayerLoopHelper.Initialize(ref loop);
            var customUpdate = new PlayerLoopSystem()
            {
                updateDelegate = JustBeforeUpdateType.Run,
                type = typeof(JustBeforeUpdateType)
            };
            if (InsertBefore<Update.ScriptRunBehaviourUpdate>(ref loop, customUpdate))
            {
                PlayerLoop.SetPlayerLoop(loop);
                DevLog.Log($"{NAME} enable JustBeforeUpdate");
            }
            else
            {
                DevLog.LogWarning($"{NAME} Failed to setup JustBeforeUpdate");
            }
        }
#endif

        public static bool InsertBefore<TTarget>(ref PlayerLoopSystem root, PlayerLoopSystem node)
        {
            return InsertBeforeInner<TTarget>(ref root, root.subSystemList, node);
        }

        static bool InsertBeforeInner<TTarget>(ref PlayerLoopSystem parent, PlayerLoopSystem[]? children, PlayerLoopSystem node)
        {
            if (children == null) return false;

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].type == typeof(TTarget))
                {
                    var newChildren = new List<PlayerLoopSystem>(children);
                    newChildren.Insert(i, node);
                    parent.subSystemList = newChildren.ToArray();
                    return true;
                }

                var grandChildren = children[i].subSystemList;
                if (InsertBeforeInner<TTarget>(ref children[i], grandChildren, node))
                {
                    return true;
                }
            }
            return false;
        }

        static bool ContainsType(PlayerLoopSystem root, Type type)
        {
            if (root.type == type) return true;

            var list = root.subSystemList;
            if (list == null) return false;

            for (int i = 0; i < list.Length; i++)
            {
                if (ContainsType(list[i], type)) return true;
            }
            return false;
        }
    }
}
