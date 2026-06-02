#nullable enable

using System.IO;

namespace Uft.UnityUtils.Save
{
    public class SaveApiWindows : ISaveApi
    {
        public const string EDITOR_SAVE_DATA_NAME = "EditorSaveData.dat";
        static readonly DevLogWithTag DevLog = new("[" + nameof(SaveApiWindows) + "]");

        public void Initialize() { }

        public void Dispose() { }

        /// <summary><paramref name="size"/> 未使用</summary>
        public byte[]? ReadRaw(string filePath, int offset, int size = 0)
        {
            if (!File.Exists(filePath))
            {
                DevLog.Log($"{nameof(ReadRaw)}() file not found. ({nameof(filePath)}={filePath})");
                return null;
            }
            var buffer = File.ReadAllBytes(filePath);
            DevLog.Log($"{nameof(ReadRaw)}() loaded {buffer.Length} bytes.");
            return buffer;
        }

        public void WriteRaw(string filePath, int offset, byte[] buffer)
        {
            using var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(buffer);
            DevLog.Log($"{nameof(WriteRaw)}() ({nameof(offset)}={offset}, length={buffer.Length})");
        }
    }
}
