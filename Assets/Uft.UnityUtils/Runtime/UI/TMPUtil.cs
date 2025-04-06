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
        public const float ONE_FRAME = 0.0166f;

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
        public static async UniTask TypeWriterEffectAsync(this TMP_Text tmp, string text, float wait_sec = ONE_FRAME * 2, CancellationToken ct = default, ScrollRect scrollRect = null, float scrollDuration_sec = 0.3f)
        {
            var waitCounter = 0.0f;
            foreach (var c in text)
            {
                tmp.text += c.ToString();
                waitCounter += wait_sec;
                if (ONE_FRAME <= waitCounter)
                {
                    await UniTask.WaitForSeconds(wait_sec);
                    waitCounter = 0.0f;
                }
                if (ct.IsCancellationRequested) return;
                if (scrollRect != null &&
                    0.1f < scrollRect.verticalNormalizedPosition &&
                    scrollRect.viewport.rect.height + 0.1f < scrollRect.content.rect.height &&
                    !DOTween.IsTweening(scrollRect))
                {
                    await scrollRect.DOVerticalNormalizedPos(0.0f, scrollDuration_sec);
                }
            }
        }
    }
}
