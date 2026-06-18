using Uft.UnityUtils.UI;
using UnityEngine;

namespace Uft.UnityUtils.Samples.UISample
{
    public class RtCursorUtilSample : MonoBehaviour
    {
        [SerializeField] MenuItemSample[] _menuItems;

        MenuItemSample _lastSelectedItem;

        void Start()
        {
            RtCursorUtil.UpdateCursorTo(this._menuItems, null, this._menuItems[0]);
        }

        void Update()
        {
            // NOTE: InputSystem 活用
            this._lastSelectedItem = RtCursorUtil.UpdateCursor(
                this._menuItems,
                this._lastSelectedItem,
                (item) => item.Button,
                (item) => item.Cursor);
        }
    }
}
