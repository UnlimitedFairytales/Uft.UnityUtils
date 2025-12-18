#nullable enable

using UnityEngine;

namespace Uft.UnityUtils
{
    public static class AudioUtil
    {
        public static float LinearToDecibel(float linearVolume01)
        {
            linearVolume01 = Mathf.Clamp(linearVolume01, 0.0001f, 1.0f);
            return 20 * Mathf.Log10(linearVolume01); // -80 ~ 0
        }
    }
}
