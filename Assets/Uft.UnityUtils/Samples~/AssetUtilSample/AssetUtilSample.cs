using Cysharp.Threading.Tasks;
using System;
using TMPro;
using Uft.UnityUtils.Common;
using UnityEngine;

namespace Uft.UnityUtils.Samples.AssetUtilSample
{
    public class AssetUtilSample : MonoBehaviour
    {
        public const string PATH_PART1 = "Assets/Samples/Uft.UnityUtils";
        public const string PATH_PART2 = "AssetUtilSample/Scripts";
        const string fileName = "txt-sample.txt";

        [SerializeField] TMP_Text txtText1;
        [SerializeField] TMP_Text txtText2;

        void Start()
        {
            var srcPath = DirectoryUtil.GetLatestSampleSourceDirectory(PATH_PART1, PATH_PART2);
            var resourcesPath = DirectoryUtil.ToResourcesPath(srcPath);
            var streamingAssetsPath = DirectoryUtil.ToStreamingAssetsPath(srcPath);
            var relativePath = $"{resourcesPath}/{fileName}"["Assets/Resources/".Length..];
            {
                // NOTE: API動作確認                
                var relativePath2 = $"{streamingAssetsPath}/{fileName}"["Assets/StreamingAssets/".Length..];
                if (relativePath == relativePath2)
                {
                    DevLog.Log("relativePath and relativePath2 are the same.");
                }
            }
            try
            {
                this.txtText1.text = AssetUtil.LoadText(relativePath, true);
            }
            catch (Exception ex)
            {
                var message = "Please click \"Tools > Uft.UnityUtils.Samples > AssetUtilSample > ..., and restart";
                DevLog.LogWarning(ex.Message);
                this.txtText1.text = message;
            }

            UniTask.Void(async () =>
            {
                try
                {
                    await UniTask.WaitForSeconds(1);
                    this.txtText2.text = await AssetUtil.LoadTextAsync(relativePath, false);
                }
                catch (Exception ex)
                {
                    var message = "Please click \"Tools > Uft.UnityUtils.Samples > AssetUtilSample > ..., and restart";
                    DevLog.LogWarning(ex.Message);
                    this.txtText2.text = message;
                }
            });
        }
    }
}
