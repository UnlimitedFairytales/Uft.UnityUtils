using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class ToastUI : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] TMP_Text _lblHeader;
        [SerializeField] TMP_Text _lblContent;
        [SerializeField] Button _tapArea;

        MessageBoxHelper<int> _messageBoxHelper;

        void Awake()
        {
            _messageBoxHelper = new MessageBoxHelper<int>(gameObject, () => 0, _animator);
            _tapArea.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await _messageBoxHelper.CloseAsync();
            }));
        }

        // Unity event functions & event handlers / pure code

        public async UniTask ShowAsync(string headerText, string contentText, int timeout_sec = 0)
        {
            if (_lblHeader != null) _lblHeader.SetText(headerText);
            if (_lblContent != null) _lblContent.SetText(contentText);
            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(timeout_sec * 1000);
            await _messageBoxHelper.ShowAsync(cts.Token);
        }
    }
}
