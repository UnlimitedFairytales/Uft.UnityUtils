using System;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class MenuItemSample : MonoBehaviour
    {
        [SerializeField] RectTransform _cursor; public RectTransform Cursor => this._cursor;
        [SerializeField] Button _button; public Button Button => this._button;

        public Action OnTap;

        void Reset()
        {
            if (this._cursor == null) this._cursor = this.GetComponentInChildrenByName<RectTransform>("Cursor");
            if (this._button == null) this._button = this.GetComponentInChildren<Button>();
        }

        void Start()
        {
            this._button.onClick.Rebind(() => this.OnTap?.Invoke());
        }
    }
}
