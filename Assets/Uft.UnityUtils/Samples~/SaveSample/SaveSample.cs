using System.IO;
using Uft.UnityUtils.Save;
using UnityEngine;

namespace Uft.UnityUtils.Samples.SaveSample
{
    public class SaveSample : MonoBehaviour
    {
        static readonly DevLogWithTag DevLog = new("[" + nameof(SaveSample) + "]");

        void Start()
        {
            var path = Path.Combine(Application.persistentDataPath, SaveApiWindows.EDITOR_SAVE_DATA_NAME);
            using var saveApi = new SaveApiWindows();
            var bytes = saveApi.ReadRaw(path, 0) ?? new byte[] { 0 };
            DevLog.Log($"SaveData={bytes[0]}");
            bytes[0]++;
            saveApi.WriteRaw(path, 0, bytes);
        }
    }
}
