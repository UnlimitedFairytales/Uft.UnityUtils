#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uft.UnityUtils.UI
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

    public enum MessageBoxState
    {
        Closed,
        Showing,
        Shown,
        Closing
    }

    public class MessageBoxHelper<TResult>
    {
        readonly GameObject _gameObject;
        readonly Func<OperationResultStatus, OperationResult<TResult>> _getResult;
        readonly Animator? _animator;
        readonly string _showingTriggerAndStateName;
        readonly string _closingTriggerAndStateName;
        readonly bool _isCompletionOnUnexpectedNextState;
        readonly int _layerIndex;

        MessageBoxState _state = MessageBoxState.Closed; public MessageBoxState State => this._state;
        UniTask? _closingTask = null;

        public MessageBoxHelper(GameObject gameObject, Func<OperationResultStatus, OperationResult<TResult>> getResult, Animator? animator,
            string showingTriggerAndStateName = "Showing",
            string closingTriggerAndStateName = "Closing",
            bool isCompletionOnUnexpectedNextState = true, int layerIndex = 0)
        {
            this._gameObject = gameObject;
            this._getResult = getResult;
            this._animator = animator;
            this._showingTriggerAndStateName = showingTriggerAndStateName;
            this._closingTriggerAndStateName = closingTriggerAndStateName;
            this._isCompletionOnUnexpectedNextState = isCompletionOnUnexpectedNextState;
            this._layerIndex = layerIndex;
        }

        public virtual async UniTask<OperationResult<TResult>> ShowAsync(CancellationToken ct)
        {
            if (this._state != MessageBoxState.Closed) return this._getResult(OperationResultStatus.RejectedDueToDuplicate);
            if (!this._gameObject) return this._getResult(OperationResultStatus.Canceled);

            try
            {
                // 1
                this._state = MessageBoxState.Showing;
                this._gameObject.SetActive(true);

                // 2
                if (this._animator != null)
                {
                    this._animator.SetTrigger(this._showingTriggerAndStateName);
                    await this._animator.DelayForAnimation(this._showingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, this._layerIndex, ct);
                }

                // 3
                if (!this._gameObject || !this._gameObject.activeInHierarchy) return this._getResult(OperationResultStatus.Canceled);
                this._state = MessageBoxState.Shown;

                while (this._gameObject && this._gameObject.activeInHierarchy)
                {
                    await UniTask.NextFrame(ct);
                }

                // 4
                return this._getResult(this._gameObject ? OperationResultStatus.Accepted : OperationResultStatus.Canceled);
            }
            catch (OperationCanceledException)
            {
                await this.CloseAsync(CancellationToken.None);
                return this._getResult(OperationResultStatus.Canceled);
            }
            finally
            {
                if (this._state == MessageBoxState.Showing || this._state == MessageBoxState.Shown)
                {
                    this._state = this._gameObject && this._gameObject.activeInHierarchy ? MessageBoxState.Shown : MessageBoxState.Closed;
                }
            }
        }

        public virtual UniTask CloseAsync(CancellationToken ct)
        {
            if (this._state == MessageBoxState.Closing)
            {
                Assert.IsTrue(this._closingTask != null);
                return UniTask.CompletedTask; // NOTE: UniTaskは多重awaitを許可しないため、完了タスクを返す
            }
            if (this._state == MessageBoxState.Closed) return UniTask.CompletedTask;

            this._state = MessageBoxState.Closing;
            var uniTask = this.CloseAsyncInner(ct);
            this._closingTask = uniTask;
            return uniTask;
        }

        protected virtual async UniTask CloseAsyncInner(CancellationToken ct)
        {
            try
            {
                Assert.IsTrue(this._state == MessageBoxState.Closing);
                if (!this._gameObject) return;
                if (this._animator != null)
                {
                    this._animator.SetTrigger(this._closingTriggerAndStateName);
                    await this._animator.DelayForAnimation(this._closingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, this._layerIndex, ct);
                }
            }
            finally
            {
                if (this._gameObject) this._gameObject.SetActive(false);
                this._closingTask = null;
                this._state = MessageBoxState.Closed;
            }
        }
    }
}
