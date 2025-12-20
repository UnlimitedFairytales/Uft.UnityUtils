using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uft.UnityUtils.Samples.PlayerLoopSample
{
    public class PlayerLoopSample : MonoBehaviour
    {
        const string NAME = "[" + nameof(PlayerLoopSample) + "]";

        void Start()
        {
            DevLog.Log($"{NAME} Start()");
            SceneManager.LoadScene("PlayerLoopSample_Additive", LoadSceneMode.Additive);
        }
    }
}
