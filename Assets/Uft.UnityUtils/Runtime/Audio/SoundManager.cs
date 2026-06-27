#nullable enable

using System.Collections.Generic;
using Uft.UnityUtils.Asset;
using UnityEngine;
using UnityEngine.Audio;

namespace Uft.UnityUtils.Audio
{
    public class SoundManager : MonoBehaviour
    {
        // Parameters ==========================================================

        [SerializeField] SoundManagerCore _core = null!;

        Dictionary<string, AudioInfo> _audioInfoDict = null!;
        AssetLoadProxy _assetLoadProxy = null!;

        public AudioMixer AudioMixer => this._core.AudioMixer;
        public bool IsAnyVoicePlaying => this._core.IsAnyVoicePlaying;

        // Methods =============================================================

        public void Setup(Dictionary<string, AudioInfo> audioInfoDict, AssetLoadProxy assetLoadProxy)
        {
            this._audioInfoDict = audioInfoDict;
            this._assetLoadProxy = assetLoadProxy;
        }

        public void SetOutput(AudioMixer? audioMixer = null, string bgmName = "BGM", string seName = "SE", string voiceName = "Voice",
            string masterVolumeName = "MasterVolume",
            string bgmVolumeName = "BGMVolume",
            string seVolumeName = "SEVolume",
            string voiceVolumeName = "VoiceVolume")
            => this._core.SetOutput(audioMixer, bgmName, seName, voiceName, masterVolumeName, bgmVolumeName, seVolumeName, voiceVolumeName);

        public void SetMixerMasterVolume(float linearValue) => this._core.SetMixerMasterVolume(linearValue);
        public void SetMixerBgmVolume(float linearValue) => this._core.SetMixerBgmVolume(linearValue);
        public void SetMixerSeVolume(float linearValue) => this._core.SetMixerSeVolume(linearValue);
        public void SetMixerVoiceVolume(float linearValue) => this._core.SetMixerVoiceVolume(linearValue);

        public void ChangeBgm(string name, bool isLoop, float volume, float prevFadeOutSeconds, float fadeInSeconds)
        {
            var clip = this._assetLoadProxy.Load<AudioClip>(name);
            var (loopStart, loopLength) = AudioInfo.GetLoopInfo(name, this._audioInfoDict);
            this._core.ChangeBgm(clip, isLoop, volume, prevFadeOutSeconds, fadeInSeconds, loopStart, loopLength);
        }

        public void StopBgm(float fadeOutSeconds) => this._core.StopBgm(fadeOutSeconds);

        public void PlaySe(string name, bool isLoop, float volume)
        {
            var clip = this._assetLoadProxy.Load<AudioClip>(name);
            var (loopStart, loopLength) = AudioInfo.GetLoopInfo(name, this._audioInfoDict);
            this._core.PlaySe(clip, isLoop, volume, loopStart, loopLength);
        }

        public void StopSe(float fadeOutSeconds, string? name = null)
        {
            var clip = name != null ? this._assetLoadProxy.Load<AudioClip>(name) : null;
            this._core.StopSe(fadeOutSeconds, clip);
        }

        public void PlayVoice(string name, bool isLoop, float volume)
        {
            var clip = this._assetLoadProxy.Load<AudioClip>(name);
            var (loopStart, loopLength) = AudioInfo.GetLoopInfo(name, this._audioInfoDict);
            this._core.PlayVoice(clip, isLoop, volume, loopStart, loopLength);
        }

        public void StopVoice(float fadeOutSeconds, string? name = null)
        {
            var clip = name != null ? this._assetLoadProxy.Load<AudioClip>(name) : null;
            this._core.StopVoice(fadeOutSeconds, clip);
        }
    }
}
