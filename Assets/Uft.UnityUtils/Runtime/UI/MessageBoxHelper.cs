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

            _gameObject = gameObject;
            _getResult = getResult;
            _animator = animator;
            _showingTriggerAndStateName = showingTriggerAndStateName;
            _closingTriggerAndStateName = closingTriggerAndStateName;
            _isCompletionOnUnexpectedNextState = isCompletionOnUnexpectedNextState;
            _layerIndex = layerIndex;
        }

        public virtual async UniTask<TResult> ShowAsync(CancellationToken ct)
        {
            _gameObject.SetActive(true);
            _isClosable = true;

            if (_animator != null)
            {
                _animator.SetTrigger(_showingTriggerAndStateName);
                await AnimatorUtil.DelayForAnimation(_animator, _showingTriggerAndStateName, true, _isCompletionOnUnexpectedNextState, default, _layerIndex);
            }
            while (_gameObject.activeSelf)
            {
                await UniTask.NextFrame();
                if (ct.IsCancellationRequested && _isClosable)
                {
                    Debug.Log($"{nameof(MessageBoxHelper<TResult>)}.{nameof(ShowAsync)} canceled");
                    await CloseAsync();
                }
            }
            return _getResult();
        }

        public virtual async UniTask CloseAsync()
        {
            if (!_isClosable) return;
            _isClosable = false;

            if (_animator != null)
            {
                _animator.SetTrigger(_closingTriggerAndStateName);
                await AnimatorUtil.DelayForAnimation(_animator, _closingTriggerAndStateName, true, _isCompletionOnUnexpectedNextState, default, _layerIndex);
            }
            _gameObject.SetActive(false);
        }
    }
}
