#nullable enable

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.UI
{
    public class TextAreaUI : MonoBehaviour
    {
        [SerializeField] protected Image? _imgBg;
        [SerializeField] protected TMP_Text? _txtText;
        [SerializeField] protected Color _defaultBgColor;
        [SerializeField] protected Color _defaultColor;
        [SerializeField] protected bool _autoDeactivate;

        void Reset()
        {
            if (this._imgBg == null) this._imgBg = this.GetComponentInChildrenByName<Image>("imgBg");
            if (this._txtText == null) this._txtText = this.GetComponentInChildrenByName<TMP_Text>("txtText");
            this._defaultBgColor = new Color32(32, 32, 32, 192);
            this._defaultColor = Color.white;
            this._autoDeactivate = true;
            this.OnValidate();
        }

        void OnValidate()
        {
            if (Application.isPlaying) return;

            if (this._imgBg != null) this._imgBg.color = this._defaultBgColor;
            if (this._txtText != null) this._txtText.color = this._defaultColor;
        }

        void Awake() => this.SetText("", active: false);

        public void SetText(string text, Color? color = null, Color? bgColor = null, int? width = null, int? height = null, bool? active = null)
        {
            if (this._imgBg != null)
            {
                var currentSize = this._imgBg.rectTransform.sizeDelta;
                this._imgBg.rectTransform.sizeDelta = new Vector2(width ?? currentSize.x, height ?? currentSize.y);
                this._imgBg.color = bgColor ?? this._defaultBgColor;
            }
            if (this._txtText != null)
            {
                this._txtText.text = text;
                this._txtText.color = color ?? this._defaultColor;
            }
            bool nextActive = active ?? (!this._autoDeactivate || !string.IsNullOrEmpty(text));
            this.gameObject.SetActive(nextActive);
        }
    }
}
