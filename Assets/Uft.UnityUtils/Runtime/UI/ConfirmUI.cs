#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class ConfirmUI : ToastUI
    {
        [SerializeField] Button? _btnCancel;

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

        public virtual async UniTask SubmitCancel()
        {
            if (this._messageBoxHelper == null) throw new OperationCanceledException("Before Awake()");

            this._result = RESULT_CANCEL;
            await this._messageBoxHelper.CloseAsync(default);
        }
    }
}
