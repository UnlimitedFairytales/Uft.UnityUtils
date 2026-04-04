#nullable enable

using System;

namespace Uft.UnityUtils
{
    public interface ISaveApi : IDisposable
    {
        void Initialize();
        byte[] ReadLaw(string filePath, int offset, int size);
        void WriteRaw(string filePath, int offset, byte[] buffer);
    }
}
