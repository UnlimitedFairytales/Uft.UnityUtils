#nullable enable

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class AnimatorUtil
    {
        public static async UniTask DelayForAnimation(this Animator animator, string delayStateName, bool isCompleteAfterFirstLoop = false, bool isCompleteOnUnexpectedNextState = true, int layerIndex = 0, CancellationToken cancellationToken = default)
        {
            if (!animator) return;
            if (layerIndex < 0 || animator.layerCount <= layerIndex) return;

            var delayHash = Animator.StringToHash(delayStateName);

            await UniTask.NextFrame(cancellationToken); // NOTE: SetTrigger()後にStateが変わるのは Internal animation update後。そのため1フレーム待つ
            while (true)
            {
                if (!animator) return;
                if (!animator.isActiveAndEnabled) return;
                if (!animator.gameObject.activeInHierarchy) return;

                try
                {
                    var current = animator.GetCurrentAnimatorStateInfo(layerIndex);
                    var next = animator.GetNextAnimatorStateInfo(layerIndex);
                    var isTransition = animator.IsInTransition(layerIndex);

                    var isDelayState = current.shortNameHash == delayHash || (next.shortNameHash == delayHash && isTransition);
                    var isFirstLoopFinished = current.shortNameHash == delayHash && 1.0f <= current.normalizedTime;
                    var isUnexpectedNextState = isTransition && next.shortNameHash != delayHash;
                    if (!isDelayState || (isCompleteAfterFirstLoop && isFirstLoopFinished) || (isCompleteOnUnexpectedNextState && isUnexpectedNextState))
                    {
                        return;
                    }
                }
                catch (MissingReferenceException)
                {
                    return;
                }
                await UniTask.NextFrame(cancellationToken);
            }
        }
    }
}
