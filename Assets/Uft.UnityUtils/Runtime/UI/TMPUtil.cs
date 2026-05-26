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
        /// <summary>
        /// 簡易的なタイプライターエフェクト。より実用的な機能が欲しい場合は、DOTween Proなどを使用してください
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="text"></param>
        /// <param name="appends"></param>
        /// <param name="interval_sec"></param>
        /// <param name="scrollRect"></param>
        /// <param name="scrollDuration_sec"></param>
        /// <returns></returns>
        public static async UniTask TypeWriterEffectAsync(this TMP_Text txt,
            CancellationToken cancellationToken,
            string text,
            bool appends = false,
            float interval_sec = 0.0333f,
            ScrollRect? scrollRect = null,
            float scrollDuration_sec = 0.3f)
        {
            if (appends)
            {
                var v = txt.textInfo.characterCount;
                txt.text += text;
                txt.maxVisibleCharacters = v;
            }
            else
            {
                txt.text = text;
                txt.maxVisibleCharacters = 0;
            }
            txt.ForceMeshUpdate();
            if (scrollRect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            }

            var currentLine = -1;
            var intervalCounter_sec = 0f;
            while (txt.maxVisibleCharacters < txt.textInfo.characterCount)
            {
                await UniTask.NextFrame(cancellationToken);

                if (!txt) return;

                intervalCounter_sec += Time.deltaTime;
                while (txt.maxVisibleCharacters < txt.textInfo.characterCount
                    && interval_sec <= intervalCounter_sec)
                {
                    intervalCounter_sec -= interval_sec;
                    txt.maxVisibleCharacters++;
                }

                /*
                 *                             ┬ content y
                 *                             │
                 *                             │
                 *              ↑             │
                 * viewport y┬ ┬ content y   │
                 *           │ │             │
                 *           │ │             │
                 *           ┴ │             ┴
                 *              │
                 *              │
                 *              │
                 *              ┴
                 */
                if (scrollRect != null && 0 < txt.maxVisibleCharacters &&
                    !DOTween.IsTweening(scrollRect.content)) // NOTE: scrollRectのアサインが不十分だとNREしうるが、考慮外とする
                {
                    var checkIndex = Mathf.Min(txt.maxVisibleCharacters, txt.textInfo.characterCount - 1);
                    var checkLine  = txt.textInfo.characterInfo[checkIndex].lineNumber;
                    if (currentLine != checkLine)
                    {
                        if (currentLine <= 0) currentLine = 0;
                        var currentScrollY  = scrollRect.content.anchoredPosition.y;
                        var scrollRange     = scrollRect.content.rect.height - scrollRect.viewport.rect.height;
                        var lineHeight      = txt.textInfo.lineInfo[currentLine].lineHeight;
                        currentLine = checkLine;

                        // content y に 対する相対座標
                        var viewportTopY    = -scrollRect.content.anchoredPosition.y;
                        var viewportBottomY = viewportTopY - scrollRect.viewport.rect.height;
                        var lineTopY        = txt.textInfo.lineInfo[checkLine].ascender;
                        var lineBottomY     = txt.textInfo.lineInfo[checkLine].descender;
                        var isAboveViewport = lineTopY    > viewportTopY;
                        var isBelowViewport = lineBottomY < viewportBottomY;
                        if (isAboveViewport || isBelowViewport)
                        {
                            var targetY = isAboveViewport
                                ? Mathf.Max(-lineTopY, 0f)
                                : Mathf.Min(currentScrollY + lineHeight, scrollRange);
                            await scrollRect.content.DOAnchorPosY(targetY, scrollDuration_sec).WithCancellation(cancellationToken);
                        }
                    }
                }
            }
        }
    }
}
