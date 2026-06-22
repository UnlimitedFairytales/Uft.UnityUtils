#nullable enable

using System;

namespace Uft.UnityUtils.Save
{
    public interface ISaveApi : IDisposable
    {
        void Initialize(string filePath, byte[] emptyData);
        byte[] ReadRaw(string filePath);
        void WriteRaw(string filePath, byte[] buffer);
    }
}
