using System;
using Uft.UnityUtils.Common;
using UnityEditor;

namespace Uft.UnityUtils.Samples.CsvUtilSample.Editor
{
    public class CopyMenu
    {
        // from: Assets/Samples/Uft.UnityUtils/<version>/CsvUtilSample/Scripts/
        // dest: Assets/StreamingAssets/Samples/Uft.UnityUtils/<version>/CsvUtilSample/Scripts/

        const string MENU_ITEM_SA = "Copy to StreamingAssets";

        [MenuItem("Tools/Uft.UnityUtils.Samples/CsvUtilSample/" + MENU_ITEM_SA, priority = 3 + 19)]
        private static void Copy2StreamingAssets()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToStreamingAssets(CsvUtilSample.PATH_PART1, CsvUtilSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_SA, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }
    }
}
