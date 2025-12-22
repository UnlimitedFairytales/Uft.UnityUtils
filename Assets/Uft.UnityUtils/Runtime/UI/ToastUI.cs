#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class ToastUI : MonoBehaviour
    {
        [SerializeField] Animator? _animator;
        [SerializeField] TMP_Text? _lblHeader;
        [SerializeField] TMP_Text? _lblContent;
        [SerializeField] Button? _tapArea;

        MessageBoxHelper<int>? _messageBoxHelper;

        void Reset()
        {
            // NOTE: ResetはLinq許容
            if (this._animator == null) this._animator = this.GetComponentInChildren<Animator>();
            if (this._lblHeader == null)
            {
                this._lblHeader = this.GetComponentsInChildren<TMP_Text>()
                    .Where(tmp => tmp.gameObject.name.Contains("Header", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
            if (this._lblContent == null)
            {
                this._lblContent = this.GetComponentsInChildren<TMP_Text>()
                    .Where(tmp =>
                        tmp.gameObject.name.Contains("Content", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("Text", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("Body", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
            if (this._tapArea == null)
            {
                this._tapArea = this.GetComponentInChildren<Button>();
            }
        }

        void Awake()
        {
            if (this._tapArea == null) throw new UnassignedReferenceException($"{nameof(this._tapArea)}");

            this._messageBoxHelper = new MessageBoxHelper<int>(this.gameObject, (status) => new OperationResult<int>(status, 0), this._animator);
            this._tapArea.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await this._messageBoxHelper.CloseAsync(default);
            }));
        }

        // Unity event functions & event handlers / pure code

        public async UniTask ShowAsync(string headerText, string contentText, int timeout_sec = 0)
        {
            this.gameObject.SetActive(true);
            if (this._messageBoxHelper == null) throw new OperationCanceledException("Before Awake()");

            if (this._lblHeader != null) this._lblHeader.SetText(headerText);
            if (this._lblContent != null) this._lblContent.SetText(contentText);

            var cts = new CancellationTokenSource();
            IDisposable? timeoutTimer = null;
            try
            {
                if (0 < timeout_sec)
                {
                    timeoutTimer = cts.CancelAfterSlim(timeout_sec * 1000);
                }
                await this._messageBoxHelper.ShowAsync(cts.Token);
            }
            finally
            {
                timeoutTimer?.Dispose();
                cts.Dispose();
            }
        }
    }
}
