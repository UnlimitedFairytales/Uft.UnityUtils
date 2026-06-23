#nullable enable

using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class DebugHelper : MonoBehaviour
    {
        [SerializeField] Text? _txt;
        [SerializeField] float _interval = 0.5f;

        Color _normalColor;
        float _timer = 0f;
        int _frameCount = 0;
        float _fps;
        string _fpsAndMemoryText = "";
        string _errorText = "";

        protected virtual void Awake()
        {
            if (this._txt == null)
            {
                this._txt = TextUtil.CreateTextWithCanvas("initializing", 200, 100);
                TextUtil.Arrange(this._txt, AnchorPreset.TopLeft);
                this._txt.rectTransform.anchoredPosition = new Vector2(100, -100);
            }
            this._normalColor = this._txt.color;
        }

        protected virtual void Update()
        {
            var result = ProfilerUtil.FpsAndMemory(ref this._timer, ref this._frameCount, this._interval);
            if (result != null)
            {
                this._fps = result.Value.fps;
                this._fpsAndMemoryText = result.Value.text;
                this.RefreshText();
            }
        }

        protected void RefreshText()
        {
            if (this._txt == null) return;

            if (string.IsNullOrEmpty(this._errorText))
            {
                this._txt.text = this._fpsAndMemoryText;
                this._txt.color = this._fps < 30 ? Color.yellow : this._normalColor;
            }
            else
            {
                this._txt.text = this._fpsAndMemoryText + "\n\n" + this._errorText;
                this._txt.color = Color.red;
            }
        }

        public void SetErrorText(string text)
        {
            if (this._errorText != text)
            {
                this._errorText = text;
                this.RefreshText();
            }
        }
    }
}
