using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class AnimatorUtil
    {
        public static async UniTask DelayForAnimation(Animator animator, string delayStateName, bool isCompletionOnUnexpectedNextState = true, CancellationToken cancellationToken = default, int layerIndex = 0)
        {
            if (animator == null) return;
            await UniTask.NextFrame(cancellationToken); // NOTE: SetTrigger()後にStateが変わるのは Internal animation update後。そのため1フレーム待つ
            while (true)
            {
                var current = animator.GetCurrentAnimatorStateInfo(layerIndex);
                var next = animator.GetNextAnimatorStateInfo(layerIndex);
                var isDelayState = current.IsName(delayStateName) || next.IsName(delayStateName);
                var isUnexpectedNextState = next.shortNameHash != 0 && !next.IsName(delayStateName);
                if (!isDelayState || (isCompletionOnUnexpectedNextState && isUnexpectedNextState))
                {
                    break;
                }
                await UniTask.NextFrame(cancellationToken);
            }
        }
    }
}
