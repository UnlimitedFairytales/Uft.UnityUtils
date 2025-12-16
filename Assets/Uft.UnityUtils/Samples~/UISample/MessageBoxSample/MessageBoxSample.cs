using Uft.UnityUtils.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class MessageBoxSample : MonoBehaviour
    {
        [SerializeField] Button _button;

        [SerializeField]
        ToastUI _toastUI_prefab;
        ToastUI _toastUI;

        void Start()
        {
            this._toastUI = ComponentUtil.Instantiate(this._toastUI_prefab, this.transform, false, false);
            this._button.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                await this._toastUI.ShowAsync("Header text", "This is content text.", 5);
                Debug.Log($"{nameof(this._toastUI.ShowAsync)} complete");
            }));
        }
    }
}
