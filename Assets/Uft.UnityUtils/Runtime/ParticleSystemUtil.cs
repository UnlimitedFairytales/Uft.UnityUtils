#nullable enable

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class ParticleSystemUtil
    {
        /// <summary>subEmittersは1段目のみ対応で、再帰的にチェックしていません。また、loop=trueな内容はDelay判定から無視されます。</summary>
        public static async UniTask DelayForParticleSystem(this GameObject instantiated, CancellationToken cancellationToken)
        {
            bool AnyPlaying(ParticleSystem[] effects)
            {
                foreach (var effect in effects)
                {
                    if (!effect) continue;
                    if (effect.gameObject.activeInHierarchy && effect.isPlaying && !effect.main.loop) return true;
                    var subs = effect.subEmitters;
                    for (int j = 0; j < subs.subEmittersCount; j++)
                    {
                        var sub = subs.GetSubEmitterSystem(j);
                        if (sub && sub.gameObject.activeInHierarchy && sub.isPlaying && !sub.main.loop) return true;
                    }
                }
                return false;
            }

            var effects = instantiated.GetComponentsInChildren<ParticleSystem>(true);
            while (AnyPlaying(effects))
            {
                await UniTask.NextFrame(cancellationToken);
            }
        }

        /// <summary>subEmittersが生成元の子にならない場合、Destroy対象にならないため注意してください。生成後のawaitはDelayForParticleSystemに従います。</summary>
        public static async UniTask CreateAndPlayEffectAsync(this GameObject effectPrefab, Vector3 screenPos, float distance, Camera camera, Transform parent, CancellationToken cancellationToken)
        {
            var instantiated = Object.Instantiate(effectPrefab, parent);
            if (instantiated)
            {
                try
                {
                    screenPos.z = distance;
                    var pos3D = camera.ScreenToWorldPoint(screenPos);
                    instantiated.transform.position = pos3D;
                    await DelayForParticleSystem(instantiated, cancellationToken);
                }
                finally
                {
                    if (instantiated)
                    {
                        Object.Destroy(instantiated);
                    }
                }
            }
        }
    }
}
