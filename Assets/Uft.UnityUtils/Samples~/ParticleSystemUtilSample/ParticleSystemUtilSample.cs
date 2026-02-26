using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Uft.UnityUtils.Samples.ParticleSystemUtilSample
{
    public class ParticleSystemUtilSample : MonoBehaviour
    {
        [SerializeField] GameObject _tapEffect_prefab;

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var screenPos = Mouse.current.position.ReadValue();
                var distance = CameraUtil.GetPixelEqualSizeDistance(Screen.height, Camera.main.fieldOfView);
                this._tapEffect_prefab.CreateAndPlayEffectAsync(screenPos, distance, Camera.main, this.transform).Forget();
            }
        }
    }
}
