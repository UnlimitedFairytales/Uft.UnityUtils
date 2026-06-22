#nullable enable

using System.IO;

namespace Uft.UnityUtils.Save
{
    /// <summary>OSX, Windows共通</summary>
    public class SaveApiStandalone : ISaveApi
    {
        public const string EDITOR_SAVE_DATA_NAME = "EditorSaveData.dat";
        static readonly DevLogWithTag DevLog = new("[" + nameof(SaveApiStandalone) + "]");

        public void Initialize(string filePath, byte[] emptyData)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, emptyData);
            }
        }

        public void Dispose() { }

        public byte[] ReadRaw(string filePath)
        {
            if (!File.Exists(filePath))
            {
                DevLog.Log($"{nameof(ReadRaw)}() file not found. ({nameof(filePath)}={filePath})");
                throw new FileNotFoundException();
            }
            var buffer = File.ReadAllBytes(filePath);
            DevLog.Log($"{nameof(ReadRaw)}() loaded {buffer.Length} bytes.");
            return buffer;
        }

        public void WriteRaw(string filePath, byte[] buffer)
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(buffer);
            DevLog.Log($"{nameof(WriteRaw)}() written {buffer.Length} bytes.");
        }
    }
}
