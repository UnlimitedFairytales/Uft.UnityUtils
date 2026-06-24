#nullable enable

using System;
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
                this.WriteRaw(filePath, emptyData);
            }
        }

        public void Dispose() { }

        public byte[] ReadRaw(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{nameof(filePath)}={filePath}");
            }
            var buffer = File.ReadAllBytes(filePath);
            DevLog.Log($"{nameof(ReadRaw)}() loaded {buffer.Length} bytes.");
            return buffer;
        }

        public void WriteRaw(string filePath, byte[] buffer)
        {
            var tempFilePath = filePath + ".tmp";
            using (var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(buffer);
                fs.Flush(true);
            }
            if (File.Exists(filePath))
            {
                File.Replace(tempFilePath, filePath, null);
            }
            else
            {
                File.Move(tempFilePath, filePath);
            }
            DevLog.Log($"{nameof(WriteRaw)}() written {buffer.Length} bytes.");
        }

        public void MoveCorruptedFile(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupPath = $"{filePath}.{timestamp}.corrupted";
            File.Move(filePath, backupPath);
            DevLog.Log($"{nameof(MoveCorruptedFile)}() moved to {backupPath}");
        }
    }
}
