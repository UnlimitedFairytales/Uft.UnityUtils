using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using Uft.UnityUtils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class TMPUtilSample : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] TMP_Text _tmpText;
        [SerializeField] ScrollRect _scrollRect;

        CancellationTokenSource _lastCts = null;

        void Start()
        {
            this._button.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                if (this._lastCts != null)
                {
                    this._lastCts.Cancel(); // NOTE: UniTaskはこの行が終わる前にawaitがキャンセルされる場合がある
                    if (this._lastCts != null)
                    {
                        this._lastCts.Dispose();
                        this._lastCts = null;
                    }
                }
                this._tmpText.text = "";
                var cts = new CancellationTokenSource();
                this._lastCts = cts;
                try
                {
                    await this._tmpText.TypeWriterEffectAsync(@"O, that this too too solid flesh would melt,
Thaw, and resolve itself into a dew!
Or that the Everlasting had not fix'd
His canon 'gainst self-slaughter! O God! God!
How weary, stale, flat, and profitable
Seem to me all the uses of this world!", TMPUtil.ONE_FRAME * 2, cts.Token, this._scrollRect);
                }
                catch (OperationCanceledException)
                {
                    DevLog.Log($"{nameof(TMPUtil.TypeWriterEffectAsync)} canceled");
                }
                finally
                {
                    if (cts == this._lastCts)
                    {
                        this._lastCts.Dispose();
                        this._lastCts = null;
                    }
                }
            }));
        }
    }
}
