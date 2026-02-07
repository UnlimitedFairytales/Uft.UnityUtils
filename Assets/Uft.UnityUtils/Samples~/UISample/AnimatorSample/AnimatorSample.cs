using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class AnimatorSample : MonoBehaviour
    {
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する
        const string SHOWING = "Showing";
        const string SHOWN = "Shown";
        const string HIDIING = "Hiding";
        const string HIDDEN = "Hidden";
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する

        [SerializeField] Button _button;
        [SerializeField] Animator _animator;

        bool _isClosed = true;

        void Start()
        {
            this._button.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                var nextStateName = this._isClosed ? SHOWING : HIDIING;
                this._isClosed = !this._isClosed;
                this._animator.SetTrigger(nextStateName);
                await this._animator.DelayForAnimation(nextStateName, true);
                DevLog.Log($"{nextStateName} complete");
            }));
        }
    }
}
