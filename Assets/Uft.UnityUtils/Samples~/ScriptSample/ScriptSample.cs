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
