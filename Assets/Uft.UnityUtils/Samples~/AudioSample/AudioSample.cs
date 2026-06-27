#nullable enable

using System.Collections.Generic;
using Uft.UnityUtils.Asset;
using Uft.UnityUtils.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.AudioSample
{
    public class AudioSample : MonoBehaviour
    {
        [SerializeField] AudioMixer _audioMixer = null!;
        [SerializeField] Slider _sldMaster = null!;
        [SerializeField] Slider _sldBGM = null!;
        [SerializeField] Slider _sldSE = null!;
        [SerializeField] Slider _sldVoice = null!;

        [Header("SoundManager")]
        [SerializeField] SoundManager _soundManager = null!;
        [SerializeField] AudioClip _audioBGM1 = null!;
        [SerializeField] AudioClip _audioSE1 = null!;
        [SerializeField] AudioClip _audioVoice1 = null!;
        [SerializeField] AudioClip _audioBGM2 = null!;
        [SerializeField] AudioClip _audioSE2 = null!;
        [SerializeField] AudioClip _audioVoice2 = null!;
        [SerializeField] Button _btnBGM1 = null!;
        [SerializeField] Button _btnSE1 = null!;
        [SerializeField] Button _btnVoice1 = null!;
        [SerializeField] Button _btnBGM2 = null!;
        [SerializeField] Button _btnSE2 = null!;
        [SerializeField] Button _btnVoice2 = null!;

        void Start()
        {
            var dict = new Dictionary<string, AudioInfo>()
            {
                { "BGM1",   new AudioInfo() { name = "BGM1",   loopStart = -1, loopLength = -1 } },
                { "BGM2",   new AudioInfo() { name = "BGM2",   loopStart =  97231, loopLength = 196923 } }, // 約1秒+2/4拍子1小節後、60/117BPM * 48k * 2拍 * 4小節
                { "SE1",    new AudioInfo() { name = "SE1",    loopStart = -1, loopLength = -1 } },
                { "SE2",    new AudioInfo() { name = "SE2",    loopStart = -1, loopLength = -1 } },
                { "Voice1", new AudioInfo() { name = "Voice1", loopStart = -1, loopLength = -1 } },
                { "Voice2", new AudioInfo() { name = "Voice2", loopStart = -1, loopLength = -1 } }
            };
            var assetLoadProxy = new AssetLoadProxy((name, type) =>
            {
                return name switch
                {
                    "BGM1" => this._audioBGM1,
                    "BGM2" => this._audioBGM2,
                    "SE1"  => this._audioSE1,
                    "SE2"  => this._audioSE2,
                    "Voice1" => this._audioVoice1,
                    "Voice2" => this._audioVoice2,
                    _ => null
                };
            });
            this._soundManager.Setup(dict, assetLoadProxy);
            this._soundManager.SetOutput(this._audioMixer, "BGM", "SE", "Voice", "MasterVolume", "BGMVolume", "SEVolume", "VoiceVolume");

            // 1
            this._btnBGM1.onClick.AddListener(() => this._soundManager.ChangeBgm("BGM1", true, 1, 3.0f, 3.0f));
            this._btnSE1.onClick.AddListener(() => this._soundManager.PlaySe("SE1", false, 1));
            this._btnVoice1.onClick.AddListener(() => this._soundManager.PlayVoice("Voice1", false, 1));

            // 2
            this._btnBGM2.onClick.AddListener(() => this._soundManager.ChangeBgm("BGM2", true, 1, 3.0f, 3.0f));
            this._btnSE2.onClick.AddListener(() => this._soundManager.PlaySe("SE2", false, 1));
            this._btnVoice2.onClick.AddListener(() => this._soundManager.PlayVoice("Voice2", false, 1));
        }

        void Update()
        {
            this._soundManager.SetMixerMasterVolume(this._sldMaster.value);
            this._soundManager.SetMixerBgmVolume(this._sldBGM.value);
            this._soundManager.SetMixerSeVolume(this._sldSE.value);
            this._soundManager.SetMixerVoiceVolume(this._sldVoice.value);
        }
    }
}
