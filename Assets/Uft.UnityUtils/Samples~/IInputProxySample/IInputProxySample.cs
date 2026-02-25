using TMPro;
using UnityEngine;

namespace Uft.UnityUtils.Samples.IInputProxySample
{
    public class IInputProxySample : MonoBehaviour
    {
        [SerializeField] TMP_Text txtDebug;
        IInputProxy input;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            this.input = new SimpleInput();
        }

        void Update()
        {
            this.input.Update();
            this.txtDebug.text = this.input.DebugString("Click", "Attack");
        }
    }
}
