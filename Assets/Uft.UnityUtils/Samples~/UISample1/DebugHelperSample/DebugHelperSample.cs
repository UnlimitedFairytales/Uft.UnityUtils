using Uft.UnityUtils.UI;
using UnityEngine;

namespace Uft.UnityUtils.Samples.UISample1
{
    public class DebugHelperSample : MonoBehaviour
    {
        [SerializeField] DebugHelper _helper;

        void Start()
        {
            if (Random.Range(0, 2) == 0)
            {
                this._helper.SetErrorText("Error がセットされた場合の例");
            }
        }
    }
}
