using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class RebindSample : MonoBehaviour
    {
        [SerializeField] Toggle _tgl0;
        [SerializeField] Toggle _tgl1;
        [SerializeField] Toggle _tgl2;
        [SerializeField] CanvasGroup _cg0;
        [SerializeField] CanvasGroup _cg1;
        [SerializeField] CanvasGroup _cg2;
        [SerializeField] TMP_Text _txt;

        void Awake()
        {
            // NOTE: ToggleGroup 管理の切り替えは prev false → next true の順に1つずつ発生する
            this._tgl0.onValueChanged.Rebind(this._tgl0, this.ChangeTab);
            this._tgl1.onValueChanged.Rebind(this._tgl1, this.ChangeTab);
            this._tgl2.onValueChanged.Rebind(this._tgl2, this.ChangeTab);

            this._tgl0.SetIsOnWithoutNotify(true);
            this.ChangeTab(this._tgl0, true);
        }

        void Update()
        {
            var selected = EventSystemUtil.Selected;
            this._txt.text = selected != null ? $"Selected: {selected.name}({selected.GetInstanceID()})" : "Selected: (none)";
        }

        void ChangeTab(Toggle sender, bool isOn)
        {
            this._cg0.interactable = sender == this._tgl0 && isOn;
            this._cg1.interactable = sender == this._tgl1 && isOn;
            this._cg2.interactable = sender == this._tgl2 && isOn;
        }
    }
}
