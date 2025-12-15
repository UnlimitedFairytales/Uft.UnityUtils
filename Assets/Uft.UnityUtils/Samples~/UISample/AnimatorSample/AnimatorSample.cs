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
        const string CLOSING = "Closing";
        const string CLOSED = "Closed";
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する

        [SerializeField] Button _button;
        [SerializeField] Animator _animator;

        bool _isClosed = true;

        void Start()
        {
            this._button.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                var nextStateName = this._isClosed ? SHOWING : CLOSING;
                this._isClosed = !this._isClosed;
                this._animator.SetTrigger(nextStateName);
                await this._animator.DelayForAnimation(nextStateName, true);
                Debug.Log($"{nextStateName} complete");
            }));
        }
    }
}
