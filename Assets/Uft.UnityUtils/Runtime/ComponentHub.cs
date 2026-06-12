#nullable enable

using UnityEngine;

namespace Uft.UnityUtils
{
    public class ComponentHub
    {
        public enum Scope { Self, FirstChild }

        public Animator? Animator { get; }
        public Canvas? Canvas { get; }
        public CanvasGroup? CanvasGroup { get; }
        public RectTransform? RectTransform { get; }

        public ComponentHub(Component source, Scope scope = Scope.Self)
        {
            if (scope == Scope.Self)
            {
                this.Animator = source.GetComponent<Animator>();
                this.Canvas = source.GetComponent<Canvas>();
                this.CanvasGroup = source.GetComponent<CanvasGroup>();
                this.RectTransform = source.GetComponent<RectTransform>();
            }
            else
            {
                var child = source.transform.GetChild(0);
                this.Animator = child.GetComponent<Animator>();
                this.Canvas = child.GetComponent<Canvas>();
                this.CanvasGroup = child.GetComponent<CanvasGroup>();
                this.RectTransform = child.GetComponent<RectTransform>();
            }
        }
    }
}
