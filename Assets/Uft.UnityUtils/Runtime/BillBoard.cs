using UnityEngine;

namespace Uft.UnityUtils.Samples.UISample
{
    [DefaultExecutionOrder(10000)]
    public class RectTransformUtilSample : MonoBehaviour
    {
        public bool isYOnly = false;
        public Camera sourceCamera;

        protected virtual void LateUpdate()
        {
            if (this.sourceCamera == null)
            {
                this.sourceCamera = Camera.main;
            }
            if (this.sourceCamera == null) return;

            this.transform.forward = this.sourceCamera.transform.forward;
            if (this.isYOnly)
            {
                this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
            }
        }
    }
}
