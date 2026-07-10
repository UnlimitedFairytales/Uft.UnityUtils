#nullable enable
#if DEBUG || UNITY_EDITOR

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    // NOTE: 「Prefabモード中にPlay開始するとInputSystem の Navigateの参照構築が正しく行われないバグ」確認用。Unity6000.0.67f1
    [RequireComponent(typeof(Selectable))]
    public sealed class EditorBugViewer : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        Selectable? _selectable;
        bool _isStarted = false;

        void Start()
        {
            this._isStarted = true;
            this._selectable = this.GetComponent<Selectable>();
            this.LogState();
        }

        public void OnSelect(BaseEventData eventData) => this.LogState();
        public void OnDeselect(BaseEventData eventData) => this.LogState();

        void LogState([CallerMemberName] string caller = "")
        {
            if (this._selectable == null)
                Debug.LogWarning($"[{caller} NG] {this.StateString()}", this);
            else
                Debug.Log($"[{caller} OK] {this.StateString()}", this);
        }

        string StateString() =>
            $"{this.name}" +
            $"  _isStarted={this._isStarted}" +
            $"  editorBugViewer.instanceID={this.GetInstanceID()}" +
            $"  go.instanceID={this.gameObject.GetInstanceID()}" +
            $"  selectable.instanceID={(this._selectable != null ? this._selectable.GetInstanceID().ToString() : "null")}";
    }
}

#endif
