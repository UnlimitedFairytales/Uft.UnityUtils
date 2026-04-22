#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils
{
    public readonly struct OperationResult<T>
    {
        public readonly OperationResultStatus status;
        public readonly T? value;

        public OperationResult(OperationResultStatus status, T? value)
        {
            this.status = status;
            this.value = value;
        }
    }

    public enum OperationResultStatus
    {
        Canceled,
        Accepted,
        RejectedDueToDuplicate,
        RejectedDueToCooldown,
    }

    public enum WindowState
    {
        Hidden,
        Showing,
        Shown,
        Hiding
    }

    /// <summary>
    /// ウィンドウのShow/Hide制御を担うヘルパ<br/>
    /// キャンセルやオブジェクト破棄が発生した場合、 OperationCanceledException を呼び出し元に伝播させず、<see cref="OperationResultStatus.Canceled"/> な結果を返す。
    /// </summary>
    public class WindowHelper<TResult>
    {
        readonly MonoBehaviour _owner;
        readonly CancellationToken _destroyCt;
        readonly Func<OperationResultStatus, OperationResult<TResult>> _getResult;
        readonly Animator? _animator;
        readonly string _showingTriggerAndStateName;
        readonly string _hidingTriggerAndStateName;
        readonly bool _isCompletionOnUnexpectedNextState;
        readonly int _layerIndex;

        WindowState _state = WindowState.Hidden; public WindowState State => this._state;

        public WindowHelper(MonoBehaviour owner, Func<OperationResultStatus, OperationResult<TResult>> getResult, Animator? animator,
            string showingTriggerAndStateName = "Showing",
            string hidingTriggerAndStateName = "Hiding",
            bool isCompletionOnUnexpectedNextState = true, int layerIndex = 0)
        {
            this._owner = owner;
            this._destroyCt = owner.destroyCancellationToken;
            this._getResult = getResult;
            this._animator = animator;
            this._showingTriggerAndStateName = showingTriggerAndStateName;
            this._hidingTriggerAndStateName = hidingTriggerAndStateName;
            this._isCompletionOnUnexpectedNextState = isCompletionOnUnexpectedNextState;
            this._layerIndex = layerIndex;
        }

        public virtual async UniTask<OperationResult<TResult>> ShowDialogAsync(CancellationToken ct)
        {
            if (this._state != WindowState.Hidden) return this._getResult(OperationResultStatus.RejectedDueToDuplicate);
            if (!this._owner) return this._getResult(OperationResultStatus.Canceled);

            // NOTE: 呼び出し側のキャンセル（タイムアウト等）とオブジェクト破棄の両方に反応させるためリンク
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, this._destroyCt);

            try
            {
                // 1
                this._state = WindowState.Showing;
                this._owner.gameObject.SetActive(true);

                // 2
                if (this._animator)
                {
                    this._animator.SetTrigger(this._showingTriggerAndStateName);
                    await this._animator.DelayForAnimation(this._showingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, this._layerIndex, linkedCts.Token);
                }

                // 3
                if (!this._owner || !this._owner.gameObject.activeInHierarchy) throw new OperationCanceledException();
                if (this._state == WindowState.Showing) this._state = WindowState.Shown;

                while (this._owner && this._owner.gameObject.activeInHierarchy)
                {
                    await UniTask.NextFrame(linkedCts.Token);
                }

                // 4
                return this._getResult(this._owner ? OperationResultStatus.Accepted : OperationResultStatus.Canceled);
            }
            catch (OperationCanceledException)
            {
                // NOTE: ctは既にキャンセル済みな場合あり。そうでない場合、先祖が非アクティブ。_destroyCtを渡す
                await this.HideAsync(this._destroyCt);
                return this._getResult(OperationResultStatus.Canceled);
            }
            finally
            {
                if (this._state == WindowState.Showing || this._state == WindowState.Shown)
                {
                    this._state = this._owner && this._owner.gameObject.activeInHierarchy ? WindowState.Shown : WindowState.Hidden;
                }
            }
        }

        public virtual async UniTask HideAsync(CancellationToken ct)
        {
            // NOTE: 呼び出し側のキャンセル（タイムアウト等）とオブジェクト破棄の両方に反応させるためリンク
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, this._destroyCt);

            try
            {
                if (this._state == WindowState.Hiding)
                {
                    // NOTE: finallyが必ずHiddenにするため、WaitUntilで完了を待つ
                    await UniTask.WaitUntil(() => this._state == WindowState.Hidden, cancellationToken: linkedCts.Token);
                    return;
                }
                if (this._state == WindowState.Hidden) return;

                this._state = WindowState.Hiding;
                if (this._animator)
                {
                    this._animator.SetTrigger(this._hidingTriggerAndStateName);
                    await this._animator.DelayForAnimation(this._hidingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, this._layerIndex, linkedCts.Token);
                }
            }
            catch (OperationCanceledException) { } // NOTE: OCEは握りつぶす
            finally
            {
                if (this._state != WindowState.Hidden)
                {
                    if (this._owner) this._owner.gameObject.SetActive(false);
                    this._state = WindowState.Hidden;
                }
            }
        }
    }
}
