using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class ParticleSystemUtil
    {
        public static async UniTask DelayForParticleSystem(GameObject instantiated, CancellationToken cancellationToken = default)
        {
            var effects = instantiated.GetComponentsInChildren<ParticleSystem>(true);
            var eachIsPlaying = true;
            while (eachIsPlaying)
            {
                eachIsPlaying = false;
                foreach (var effect in effects)
                {
                    if (effect.isPlaying)
                    {
                        eachIsPlaying = true;
                        break;
                    }
                }
                await UniTask.NextFrame(cancellationToken);
            }
        }

        public static async UniTask CreateAndPlayEffectAsync(GameObject effect_prefab, Vector3 screenPos, float distance, Camera camera, Transform parent, CancellationToken cancellationToken = default)
        {
            var instantiated = Object.Instantiate(effect_prefab, parent);
            if (instantiated != null)
            {
                screenPos.z = distance;
                var pos3D = camera.ScreenToWorldPoint(screenPos);
                instantiated.transform.position = pos3D;
                await DelayForParticleSystem(instantiated, cancellationToken);
                if (instantiated != null)
                {
                    Object.Destroy(instantiated);
                }
            }
        }
    }
}
