using Uft.UnityUtils.UI;
using UnityEngine;

namespace Uft.UnityUtils.Samples._TransformSample
{
    public class Cube3x3x3View : MonoBehaviour
    {
        void Start()
        {
            this.gameObject.SetCameraToCanvas(Camera.main, true, CameraUtil.DISTANCE1080P_FOV60_PPU100);
        }
    }
}
