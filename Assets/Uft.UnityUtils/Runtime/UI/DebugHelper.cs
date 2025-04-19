using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class DebugHelper : MonoBehaviour
    {
        [SerializeField] Text _text;
        [SerializeField] float interval = 0.5f;
        Color _normalColor;
        float _timer = 0f;
        int _frameCount = 0;
        float _fps;

        void Start()
        {
            if (_text == null)
            {
                _text = TextUtil.CreateTextWithCanvas("initializing", 200, 100);
                TextUtil.Arrange(_text, AnchorPreset.TopLeft);
                _text.rectTransform.anchoredPosition = new Vector2(100, -100);
            }
            _normalColor = _text.color;
        }

        void Update()
        {
            _timer += Time.unscaledDeltaTime;
            _frameCount++;
            if (_timer >= interval)
            {
                _fps = _frameCount / _timer;
                _text.color = (_fps < 30f) ? Color.red : _normalColor;
                _timer = 0f;
                _frameCount = 0;
            }
            long totalReserved = Profiler.GetTotalReservedMemoryLong() / (1024 * 1024);
            long totalUsed = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);
            long monoUsed = Profiler.GetMonoUsedSizeLong() / (1024 * 1024);

            _text.text = $"{_fps:F1} FPS" + "\n" +
                $"Total Reserved: {totalReserved} MB\n" +
                $"Used: {totalUsed} MB\n" +
                $"Mono: {monoUsed} MB";
        }
    }
}