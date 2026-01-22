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
    public class ConfirmUI : ToastUI
    {
        [SerializeField] Button? _btnCancel; public Button? BtnCancel => this._btnCancel;

        protected override void Reset()
        {
            // NOTE: _btnOk を 探すルールが変わっているため、base.Reset() ではなくベタ書きし直し

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
                this._btnOk = this.GetComponentsInChildren<Button>()
                    .Where(tmp =>
                        tmp.gameObject.name.Contains("OK", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("Confirm", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("Yes", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
            if (this._btnCancel == null)
            {
                this._btnCancel = this.GetComponentsInChildren<Button>()
                    .Where(tmp =>
                        tmp.gameObject.name.Contains("Cancel", StringComparison.OrdinalIgnoreCase) ||
                        tmp.gameObject.name.Contains("No", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (this._btnCancel == null) throw new UnassignedReferenceException(nameof(this._btnCancel));
            this._btnCancel.onClick.AddListener(UniTask.UnityAction(async () => await this.SubmitCancel()));
        }

        public async UniTask<OperationResult<int>> ShowAsync(string? headerText = null, string? contentText = null, int timeout_sec = 0, int initialSelection = RESULT_CANCEL)
        {
            this.gameObject.SetActive(true);
            if (this._messageBoxHelper == null) throw new OperationCanceledException("Before Awake()");
            if (this._btnOk == null) throw new UnassignedReferenceException(nameof(this._btnOk));
            if (this._btnCancel == null) throw new UnassignedReferenceException(nameof(this._btnCancel));

            if (this._lblHeader != null && headerText != null) this._lblHeader.SetText(headerText);
            if (this._lblContent != null && contentText != null) this._lblContent.SetText(contentText);

            // NOTE: このブロックだけ追加された。それ以外はコピペ
            if (initialSelection == RESULT_OK)
            {
                this._btnOk.Select();
            }
            else if (initialSelection == RESULT_CANCEL)
            {
                this._btnCancel.Select();
            }

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

        public virtual async UniTask SubmitCancel()
        {
            if (this._messageBoxHelper == null) throw new OperationCanceledException("Before Awake()");

            this._result = RESULT_CANCEL;
            await this._messageBoxHelper.CloseAsync(default);
        }
    }
}
