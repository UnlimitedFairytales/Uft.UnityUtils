#nullable enable

using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Uft.UnityUtils.Audio
{
    public class SoundManager : MonoBehaviour
    {
        static void PlayAudioInner(AudioClip clip, bool isLoop, float volume, AudioSource[] audioList, ref int lastIndex)
        {
            lastIndex = lastIndex < audioList.Length - 1 ? lastIndex + 1 : 0;
            int i = lastIndex;

            // NOTE: 途中状態は考慮しなくていいように
            audioList[i].DOComplete();

            audioList[i].clip = clip;
            audioList[i].loop = isLoop;
            audioList[i].time = 0;
            audioList[i].volume = volume;
            audioList[i].Play();
        }

        static void StopAudioInner(float fadeOutSeconds, AudioClip? clip, AudioSource[] audioList)
        {
            var ease = Ease.Linear;
            for (int i = 0; i < audioList.Length; i++)
            {
                var capturedIndex = i;
                if (clip == null || audioList[capturedIndex].clip == clip)
                {
                    // NOTE: 途中状態は考慮しなくていいように
                    audioList[capturedIndex].DOComplete();

                    audioList[capturedIndex].DOFade(0, fadeOutSeconds)
                        .SetEase(ease)
                        .OnComplete(() => audioList[capturedIndex].Stop());
                }
            }
        }

        // Parameters

        [SerializeField] AudioMixer _audioMixer; public AudioMixer AudioMixer => this._audioMixer;
        [SerializeField] string _masterVolumeName = "MasterVolume";
        [SerializeField] string _bgmVolumeName = "BGMVolume";
        [SerializeField] string _seVolumeName = "SEVolume";
        [SerializeField] string _voiceVolumeName = "VoiceVolume";
        [SerializeField] AudioSource _audioBgm1;
        [SerializeField] AudioSource _audioBgm2;
        [SerializeField] AudioSource[] _audioSeList; // NOTE: 8つ想定
        [SerializeField] AudioSource[] _audioVoiceList; // NOTE: 8つ想定

        /// <summary>フェード中は新しい方をcurrentと見なす</summary>
        bool _currentBgmIsBgm1 = false;

        int _lastSeIndex = -1;
        int _lastVoiceIndex = -1;

        public bool IsAnyVoicePlaying
        {
            get
            {
                foreach (var audioVoice in this._audioVoiceList)
                {
                    if (audioVoice.isPlaying) return true;
                }
                return false;
            }
        }

        public void SetOutput(AudioMixer? audioMixer = null, string bgmName = "BGM", string seName = "SE", string voiceName = "Voice",
            string masterVolumeName = "MasterVolume",
            string bgmVolumeName = "BGMVolume",
            string seVolumeName = "SEVolume",
            string voiceVolumeName = "VoiceVolume")
        {
            if (audioMixer != null)
            {
                this._audioMixer = audioMixer;
            }
            var mixer = this._audioMixer;
            this._audioBgm1.outputAudioMixerGroup = mixer.FindMatchingGroups(bgmName)[0];
            this._audioBgm2.outputAudioMixerGroup = mixer.FindMatchingGroups(bgmName)[0];
            foreach (var audioSe in this._audioSeList)
            {
                audioSe.outputAudioMixerGroup = mixer.FindMatchingGroups(seName)[0];
            }
            foreach (var audioVoice in this._audioVoiceList)
            {
                audioVoice.outputAudioMixerGroup = mixer.FindMatchingGroups(voiceName)[0];
            }
            this._masterVolumeName = masterVolumeName;
            this._bgmVolumeName = bgmVolumeName;
            this._seVolumeName = seVolumeName;
            this._voiceVolumeName = voiceVolumeName;
        }

        public void SetMixerMasterVolume(float linearValue) => this.SetMixerVolume(this._masterVolumeName, linearValue);
        public void SetMixerBgmVolume(float linearValue) => this.SetMixerVolume(this._bgmVolumeName, linearValue);
        public void SetMixerSeVolume(float linearValue) => this.SetMixerVolume(this._seVolumeName, linearValue);
        public void SetMixerVoiceVolume(float linearValue) => this.SetMixerVolume(this._voiceVolumeName, linearValue);
        void SetMixerVolume(string name, float linearValue)
        {
            var dB = AudioUtil.LinearToDecibel(linearValue);
            this._audioMixer.SetFloat(name, dB);
        }

        public void ChangeBgm(AudioClip clip, bool isLoop, float volume, float prevFadeOutSeconds, float fadeInSeconds)
        {
            // NOTE: 途中状態は考慮しなくていいように
            this._audioBgm1.DOComplete();
            this._audioBgm2.DOComplete();

            var ease = Ease.Linear;
            var delay = fadeInSeconds == 0.0f ? 0 : (prevFadeOutSeconds / 2.0f);

            // NOTE: ping-pong で次の再生を設定後、フラグを更新
            if (this._currentBgmIsBgm1)
            {
                this._audioBgm2.clip = clip;
                this._audioBgm2.loop = isLoop;
                this._audioBgm2.time = 0;
                if (0.0f == fadeInSeconds)
                {
                    this._audioBgm2.volume = volume;
                    this._audioBgm2.Play();
                }
                else
                {
                    this._audioBgm2.volume = 0;
                    this._audioBgm2.Play();
                    this._audioBgm2.DOFade(volume, fadeInSeconds).SetEase(ease).SetDelay(delay);
                }
                this._audioBgm1.DOFade(0, prevFadeOutSeconds).SetEase(ease)
                    .OnComplete(() => this._audioBgm1.Stop());
            }
            else
            {
                this._audioBgm1.clip = clip;
                this._audioBgm1.loop = isLoop;
                this._audioBgm1.time = 0;
                if (0.0f == fadeInSeconds)
                {
                    this._audioBgm1.volume = volume;
                    this._audioBgm1.Play();
                }
                else
                {
                    this._audioBgm1.volume = 0;
                    this._audioBgm1.Play();
                    this._audioBgm1.DOFade(volume, fadeInSeconds).SetEase(ease).SetDelay(delay);
                }
                this._audioBgm2.DOFade(0, prevFadeOutSeconds).SetEase(ease)
                    .OnComplete(() => this._audioBgm2.Stop());
            }
            this._currentBgmIsBgm1 = !this._currentBgmIsBgm1;
        }

        public void StopBgm(float fadeOutSeconds)
        {
            // NOTE: 途中状態は考慮しなくていいように
            this._audioBgm1.DOComplete();
            this._audioBgm2.DOComplete();

            var ease = Ease.Linear;
            if (this._audioBgm1.isPlaying)
            {
                this._audioBgm1.DOFade(0, fadeOutSeconds)
                    .SetEase(ease)
                    .OnComplete(() => this._audioBgm1.Stop());
            }
            if (this._audioBgm2.isPlaying)
            {
                this._audioBgm2.DOFade(0, fadeOutSeconds)
                    .SetEase(ease)
                    .OnComplete(() => this._audioBgm2.Stop());
            }
        }

        public void PlaySe(AudioClip clip, bool isLoop, float volume)
        {
            if (this._audioSeList.Length == 0) throw new InvalidOperationException($"{nameof(this._audioSeList)} is empty.");

            PlayAudioInner(clip, isLoop, volume, this._audioSeList, ref this._lastSeIndex);
        }

        public void StopSe(float fadeOutSeconds, AudioClip? clip = null) => StopAudioInner(fadeOutSeconds, clip, this._audioSeList);

        public void PlayVoice(AudioClip clip, bool isLoop, float volume)
        {
            if (this._audioVoiceList.Length == 0) throw new InvalidOperationException($"{nameof(this._audioVoiceList)} is empty.");

            PlayAudioInner(clip, isLoop, volume, this._audioVoiceList, ref this._lastVoiceIndex);
        }

        public void StopVoice(float fadeOutSeconds, AudioClip? clip = null) => StopAudioInner(fadeOutSeconds, clip, this._audioVoiceList);
    }
}
