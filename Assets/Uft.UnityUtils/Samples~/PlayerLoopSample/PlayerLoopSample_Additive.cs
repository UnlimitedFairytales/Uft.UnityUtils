using UnityEngine;

namespace Uft.UnityUtils.Samples.PlayerLoopSample
{
    [DefaultExecutionOrder(1)]
    public class PlayerLoopSample_Additive : MonoBehaviour
    {
        private void OnEnable() => PlayerLoopUtil.JustBeforeUpdate += this.JustBeforeUpdate;
        private void OnDisable() => PlayerLoopUtil.JustBeforeUpdate -= this.JustBeforeUpdate;
        void JustBeforeUpdate() => Debug.Log($"<color=aqua>{Time.frameCount}frame {nameof(PlayerLoopSample_Additive)} JustBeforeUpdate</color>");
        void Update() => Debug.Log($"<color=aqua>{Time.frameCount}frame {nameof(PlayerLoopSample_Additive)} Update</color>");
        void LateUpdate() => Debug.Log($"<color=aqua>{Time.frameCount}frame {nameof(PlayerLoopSample_Additive)} LateUpdate</color>");
    }
}
