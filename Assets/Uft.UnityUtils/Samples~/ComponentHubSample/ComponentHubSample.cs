using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.ComponentHubSample
{
    public class ComponentHubSample : MonoBehaviour
    {
        [SerializeField] Button _btnSelf;
        [SerializeField] Button _btnFirstChild;

        ComponentHub _selfHub;
        ComponentHub _firstChildHub;

        void Awake()
        {
            this._selfHub = new ComponentHub(this, ComponentHub.Scope.Self);
            this._firstChildHub = new ComponentHub(this, ComponentHub.Scope.FirstChild);
        }

        void Start()
        {
            this._btnSelf.onClick.AddListener(this.OnSelf);
            this._btnFirstChild.onClick.AddListener(this.OnFirstChild);

            Debug.Log($"[Self] Animator={this._selfHub.Animator}, Canvas={this._selfHub.Canvas}, CanvasGroup={this._selfHub.CanvasGroup}, RectTransform={this._selfHub.RectTransform}");
            Debug.Log($"[FirstChild] Animator={this._firstChildHub.Animator}, Canvas={this._firstChildHub.Canvas}, CanvasGroup={this._firstChildHub.CanvasGroup}, RectTransform={this._firstChildHub.RectTransform}");
        }

        void OnSelf()
        {
            if (this._selfHub.CanvasGroup != null)
                this._selfHub.CanvasGroup.alpha = 0.5f;
            if (this._selfHub.Animator != null)
                this._selfHub.Animator.Play("Show");
        }

        void OnFirstChild()
        {
            if (this._firstChildHub.CanvasGroup != null)
                this._firstChildHub.CanvasGroup.alpha = 0.5f;
            if (this._firstChildHub.Animator != null)
                this._firstChildHub.Animator.Play("Show");
        }
    }
}
