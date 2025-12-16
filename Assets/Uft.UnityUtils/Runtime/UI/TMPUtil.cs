#nullable enable

using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public static class TMPUtil
    {
        public const float ONE_FRAME = 0.016666f;

        /// <summary>
        /// 簡易的なタイプライターエフェクト。より実用的な機能が欲しい場合は、DOTween Proなどを使用してください
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="text"></param>
        /// <param name="wait_sec"></param>
        /// <param name="ct"></param>
        /// <param name="scrollRect"></param>
        /// <param name="scrollDuration_sec"></param>
        /// <returns></returns>
        public static async UniTask TypeWriterEffectAsync(this TMP_Text tmp, string text, float wait_sec = ONE_FRAME * 2, CancellationToken ct = default, ScrollRect? scrollRect = null, float scrollDuration_sec = 0.3f)
        {
            var charsPerFrame = wait_sec < 0.001f ? 1000 : ONE_FRAME / wait_sec;

            int index = 0;
            float acc = 0f;
            while (index < text.Length)
            {
                await UniTask.NextFrame(ct);

                if (!tmp) return;

                acc += charsPerFrame;
                if (acc < 1) continue;

                var emitCount = Mathf.RoundToInt(acc);
                acc = 0;
                for (int i = 0; i < emitCount && index < text.Length; i++)
                {
                    tmp.text += text[index];
                    index++;
                }
                if (scrollRect != null &&
                    0.1f < scrollRect.verticalNormalizedPosition &&
                    scrollRect.viewport.rect.height + 0.1f < scrollRect.content.rect.height &&
                    !DOTween.IsTweening(scrollRect))
                {
                    var t = scrollRect.DOVerticalNormalizedPos(0.0f, scrollDuration_sec);
                    await t.ToUniTask(cancellationToken: ct);
                }
            }
        }
    }
}
