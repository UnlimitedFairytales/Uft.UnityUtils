using System;
using Uft.UnityUtils.Common;
using UnityEditor;

namespace Uft.UnityUtils.Samples.AssetUtilSample.Editor
{
    public class CopyMenu
    {
        // from: Assets/Samples/Uft.UnityUtils/<version>/AssetUtilSample/Scripts/
        // dest: Assets/StreamingAssets/Samples/Uft.UnityUtils/<version>/AssetUtilSample/Scripts/

        const string MENU_ITEM_RS = "Copy to Resources";
        const string MENU_ITEM_SA = "Copy to StreamingAssets";

        [MenuItem("Tools/Uft.UnityUtils.Samples/AssetUtilSample/" + MENU_ITEM_RS, priority = 1 + 19)]
        private static void Copy2Resources()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToResources(AssetUtilSample.PATH_PART1, AssetUtilSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_RS, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Uft.UnityUtils.Samples/AssetUtilSample/" + MENU_ITEM_SA, priority = 1 + 19)]
        private static void Copy2StreamingAssets()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToStreamingAssets(AssetUtilSample.PATH_PART1, AssetUtilSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_SA, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }
    }
}
