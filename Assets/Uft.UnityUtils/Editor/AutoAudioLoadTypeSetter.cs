#if !DISABLE_UNITYUTILS_AUTO_AUDIO_LOAD_TYPE_SETTER
using UnityEditor;
using UnityEngine;

namespace Uft.UnityUtils.Editor
{
    public class AutoAudioLoadTypeSetter : AssetPostprocessor
    {
        const float THRESHOLD_sec = 10f;
        void OnPostprocessAudio(AudioClip clip)
        {
            if (!this.assetImporter.importSettingsMissing) return;

            var importer = (AudioImporter)this.assetImporter;
            var settings = importer.defaultSampleSettings;

            float length = clip.length;
            if (length >= THRESHOLD_sec && settings.loadType == AudioClipLoadType.DecompressOnLoad)
            {
                settings.loadType = AudioClipLoadType.Streaming;
                importer.defaultSampleSettings = settings;
                importer.SaveAndReimport();
                Debug.Log($"[{nameof(AutoAudioLoadTypeSetter)}] {this.assetImporter.assetPath} -> Streaming ({length:0} sec)");
            }
        }
    }
}
#endif
