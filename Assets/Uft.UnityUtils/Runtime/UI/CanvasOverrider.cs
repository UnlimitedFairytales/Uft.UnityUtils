#nullable enable

using UnityEngine;

namespace Uft.UnityUtils
{
    [DefaultExecutionOrder(9000)]
    [RequireComponent(typeof(Canvas))]
    public class CanvasOverrider : MonoBehaviour
    {
        [SerializeField] string overrideSortingLayerName = "Default";
        [SerializeField] int overrideSortingOrder = 0;

        Canvas? _canvas;

        protected virtual void Awake()
        {
            this._canvas = this.GetComponent<Canvas>();
            this.ApplySortingLayer();
        }

        protected virtual void OnTransformParentChanged()
        {
            this.ApplySortingLayer();
        }

        protected virtual void ApplySortingLayer()
        {
            if (this._canvas == null) this._canvas = this.GetComponent<Canvas>();
            if (this._canvas == null) return;
            this._canvas.sortingLayerName = this.overrideSortingLayerName;
            this._canvas.sortingOrder = this.overrideSortingOrder;
        }
    }
}
