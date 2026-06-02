#if UNITY_EDITOR
using System.IO;
using Uft.UnityUtils.Save;
using UnityEditor;
using UnityEngine;

namespace Uft.UnityUtils.Editor
{
    public class EditorSaveDataTool
    {
        static readonly DevLogWithTag DevLog = new("[" + nameof(EditorSaveDataTool) + "]");

        [MenuItem("Tools/Uft.UnityUtils/Delete " + SaveApiWindows.EDITOR_SAVE_DATA_NAME, priority = 21062000 - 1)]
        static void DeleteEditorSaveData()
        {
            var title = "Delete " + SaveApiWindows.EDITOR_SAVE_DATA_NAME;

            var filePath = Path.Combine(Application.persistentDataPath, SaveApiWindows.EDITOR_SAVE_DATA_NAME);
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog(title, "File not found: " + filePath, "OK");
                return;
            }

            if (!EditorUtility.DisplayDialog(title, "Delete save data?\n" + filePath, "OK", "Cancel")) return;

            File.Delete(filePath);
            DevLog.Log($"Save data deleted. ({filePath})");
            EditorUtility.DisplayDialog(title, "Save data deleted.", "OK");
        }
    }
}
#endif
