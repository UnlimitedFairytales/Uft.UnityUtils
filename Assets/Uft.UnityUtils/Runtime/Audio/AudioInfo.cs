#nullable enable

using System;
using System.Collections.Generic;

namespace Uft.UnityUtils.Audio
{
    [Serializable]
    public class AudioInfo
    {
        // Static members ======================================================

        public static (int loopStart, int loopLength) GetLoopInfo(string name, Dictionary<string, AudioInfo> audioInfoDict)
        {
            if (audioInfoDict.TryGetValue(name, out var info)) return (info.loopStart, info.loopLength);
            return (-1, -1);
        }

        // Parameters ==========================================================

        public string name = "";
        public int loopStart = -1;
        public int loopLength = -1;
    }
}
