using System;
using Uft.UnityUtils.Common;
using UnityEditor;

namespace Uft.UnityUtils.Samples.ScriptSample.Editor
{
    public class CopyMenu
    {
        // from: Assets/Samples/Uft.UnityUtils/<version>/ScriptSample/Scripts/
        // dest: Assets/StreamingAssets/Samples/Uft.UnityUtils/<version>/ScriptSample/Scripts/

        const string MENU_ITEM_RS = "Copy to Resources";
        const string MENU_ITEM_SA = "Copy to StreamingAssets";

        [MenuItem("Tools/Uft.UnityUtils.Samples/ScriptSample/" + MENU_ITEM_RS)]
        private static void Copy2Resources()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToResources(ScriptSample.PATH_PART1, ScriptSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_RS, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Uft.UnityUtils.Samples/ScriptSample/" + MENU_ITEM_SA)]
        private static void Copy2StreamingAssets()
        {
            try
            {
                DirectoryUtil.CopyLatestSampleToStreamingAssets(ScriptSample.PATH_PART1, ScriptSample.PATH_PART2);
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(MENU_ITEM_SA, ex.Message, "OK");
            }
            AssetDatabase.Refresh();
        }
    }
}
