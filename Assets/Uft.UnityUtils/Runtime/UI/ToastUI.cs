#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class ToastUI : MonoBehaviour
    {
        public const int RESULT_OK = 1;
        public const int RESULT_CANCEL = 2;

        [SerializeField] protected Animator? _animator;
        [SerializeField] protected TMP_Text? _lblHeader;
        [SerializeField] protected TMP_Text? _lblContent;
        [SerializeField] protected Button? _btnOk;

        protected MessageBoxHelper<int>? _messageBoxHelper;

        protected int _result;

        protected virtual void Reset()
        {
            // NOTE: ResetはLinq許容
            if (this._animator == null) this._animator = this.GetComponentInChildren<Animator>();
            if (this._lblHeader == null)
            {
                this._lblHeader = this.GetComponentsInChildren<TMP_Text>()
                    .Where(tmp =>
                        tmp.gameObject.name.Contains("Title", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("Header", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
            if (this._lblContent == null)
            {
                this._lblContent = this.GetComponentsInChildren<TMP_Text>()
                    .Where(tmp =>
                        tmp.gameObject.name.Contains("Content", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("Body", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
            if (this._btnOk == null)
            {
                this._btnOk = this.GetComponentInChildren<Button>();
            }
        }

        [MemberNotNull(nameof(this._btnOk))]
        [MemberNotNull(nameof(this._messageBoxHelper))]
        protected virtual void Awake()
        {
            if (this._btnOk == null) throw new UnassignedReferenceException(nameof(this._btnOk));

            this._messageBoxHelper = new MessageBoxHelper<int>(this.gameObject, (status) => new OperationResult<int>(status, this._result), this._animator);
            this._btnOk.onClick.AddListener(UniTask.UnityAction(async () => await this.SubmitOk()));
        }

        // Unity event functions & event handlers / pure code

        /// <summary>引数がnullの場合は文字列を適用しません。</summary>
        public async UniTask<OperationResult<int>> ShowAsync(string? headerText = null, string? contentText = null, int timeout_sec = 0)
        {
            this.gameObject.SetActive(true);
            if (this._messageBoxHelper == null) throw new OperationCanceledException("Before Awake()");

            if (this._lblHeader != null && headerText != null) this._lblHeader.SetText(headerText);
            if (this._lblContent != null && contentText != null) this._lblContent.SetText(contentText);

            var cts = new CancellationTokenSource();
            IDisposable? timeoutTimer = null;
            try
            {
                if (0 < timeout_sec)
                {
                    timeoutTimer = cts.CancelAfterSlim(timeout_sec * 1000);
                }
                return await this._messageBoxHelper.ShowAsync(cts.Token);
            }
            finally
            {
                timeoutTimer?.Dispose();
                cts.Dispose();
            }
        }

        public virtual async UniTask SubmitOk()
        {
            if (this._messageBoxHelper == null) throw new OperationCanceledException("Before Awake()");

            this._result = RESULT_OK;
            await this._messageBoxHelper.CloseAsync(default);
        }
    }
}
