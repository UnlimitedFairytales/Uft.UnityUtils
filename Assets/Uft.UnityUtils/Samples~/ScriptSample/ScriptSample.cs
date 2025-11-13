using TMPro;
using UnityEngine;
using UnityHelpers;

namespace Uft.UnityUtils.Samples.ScriptSample
{
    public class ScriptSample : MonoBehaviour
    {
        public const string PATH_PART1 = "Assets/Samples/Uft.UnityUtils";
        public const string PATH_PART2 = "ScriptSample/Scripts";
        const string fileName = "txt-sample.txt";

        [SerializeField] TMP_Text txtText1;
        [SerializeField] TMP_Text txtText2;

        void Awake()
        {
            DevLog.Log(DevLog.red, color: DevLog.red);
            DevLog.Log(DevLog.lime, color: DevLog.lime);
            DevLog.Log(DevLog.blue, color: DevLog.blue);

            DevLog.Log(DevLog.cyan, color: DevLog.cyan);
            DevLog.Log(DevLog.magenta, color: DevLog.magenta);
            DevLog.Log(DevLog.yellow, color: DevLog.yellow);

            DevLog.Log(DevLog.maroon, color: DevLog.maroon);
            DevLog.Log(DevLog.green, color: DevLog.green);
            DevLog.Log(DevLog.navy, color: DevLog.navy);

            DevLog.Log(DevLog.teal, color: DevLog.teal);
            DevLog.Log(DevLog.purple, color: DevLog.purple);
            DevLog.Log(DevLog.olive, color: DevLog.olive);

            DevLog.Log(DevLog.white, color: DevLog.white);
            DevLog.Log(DevLog.silver, color: DevLog.silver);
            DevLog.Log(DevLog.grey, color: DevLog.grey);
            DevLog.Log(DevLog.black, color: DevLog.black);

            DevLog.LogWarning(DevLog.red, color: DevLog.red);
            DevLog.LogWarning(DevLog.lime, color: DevLog.lime);
            DevLog.LogWarning(DevLog.blue, color: DevLog.blue);

            DevLog.LogWarning(DevLog.cyan, color: DevLog.cyan);
            DevLog.LogWarning(DevLog.magenta, color: DevLog.magenta);
            DevLog.LogWarning(DevLog.yellow, color: DevLog.yellow);

            DevLog.LogWarning(DevLog.maroon, color: DevLog.maroon);
            DevLog.LogWarning(DevLog.green, color: DevLog.green);
            DevLog.LogWarning(DevLog.navy, color: DevLog.navy);

            DevLog.LogWarning(DevLog.teal, color: DevLog.teal);
            DevLog.LogWarning(DevLog.purple, color: DevLog.purple);
            DevLog.LogWarning(DevLog.olive, color: DevLog.olive);

            DevLog.LogWarning(DevLog.white, color: DevLog.white);
            DevLog.LogWarning(DevLog.silver, color: DevLog.silver);
            DevLog.LogWarning(DevLog.grey, color: DevLog.grey);
            DevLog.LogWarning(DevLog.black, color: DevLog.black);

            DevLog.LogError(DevLog.red, color: DevLog.red);
            DevLog.LogError(DevLog.lime, color: DevLog.lime);
            DevLog.LogError(DevLog.blue, color: DevLog.blue);

            DevLog.LogError(DevLog.cyan, color: DevLog.cyan);
            DevLog.LogError(DevLog.magenta, color: DevLog.magenta);
            DevLog.LogError(DevLog.yellow, color: DevLog.yellow);

            DevLog.LogError(DevLog.maroon, color: DevLog.maroon);
            DevLog.LogError(DevLog.green, color: DevLog.green);
            DevLog.LogError(DevLog.navy, color: DevLog.navy);

            DevLog.LogError(DevLog.teal, color: DevLog.teal);
            DevLog.LogError(DevLog.purple, color: DevLog.purple);
            DevLog.LogError(DevLog.olive, color: DevLog.olive);

            DevLog.LogError(DevLog.white, color: DevLog.white);
            DevLog.LogError(DevLog.silver, color: DevLog.silver);
            DevLog.LogError(DevLog.grey, color: DevLog.grey);
            DevLog.LogError(DevLog.black, color: DevLog.black);
        }

        void Start()
        {
            var srcPath = DirectoryUtil.GetLatestSampleSourceDirectory(PATH_PART1, PATH_PART2);
            var resourcesPath = DirectoryUtil.ToResourcesPath(srcPath);
            var relativePath = $"{resourcesPath}/{fileName}"["Assets/Resources/".Length..];
            {
                // NOTE: API動作確認
                var streamingAssetsPath = DirectoryUtil.ToStreamingAssetsPath(srcPath);
                var relativePath2 = $"{streamingAssetsPath}/{fileName}"["Assets/StreamingAssets/".Length..];
                if (relativePath == relativePath2)
                {
                    Debug.Log("relativePath and relativePath2 are the same.");
                }
            }

            this.txtText1.text = AssetUtil.LoadText(relativePath, true);
            if (string.IsNullOrWhiteSpace(this.txtText1.text))
            {
                var message = "Please click \"Tools > Uft.UnityUtils.Samples > ScriptSample > ..., and restart";
                Debug.LogWarning(message);
                this.txtText1.text = message;
            }

            this.txtText2.text = AssetUtil.LoadText(relativePath, false);
            if (string.IsNullOrWhiteSpace(this.txtText2.text))
            {
                var message = "Please click \"Tools > Uft.UnityUtils.Samples > ScriptSample > ..., and restart";
                Debug.LogWarning(message);
                this.txtText2.text = message;
            }
        }
    }
}
