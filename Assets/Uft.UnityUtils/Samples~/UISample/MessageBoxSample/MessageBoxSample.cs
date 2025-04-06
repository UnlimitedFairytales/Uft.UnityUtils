using Uft.UnityUtils.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples
{
    public class MessageBoxSample : MonoBehaviour
    {
        [SerializeField] Button _button;

        [SerializeField]
        ToastUI _toastUI_prefab;
        ToastUI _toastUI;

        void Start()
        {
            _toastUI = ComponentUtil.Instantiate(_toastUI_prefab, transform, false, false);
            _button.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await _toastUI.ShowAsync("Header text", "This is content text.", 5);
                Debug.Log($"{nameof(_toastUI.ShowAsync)} complete");
            }));
        }
    }
}
