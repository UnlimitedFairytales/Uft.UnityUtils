#nullable enable

using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;

namespace Uft.UnityUtils.Audio
{
    public class SoundManagerCore : MonoBehaviour
    {
        // Static members ======================================================

        static readonly DevLogWithTag DevLog = new("[" + nameof(SoundManagerCore) + "]");

        static bool IsLoopSectionEnabled(int loopStart, int loopLength) => 0 <= loopStart && 0 < loopLength;

        /// <summary>sourceに対するループ監視タスクが動いていればキャンセルする</summary>
        static void CancelLoopWatch(AudioSource source, Dictionary<AudioSource, CancellationTokenSource> loopCtsBySource)
        {
            if (loopCtsBySource.TryGetValue(source, out var oldCts))
            {
                oldCts.Cancel();
                oldCts.Dispose();
                loopCtsBySource.Remove(source);
            }
        }

        /// <summary>sourceに対する直前のループ監視タスクをキャンセルし、isLoopかつループ区間が有効な場合のみ新しい監視タスクを開始する</summary>
        static void SetLoopWatch(AudioSource source, bool isLoop, int loopStart, int loopLength, Dictionary<AudioSource, CancellationTokenSource> loopCtsBySource, CancellationToken destroyCancellationToken)
        {
            CancelLoopWatch(source, loopCtsBySource);

            if (!isLoop || !IsLoopSectionEnabled(loopStart, loopLength)) return;

            var cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            loopCtsBySource[source] = cts;
            WatchLoopAsync(source, loopStart, loopLength, cts.Token).Forget();
        }

        /// <summary>ループ終端後に2秒以上ループ開始部と重複がある音源を前提とした、簡易timeSamples巻き戻し式ループ</summary>
        static async UniTask WatchLoopAsync(AudioSource source, int loopStart, int loopLength, CancellationToken ct)
        {
            var loopEnd = loopStart + loopLength;
            while (true)
            {
                await UniTask.NextFrame(ct);
                if (source.isPlaying && loopEnd <= source.timeSamples) source.timeSamples -= loopLength;
            }
        }

        static void PlayAudioInner(AudioClip clip, bool isLoop, float volume, int loopStart, int loopLength, AudioSource[] audioList, ref int lastIndex, Dictionary<AudioSource, CancellationTokenSource> loopCtsBySource, CancellationToken destroyCancellationToken)
        {
            lastIndex = lastIndex < audioList.Length - 1 ? lastIndex + 1 : 0;
            int i = lastIndex;

            // NOTE: 途中状態は考慮しなくていいように
            audioList[i].DOComplete();

            // NOTE: ループ区間指定がある場合は手動でtimeSamplesを巻き戻すため、AudioSource側のloopは使わない
            audioList[i].clip = clip;
            audioList[i].loop = isLoop && !IsLoopSectionEnabled(loopStart, loopLength);
            audioList[i].time = 0;
            audioList[i].volume = volume;
            audioList[i].Play();

            SetLoopWatch(audioList[i], isLoop, loopStart, loopLength, loopCtsBySource, destroyCancellationToken);
        }

        static void StopAudioInner(float fadeOutSeconds, AudioClip? clip, AudioSource[] audioList, Dictionary<AudioSource, CancellationTokenSource> loopCtsBySource)
        {
            var ease = Ease.Linear;
            for (int i = 0; i < audioList.Length; i++)
            {
                var capturedIndex = i;
                if (clip == null || audioList[capturedIndex].clip == clip)
                {
                    // NOTE: 途中状態は考慮しなくていいように
                    audioList[capturedIndex].DOComplete();

                    CancelLoopWatch(audioList[capturedIndex], loopCtsBySource);

                    audioList[capturedIndex].DOFade(0, fadeOutSeconds)
                        .SetEase(ease)
                        .OnComplete(() => audioList[capturedIndex].Stop());
                }
            }
        }

        // Parameters ==========================================================

        [SerializeField] AudioMixer _audioMixer = null!; public AudioMixer AudioMixer => this._audioMixer;
        [SerializeField] string _masterVolumeName = "MasterVolume";
        [SerializeField] string _bgmVolumeName = "BGMVolume";
        [SerializeField] string _seVolumeName = "SEVolume";
        [SerializeField] string _voiceVolumeName = "VoiceVolume";
        [SerializeField] AudioSource _audioBgm1 = null!;
        [SerializeField] AudioSource _audioBgm2 = null!;
        [SerializeField] AudioSource[] _audioSeList = null!; // NOTE: 8つ想定
        [SerializeField] AudioSource[] _audioVoiceList = null!; // NOTE: 8つ想定

        // Status ==============================================================

        /// <summary>フェード中は新しい方をcurrentと見なす</summary>
        bool _currentBgmIsBgm1 = false;

        int _lastSeIndex = -1;
        int _lastVoiceIndex = -1;

        readonly Dictionary<AudioSource, CancellationTokenSource> _loopCtsBySource = new();

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

        // Methods =============================================================

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

        public void ChangeBgm(AudioClip clip, bool isLoop, float volume, float prevFadeOutSeconds, float fadeInSeconds, int loopStart = -1, int loopLength = -1)
        {
            // NOTE: 途中状態は考慮しなくていいように
            this._audioBgm1.DOComplete();
            this._audioBgm2.DOComplete();

            var ease = Ease.Linear;
            var delay = fadeInSeconds == 0.0f ? 0 : (prevFadeOutSeconds / 2.0f);

            // NOTE: ループ区間指定がある場合は手動でtimeSamplesを巻き戻すため、AudioSource側のloopは使わない
            var isNativeLoop = isLoop && !IsLoopSectionEnabled(loopStart, loopLength);

            // NOTE: ping-pong で次の再生を設定後、フラグを更新
            if (this._currentBgmIsBgm1)
            {
                this._audioBgm2.clip = clip;
                this._audioBgm2.loop = isNativeLoop;
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
                SetLoopWatch(this._audioBgm2, isLoop, loopStart, loopLength, this._loopCtsBySource, this.destroyCancellationToken);
                this._audioBgm1.DOFade(0, prevFadeOutSeconds).SetEase(ease)
                    .OnComplete(() =>
                    {
                        CancelLoopWatch(this._audioBgm1, this._loopCtsBySource);
                        this._audioBgm1.Stop();
                    });
            }
            else
            {
                this._audioBgm1.clip = clip;
                this._audioBgm1.loop = isNativeLoop;
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
                SetLoopWatch(this._audioBgm1, isLoop, loopStart, loopLength, this._loopCtsBySource, this.destroyCancellationToken);
                this._audioBgm2.DOFade(0, prevFadeOutSeconds).SetEase(ease)
                    .OnComplete(() =>
                    {
                        CancelLoopWatch(this._audioBgm2, this._loopCtsBySource);
                        this._audioBgm2.Stop();
                    });
            }
            this._currentBgmIsBgm1 = !this._currentBgmIsBgm1;
        }

        public void StopBgm(float fadeOutSeconds)
        {
            // NOTE: 途中状態は考慮しなくていいように
            this._audioBgm1.DOComplete();
            this._audioBgm2.DOComplete();

            CancelLoopWatch(this._audioBgm1, this._loopCtsBySource);
            CancelLoopWatch(this._audioBgm2, this._loopCtsBySource);

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

        public void PlaySe(AudioClip clip, bool isLoop, float volume, int loopStart = -1, int loopLength = -1)
        {
            if (this._audioSeList.Length == 0) throw new InvalidOperationException($"{nameof(this._audioSeList)} is empty.");

            PlayAudioInner(clip, isLoop, volume, loopStart, loopLength, this._audioSeList, ref this._lastSeIndex, this._loopCtsBySource, this.destroyCancellationToken);
        }

        public void StopSe(float fadeOutSeconds, AudioClip? clip = null) => StopAudioInner(fadeOutSeconds, clip, this._audioSeList, this._loopCtsBySource);

        public void PlayVoice(AudioClip clip, bool isLoop, float volume, int loopStart = -1, int loopLength = -1)
        {
            if (this._audioVoiceList.Length == 0) throw new InvalidOperationException($"{nameof(this._audioVoiceList)} is empty.");

            PlayAudioInner(clip, isLoop, volume, loopStart, loopLength, this._audioVoiceList, ref this._lastVoiceIndex, this._loopCtsBySource, this.destroyCancellationToken);
        }

        public void StopVoice(float fadeOutSeconds, AudioClip? clip = null) => StopAudioInner(fadeOutSeconds, clip, this._audioVoiceList, this._loopCtsBySource);
    }
}
