using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uft.UnityUtils.Samples.PlayerLoopSample
{
    public class PlayerLoopSample : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene("PlayerLoopSample_Additive", LoadSceneMode.Additive);
        }
    }
}
