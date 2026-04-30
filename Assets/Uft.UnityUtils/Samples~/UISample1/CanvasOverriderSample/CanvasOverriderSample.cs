using Uft.UnityUtils.UI;
using UnityEngine;

namespace Uft.UnityUtils.Samples.UISample1
{
    public class CanvasOverriderSample : MonoBehaviour
    {
        [SerializeField] GameObject _canvasOverriderSamplePart;

        void Start()
        {
            CanvasUtil.SetCameraToCanvas(this._canvasOverriderSamplePart, Camera.main);
        }
    }
}
