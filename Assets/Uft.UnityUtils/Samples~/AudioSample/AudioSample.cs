using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.AudioSample
{
    public class AudioSample : MonoBehaviour
    {
        [SerializeField] AudioMixer _audioMixer;
        [SerializeField] AudioSource _audioBGM;
        [SerializeField] AudioSource _audioSE;
        [SerializeField] AudioSource _audioVoice;

        [SerializeField] Slider _sldMaster;
        [SerializeField] Slider _sldBGM;
        [SerializeField] Slider _sldSE;
        [SerializeField] Slider _sldVoice;

        private void Start()
        {
            _audioBGM.Play();
            _audioSE.Play();
            _audioSE.Play();
        }

        void Update()
        {
            this.SetVolume("MasterVolume", this._sldMaster.value);
            this.SetVolume("BGMVolume", this._sldBGM.value);
            this.SetVolume("SEVolume", this._sldSE.value);
            this.SetVolume("VoiceVolume", this._sldVoice.value);
        }

        void SetVolume(string name, float linearValue)
        {
            var dB = AudioUtil.LinearToDecibel(linearValue);
            this._audioMixer.SetFloat(name, dB);
        }
    }
}
