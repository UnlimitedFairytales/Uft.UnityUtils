using UnityEngine;

namespace Uft.UnityUtils
{
    [DefaultExecutionOrder(10000)]
    public class BillBoard : MonoBehaviour
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

            if (!this.isYOnly)
            {
                this.transform.forward = this.sourceCamera.transform.forward;
                return;
            }
            var fwd = this.sourceCamera.transform.forward;
            fwd.y = 0f;
            if (fwd != Vector3.zero)
            {
                this.transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
            }
        }
    }
}
