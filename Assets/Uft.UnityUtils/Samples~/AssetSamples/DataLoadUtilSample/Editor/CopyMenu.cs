using System;
using Uft.UnityUtils.Common;
using UnityEditor;

namespace Uft.UnityUtils.Samples.AssetSamples.DataLoadUtilSample.Editor
{
    public class CopyMenu
    {
        // from: Assets/Samples/Uft.UnityUtils/<version>/AssetSamples/DataLoadUtilSample/Scripts/
        // dest: Assets/StreamingAssets/Samples/Uft.UnityUtils/<version>/AssetSamples/DataLoadUtilSample/Scripts/

        const string MENU_ITEM_RS = "Copy to Resources";
        const string MENU_ITEM_SA = "Copy to StreamingAssets";

        [MenuItem("Tools/Uft.UnityUtils.Samples/AssetSamples.DataLoadUtilSample/" + MENU_ITEM_RS, priority = 21062000 + 10, secondaryPriority = 1)]
        private static void Copy2Resources()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToResources(DataLoadUtilSample.PATH_PART1, DataLoadUtilSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_RS, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Uft.UnityUtils.Samples/AssetSamples.DataLoadUtilSample/" + MENU_ITEM_SA, priority = 21062000 + 10, secondaryPriority = 2)]
        private static void Copy2StreamingAssets()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToStreamingAssets(DataLoadUtilSample.PATH_PART1, DataLoadUtilSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_SA, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }
    }
}
