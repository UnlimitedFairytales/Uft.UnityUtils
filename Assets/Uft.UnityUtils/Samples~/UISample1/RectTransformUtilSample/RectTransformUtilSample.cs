using Uft.UnityUtils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample1
{
    public class RectTransformUtilSample : MonoBehaviour
    {
        [SerializeField] GameObject[] _cubes;
        [SerializeField] Image[] _images;

        void Start()
        {
            this.gameObject.SetCameraToCanvas(Camera.main, true, CameraUtil.DISTANCE1080P_FOV60_PPU100);

            var canvas = this._images[0].canvas;
            for (int i = 0; i < this._cubes.Length; i++)
            {
                var worldTransform = this._cubes[i].transform;
                var parent = this._images[i].rectTransform;
                RectTransformUtil.WorldToLocalPointInRect(Camera.main, worldTransform.position, canvas, parent, out Vector2 resultPoint);
                this._images[i].rectTransform.anchoredPosition = resultPoint;
            }
        }
    }
}
