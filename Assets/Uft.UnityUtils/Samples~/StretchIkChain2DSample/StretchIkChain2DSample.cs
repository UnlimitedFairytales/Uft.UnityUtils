using UnityEngine;

namespace Uft.UnityUtils.Samples.StretchIkChain2DSample
{
    public class StretchIkChain2DSample : MonoBehaviour
    {
        [SerializeField] Transform _target;

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this._target.position += Vector3.left * 0.02f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this._target.position += Vector3.right * 0.02f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this._target.position += Vector3.down * 0.02f;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this._target.position += Vector3.up * 0.02f;
            }
        }
    }
}
