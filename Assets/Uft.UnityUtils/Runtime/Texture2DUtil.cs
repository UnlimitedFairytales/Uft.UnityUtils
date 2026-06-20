#nullable enable

using UnityEngine;

namespace Uft.UnityUtils
{
    // TODO: sample

    public static class Texture2DUtil
    {
        public static Texture2D? Create(byte[] pngOrJpeg)
        {
            var texture = new Texture2D(1, 1);
            if (texture.LoadImage(pngOrJpeg)) return texture;
            Object.Destroy(texture);
            return null;
        }
    }
}
