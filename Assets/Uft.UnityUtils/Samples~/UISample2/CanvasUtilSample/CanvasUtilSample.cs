using Uft.UnityUtils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class CanvasUtilSample : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;
        [SerializeField] Image _image;
        [SerializeField] SpriteRenderer _spriteRenderer;

        int _count;

        void Start()
        {
            CanvasUtil.SetCameraToCanvas(this._canvas.gameObject, Camera.main, true, CameraUtil.DISTANCE_720P_FOV60_PPU100);
        }

        void Update()
        {
            // NOTE: StartでCanvasのcameraをバインドする場合、1フレーム目では正常に配置できない。インスペクタで完結しているなら1フレーム目でもOK
            this._count++;
            if (this._count == 2)
            {
                var canvas = this.GetComponentInChildren<Canvas>();
                var worldPos = this._image.rectTransform.GetWorldPosition(canvas, new Vector2(-200, -200));
                this._spriteRenderer.transform.position = worldPos;
            }
        }
    }
}
