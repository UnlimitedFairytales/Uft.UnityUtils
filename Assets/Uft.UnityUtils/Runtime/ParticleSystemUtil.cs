#nullable enable

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class ParticleSystemUtil
    {
        /// <summary>subEmittersは1段目のみ対応で、再帰的にチェックしていません。また、loop=trueな内容はDelay判定から無視されます。</summary>
        public static async UniTask DelayForParticleSystem(this GameObject instantiated, CancellationToken cancellationToken = default)
        {
            var effects = instantiated.GetComponentsInChildren<ParticleSystem>(true);
            var isAnyPlaying = true;
            while (isAnyPlaying)
            {
                isAnyPlaying = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    var effect = effects[i];
                    if (!effect) continue;
                    if (effect.gameObject.activeInHierarchy && effect.isPlaying && !effect.main.loop)
                    {
                        isAnyPlaying = true;
                        break;
                    }
                    var subs = effect.subEmitters;
                    for (int j = 0; j < subs.subEmittersCount; j++)
                    {
                        var sub = subs.GetSubEmitterSystem(j);
                        if (!sub) continue;
                        if (sub.gameObject.activeInHierarchy && sub.isPlaying && !sub.main.loop)
                        {
                            isAnyPlaying = true;
                            break;
                        }
                    }
                    if (isAnyPlaying) break;
                }
                await UniTask.NextFrame(cancellationToken);
            }
        }

        /// <summary>subEmittersが生成元の子にならない場合、Destroy対象にならないため注意してください。生成後のawaitはDelayForParticleSystemに従います。</summary>
        public static async UniTask CreateAndPlayEffectAsync(this GameObject effectPrefab, Vector3 screenPos, float distance, Camera camera, Transform parent, CancellationToken cancellationToken = default)
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
