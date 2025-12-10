using UnityEngine;

namespace Uft.UnityUtils.Samples.PlayerLoopSample
{
    [DefaultExecutionOrder(0)]
    public class PlayerLoopSample_Base : MonoBehaviour
    {
        private void OnEnable() => PlayerLoopUtil.JustBeforeUpdate += this.JustBeforeUpdate;
        private void OnDisable() => PlayerLoopUtil.JustBeforeUpdate -= this.JustBeforeUpdate;
        void JustBeforeUpdate() => Debug.Log($"<color=lime>{Time.frameCount}frame {nameof(PlayerLoopSample_Base)} JustBeforeUpdate</color>");
        void Update() => Debug.Log($"<color=lime>{Time.frameCount}frame {nameof(PlayerLoopSample_Base)} Update</color>");
        void LateUpdate() => Debug.Log($"<color=lime>{Time.frameCount}frame {nameof(PlayerLoopSample_Base)} LateUpdate</color>");
    }
}
