using TMPro;
using UnityEngine;

namespace Uft.UnityUtils.Samples.ScriptSample
{
    public class CacheSample : MonoBehaviour
    {
        [SerializeField] GameObject _obj;
        TMP_Text _txtText1; public TMP_Text TxtText1 => CacheUtil.GetCachedComponent(ref this._txtText1, this._obj.transform);
        TMP_Text _txtText2; public TMP_Text TxtText2 => CacheUtil.GetCachedChildComponent(ref this._txtText2, this._obj.transform, true, "txtText2");
        TMP_Text[] _txtTextList; public TMP_Text[] TxtTextList => CacheUtil.GetCachedChildrenComponents(ref this._txtTextList, this._obj.transform, true);

        [SerializeField] GameObject _objPrototype;
        GameObject _instantiated; public GameObject Instantiated => CacheUtil.GetCreatedObject(ref this._instantiated, this._objPrototype);

        void Update()
        {
            this.TxtText1.text = this.TxtText1 == this.TxtTextList[0] ? "yeah1" : "Huh?";
            this.TxtText2.text = this.TxtText2 == this.TxtTextList[1] ? "yeah2" : "Huh??";

            if (!this.Instantiated.activeSelf)
            {
                this.Instantiated.SetActive(true);
            }
        }
    }
}
