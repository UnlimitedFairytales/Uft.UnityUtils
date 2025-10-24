using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class AnimatorUtil
    {
        public static async UniTask DelayForAnimation(this Animator animator, string delayStateName, bool isCompleteAfterFirstLoop = false, bool isCompleteOnUnexpectedNextState = true, CancellationToken cancellationToken = default, int layerIndex = 0)
        {
            if (animator == null) return;
            await UniTask.NextFrame(cancellationToken); // NOTE: SetTrigger()後にStateが変わるのは Internal animation update後。そのため1フレーム待つ
            while (true)
            {
                var current = animator.GetCurrentAnimatorStateInfo(layerIndex);
                var next = animator.GetNextAnimatorStateInfo(layerIndex);
                var isDelayState = current.IsName(delayStateName) || next.IsName(delayStateName);
                var isFirstLoopFinished = current.IsName(delayStateName) && 1.0f <= current.normalizedTime;
                var isUnexpectedNextState = next.shortNameHash != 0 && !next.IsName(delayStateName);
                if (!isDelayState || (isCompleteAfterFirstLoop && isFirstLoopFinished) || (isCompleteOnUnexpectedNextState && isUnexpectedNextState))
                {
                    break;
                }
                await UniTask.NextFrame(cancellationToken);
            }
        }
    }
}
