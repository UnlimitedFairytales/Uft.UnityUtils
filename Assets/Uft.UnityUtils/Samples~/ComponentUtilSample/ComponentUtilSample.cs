using System;
using TMPro;
using UnityEngine;

namespace Uft.UnityUtils.Samples.ComponentUtilSample
{
    public class ComponentUtilSample : MonoBehaviour
    {
        [SerializeField] GameObject _prototype;

        void Awake()
        {
            var instance = this._prototype.transform.Instantiate(null, false, true);
            var txtList = instance.transform.GetComponentsInChildrenOrderByName<TMP_Text>(true, t => t.name.Contains("txt", StringComparison.OrdinalIgnoreCase));
            for (int i = 0; i < txtList.Count; i++)
            {
                var t = txtList[i];
                t.text = $"YEAH! {i}";
            }
        }
    }
}
