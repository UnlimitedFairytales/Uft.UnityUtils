#if !DISABLE_UNITYUTILS_AUTO_AUDIO_LOAD_TYPE_SETTER
using UnityEditor;
using UnityEngine;

namespace Uft.UnityUtils.Editor
{
    public class AutoAudioLoadTypeSetter : AssetPostprocessor
    {
        const float THRESHOLD1_sec = 1f;
        const float THRESHOLD2_sec = 10f;
        void OnPostprocessAudio(AudioClip clip)
        {
            if (!this.assetImporter.importSettingsMissing) return;

            var importer = (AudioImporter)this.assetImporter;
            var settings = importer.defaultSampleSettings;
            float length = clip.length;

            if (settings.loadType == AudioClipLoadType.DecompressOnLoad)
            {
                if (THRESHOLD1_sec <= length && length < THRESHOLD2_sec)
                {
                    settings.loadType = AudioClipLoadType.CompressedInMemory;
                    settings.compressionFormat = AudioCompressionFormat.ADPCM;
                    importer.defaultSampleSettings = settings;
                    importer.SaveAndReimport();
                    Debug.Log($"[{nameof(AutoAudioLoadTypeSetter)}] {this.assetImporter.assetPath} -> CompressedInMemory & ADPCM ({length:0.00} sec)");
                }
                else if (THRESHOLD2_sec <= length)
                {
                    settings.loadType = AudioClipLoadType.Streaming;
                    importer.defaultSampleSettings = settings;
                    importer.SaveAndReimport();
                    Debug.Log($"[{nameof(AutoAudioLoadTypeSetter)}] {this.assetImporter.assetPath} -> Streaming ({length:0.00} sec)");
                }
            }
        }
    }
}
#endif
