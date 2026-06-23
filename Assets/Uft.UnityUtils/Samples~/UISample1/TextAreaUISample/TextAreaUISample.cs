using Uft.UnityUtils.UI;
using UnityEngine;

namespace Uft.UnityUtils.Samples.UISample1
{
    public class TextAreaUISample : MonoBehaviour
    {
        [SerializeField] TextAreaUI _errorUI;

        void Start()
        {
            this._errorUI.SetText($"{nameof(TextAreaUISample)} sample text.");
        }
    }
}
