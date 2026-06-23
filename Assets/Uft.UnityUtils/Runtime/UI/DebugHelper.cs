#nullable enable

using UnityEngine;
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
            var result = ProfilerUtil.FpsAndMemory(ref this._timer, ref this._frameCount, this._interval);
            if (result != null)
            {
                this._fps = result.Value.fps;
                this._text!.text = result.Value.text;
                this._text!.color = this._fps < 30 ? Color.red : this._normalColor;
            }
        }
    }
}
