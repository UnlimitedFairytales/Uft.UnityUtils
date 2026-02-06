using UnityEngine;

namespace Uft.UnityUtils.Samples.PlayerLoopSample
{
    [DefaultExecutionOrder(2)]
    public class PlayerLoopSample_Base2 : MonoBehaviour
    {
        // NOTE: DefaultExecutionOrderはシーン間での動作は非保証。Editor上でシーンを跨いで 0 > 1 > 2 となっていても、実機で保証はしないため注意
        private void OnEnable() => PlayerLoopUtil.JustBeforeUpdate += this.JustBeforeUpdate;
        private void OnDisable() => PlayerLoopUtil.JustBeforeUpdate -= this.JustBeforeUpdate;
        void JustBeforeUpdate() => Debug.Log($"<color=lime>{Time.frameCount}frame {nameof(PlayerLoopSample_Base2)} JustBeforeUpdate</color>");
        void Update() => Debug.Log($"<color=lime>{Time.frameCount}frame {nameof(PlayerLoopSample_Base2)} Update</color>");
        void LateUpdate() => Debug.Log($"<color=lime>{Time.frameCount}frame {nameof(PlayerLoopSample_Base2)} LateUpdate</color>");
    }
}
