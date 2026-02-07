#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uft.UnityUtils
{
    public readonly struct OperationResult<T>
    {
        public readonly OperationResultStatus status;
        public readonly T? result;

        public OperationResult(OperationResultStatus status, T? result)
        {
            this.status = status;
            this.result = result;
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

    public class WindowHelper<TResult>
    {
        readonly GameObject _gameObject;
        readonly Func<OperationResultStatus, OperationResult<TResult>> _getResult;
        readonly Animator? _animator;
        readonly string _showingTriggerAndStateName;
        readonly string _hidingTriggerAndStateName;
        readonly bool _isCompletionOnUnexpectedNextState;
        readonly int _layerIndex;

        WindowState _state = WindowState.Hidden; public WindowState State => this._state;
        UniTask? _hidingTask = null;

        public WindowHelper(GameObject gameObject, Func<OperationResultStatus, OperationResult<TResult>> getResult, Animator? animator,
            string showingTriggerAndStateName = "Showing",
            string hidingTriggerAndStateName = "Hiding",
            bool isCompletionOnUnexpectedNextState = true, int layerIndex = 0)
        {
            this._gameObject = gameObject;
            this._getResult = getResult;
            this._animator = animator;
            this._showingTriggerAndStateName = showingTriggerAndStateName;
            this._hidingTriggerAndStateName = hidingTriggerAndStateName;
            this._isCompletionOnUnexpectedNextState = isCompletionOnUnexpectedNextState;
            this._layerIndex = layerIndex;
        }

        public virtual async UniTask<OperationResult<TResult>> ShowAsync(CancellationToken ct)
        {
            if (this._state != WindowState.Hidden) return this._getResult(OperationResultStatus.RejectedDueToDuplicate);
            if (!this._gameObject) return this._getResult(OperationResultStatus.Canceled);

            try
            {
                // 1
                this._state = WindowState.Showing;
                this._gameObject.SetActive(true);

                // 2
                if (this._animator != null)
                {
                    this._animator.SetTrigger(this._showingTriggerAndStateName);
                    await this._animator.DelayForAnimation(this._showingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, this._layerIndex, ct);
                }

                // 3
                if (!this._gameObject || !this._gameObject.activeInHierarchy) return this._getResult(OperationResultStatus.Canceled);
                this._state = WindowState.Shown;

                while (this._gameObject && this._gameObject.activeInHierarchy)
                {
                    await UniTask.NextFrame(ct);
                }

                // 4
                return this._getResult(this._gameObject ? OperationResultStatus.Accepted : OperationResultStatus.Canceled);
            }
            catch (OperationCanceledException)
            {
                await this.HideAsync(CancellationToken.None);
                return this._getResult(OperationResultStatus.Canceled);
            }
            finally
            {
                if (this._state == WindowState.Showing || this._state == WindowState.Shown)
                {
                    this._state = this._gameObject && this._gameObject.activeInHierarchy ? WindowState.Shown : WindowState.Hidden;
                }
            }
        }

        public virtual UniTask HideAsync(CancellationToken ct)
        {
            if (this._state == WindowState.Hiding)
            {
                Assert.IsTrue(this._hidingTask != null);
                return UniTask.CompletedTask; // NOTE: UniTaskは多重awaitを許可しないため、完了タスクを返す
            }
            if (this._state == WindowState.Hidden) return UniTask.CompletedTask;

            this._state = WindowState.Hiding;
            var uniTask = this.HideAsyncInner(ct);
            this._hidingTask = uniTask;
            return uniTask;
        }

        protected virtual async UniTask HideAsyncInner(CancellationToken ct)
        {
            try
            {
                Assert.IsTrue(this._state == WindowState.Hiding);
                if (!this._gameObject) return;
                if (this._animator != null)
                {
                    this._animator.SetTrigger(this._hidingTriggerAndStateName);
                    await this._animator.DelayForAnimation(this._hidingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, this._layerIndex, ct);
                }
            }
            finally
            {
                if (this._gameObject) this._gameObject.SetActive(false);
                this._hidingTask = null;
                this._state = WindowState.Hidden;
            }
        }
    }
}
