using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Uft.UnityUtils.UI
{
    public class MessageBoxHelper<TResult>
    {
        readonly GameObject _gameObject;
        readonly Func<TResult> _getResult;
        readonly Animator _animator;
        readonly string _showingTriggerAndStateName;
        readonly string _closingTriggerAndStateName;
        readonly bool _isCompletionOnUnexpectedNextState;
        readonly int _layerIndex;

        bool _isClosable = false;

        public MessageBoxHelper(GameObject gameObject, Func<TResult> getResult, Animator animator, string showingTriggerAndStateName = "Showing", string closingTriggerAndStateName = "Closing", bool isCompletionOnUnexpectedNextState = true, int layerIndex = 0)
        {
            if (gameObject == null) new ArgumentNullException(nameof(gameObject));
            if (getResult == null) new ArgumentNullException(nameof(getResult));

            this._gameObject = gameObject;
            this._getResult = getResult;
            this._animator = animator;
            this._showingTriggerAndStateName = showingTriggerAndStateName;
            this._closingTriggerAndStateName = closingTriggerAndStateName;
            this._isCompletionOnUnexpectedNextState = isCompletionOnUnexpectedNextState;
            this._layerIndex = layerIndex;
        }

        public virtual async UniTask<TResult> ShowAsync(CancellationToken ct)
        {
            this._gameObject.SetActive(true);
            this._isClosable = true;

            if (this._animator != null)
            {
                this._animator.SetTrigger(this._showingTriggerAndStateName);
                await this._animator.DelayForAnimation(this._showingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, default, this._layerIndex);
            }
            while (this._gameObject.activeSelf)
            {
                await UniTask.NextFrame();
                if (ct.IsCancellationRequested && this._isClosable)
                {
                    Debug.Log($"{nameof(MessageBoxHelper<TResult>)}.{nameof(ShowAsync)} canceled");
                    await this.CloseAsync();
                }
            }
            return this._getResult();
        }

        public virtual async UniTask CloseAsync()
        {
            if (!this._isClosable) return;
            this._isClosable = false;

            if (this._animator != null)
            {
                this._animator.SetTrigger(this._closingTriggerAndStateName);
                await this._animator.DelayForAnimation(this._closingTriggerAndStateName, true, this._isCompletionOnUnexpectedNextState, default, this._layerIndex);
            }
            this._gameObject.SetActive(false);
        }
    }
}
