#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Uft.UnityUtils.Audio
{
    [CreateAssetMenu(fileName = "AudioInfoTable", menuName = "Uft.UnityUtils/Create AudioInfoTable")]
    public class AudioInfoTable : ScriptableObject
    {
        [SerializeField] AudioInfo[] _audioInfos = {};

        public Dictionary<string, AudioInfo> ToDictionary()
        {
            var dict = new Dictionary<string, AudioInfo>();
            for (int i = 0; i < this._audioInfos.Length; i++)
            {
                var info = this._audioInfos[i];
                dict[info.name] = info;
            }
            return dict;
        }
    }
}
