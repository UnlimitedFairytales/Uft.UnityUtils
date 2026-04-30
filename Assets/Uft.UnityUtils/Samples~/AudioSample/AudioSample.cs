using Uft.UnityUtils.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.AudioSample
{
    public class AudioSample : MonoBehaviour
    {
        [SerializeField] AudioMixer _audioMixer;
        [SerializeField] Slider _sldMaster;
        [SerializeField] Slider _sldBGM;
        [SerializeField] Slider _sldSE;
        [SerializeField] Slider _sldVoice;

        [Header("SoundManager")]
        [SerializeField] SoundManager _soundManager;
        [SerializeField] AudioClip _audioBGM1;
        [SerializeField] AudioClip _audioSE1;
        [SerializeField] AudioClip _audioVoice1;
        [SerializeField] AudioClip _audioBGM2;
        [SerializeField] AudioClip _audioSE2;
        [SerializeField] AudioClip _audioVoice2;
        [SerializeField] Button _btnBGM1;
        [SerializeField] Button _btnSE1;
        [SerializeField] Button _btnVoice1;
        [SerializeField] Button _btnBGM2;
        [SerializeField] Button _btnSE2;
        [SerializeField] Button _btnVoice2;

        void Start()
        {
            this._soundManager.SetOutput(this._audioMixer, "BGM", "SE", "Voice", "MasterVolume", "BGMVolume", "SEVolume", "VoiceVolume");

            // 1
            this._btnBGM1.onClick.AddListener(() => this._soundManager.ChangeBgm(this._audioBGM1, true, 1, 3.0f, 3.0f));
            this._btnSE1.onClick.AddListener(() => this._soundManager.PlaySe(this._audioSE1, false, 1));
            this._btnVoice1.onClick.AddListener(() => this._soundManager.PlayVoice(this._audioVoice1, false, 1));

            // 2
            this._btnBGM2.onClick.AddListener(() => this._soundManager.ChangeBgm(this._audioBGM2, true, 1, 3.0f, 3.0f));
            this._btnSE2.onClick.AddListener(() => this._soundManager.PlaySe(this._audioSE2, false, 1));
            this._btnVoice2.onClick.AddListener(() => this._soundManager.PlayVoice(this._audioVoice2, false, 1));
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
