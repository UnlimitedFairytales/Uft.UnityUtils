using Assets.Uft.UnityUtils.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UnityEventUtilSample
{
    public class UnityEventUtilSample : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] Toggle _toggle;

        void Start()
        {
            _button.onClick.Rebind(OnTap);
            _toggle.onValueChanged.Rebind(OnTap2);
        }

        void OnTap() => Debug.Log("Tap");
        void OnTap2(bool isOn) => Debug.Log($"Tap2:{isOn}");
    }
}
