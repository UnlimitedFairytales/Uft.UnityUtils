#nullable enable

using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class DebugHelper : MonoBehaviour
    {
        [SerializeField] Text? _text;
        [SerializeField] float _interval = 0.5f;

        Color _normalColor;
        float _timer = 0f;
        int _frameCount = 0;
        float _fps;

        protected virtual void Awake()
        {
            if (this._text == null)
            {
                this._text = TextUtil.CreateTextWithCanvas("initializing", 200, 100);
                TextUtil.Arrange(this._text, AnchorPreset.TopLeft);
                this._text.rectTransform.anchoredPosition = new Vector2(100, -100);
            }
            this._normalColor = this._text.color;
        }

        protected virtual void Update()
        {
            this._timer += Time.unscaledDeltaTime;
            this._frameCount++;
            if (this._timer >= this._interval)
            {
                this._fps = this._frameCount / this._timer;
                this._text!.color = (this._fps < 30f) ? Color.red : this._normalColor;
                this._timer -= this._interval;
                this._frameCount = 0;

                long totalReserved = Profiler.GetTotalReservedMemoryLong() / (1024 * 1024);
                long totalUsed = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);
                long monoUsed = Profiler.GetMonoUsedSizeLong() / (1024 * 1024);

                this._text!.text = $"{this._fps:F1} FPS" + "\n" +
                    $"Total Reserved: {totalReserved} MB\n" +
                    $"Used: {totalUsed} MB\n" +
                    $"Mono: {monoUsed} MB";
            }
        }
    }
}
