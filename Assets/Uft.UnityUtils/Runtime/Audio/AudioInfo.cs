#nullable enable

using System;

namespace Uft.UnityUtils.Audio
{
    [Serializable]
    public class AudioInfo
    {
        public string name = "";
        public int loopStart = -1;
        public int loopLength = -1;
    }
}
