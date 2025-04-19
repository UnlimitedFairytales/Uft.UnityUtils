using Uft.UnityUtils.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uft.UnityUtils.Samples.UISample
{
    public class TMPUtilSample : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] TMP_Text _tmpText;
        [SerializeField] ScrollRect _scrollRect;

        void Start()
        {
            _button.onClick.AddListener(UniTask.UnityAction(async () =>
            {
                _tmpText.text = "";
                await _tmpText.TypeWriterEffectAsync(@"O, that this too too solid flesh would melt,
Thaw, and resolve itself into a dew!
Or that the Everlasting had not fix'd
His canon 'gainst self-slaughter! O God! God!
How weary, stale, flat, and profitable
Seem to me all the uses of this world!", TMPUtil.ONE_FRAME * 2, default, _scrollRect);
            }));
        }
    }
}