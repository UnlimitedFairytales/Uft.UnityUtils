using UnityEngine;

namespace Uft.UnityUtils
{
    public static class AudioUtil
    {
        public static float LinearToDecibel(float linearVolume)
        {
            linearVolume = Mathf.Clamp(linearVolume, 0.0001f, 1.0f);
            return 20 * Mathf.Log10(linearVolume); // -80 ~ 0
        }
    }
}
