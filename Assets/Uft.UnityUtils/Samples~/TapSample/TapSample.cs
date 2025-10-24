using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Uft.UnityUtils.Samples.TapSample
{
    public class TapSample : MonoBehaviour
    {
        [SerializeField] GameObject _tapEffect_prefab;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var screenPos = Input.mousePosition;
                var distance = CameraUtil.GetPixelEqualSizeDistance(Screen.height, Camera.main.fieldOfView) / 10.0f;
                _tapEffect_prefab.CreateAndPlayEffectAsync(screenPos, distance, Camera.main, this.transform).Forget();
            }
        }
    }
}
