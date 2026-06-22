using System;
using System.Collections.Generic;
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
            var path = Path.Combine(Application.persistentDataPath, SaveApiStandalone.EDITOR_SAVE_DATA_NAME);

            var saveConverter = new SaveConverter<SaveData>(4096); // 4K bytes
            using var saveApi = new SaveApiStandalone();

            // Initialize
            var emptyData = saveConverter.Serialize(new SaveData());
            saveApi.Initialize(path, emptyData);

            // Read
            var payload = saveApi.ReadRaw(path);
            var saveData = saveConverter.Deserialize(payload);
            DevLog.Log($"SaveData={saveData}");

            // Update
            saveData.stage++;
            if (saveData.items.Contains("sword"))
            {
                saveData.items.Add("potion");
            }
            else
            {
                saveData.items.Add("sword");
            }

            // Write
            payload = saveConverter.Serialize(saveData);
            saveApi.WriteRaw(path, payload);
        }
    }

    [Serializable]
    public class SaveData
    {
        public int stage;
        public List<string> items = new();

        public override string ToString() => $"{this.stage},{string.Join(',', this.items)}";
    }
}
