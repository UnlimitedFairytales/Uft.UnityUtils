#nullable enable

using UnityEngine.InputSystem;

namespace Uft.UnityUtils
{
    public enum InputState
    {
        Holding,
        Triggered,
        Released
    }

    public interface IInputProxy
    {
        void Poll();
        bool GetLeft(InputState state);
        bool GetRight(InputState state);
        bool GetUp(InputState state);
        bool GetDown(InputState state);

        bool GetConfirm(InputState state);
        bool GetCancel(InputState state);

        bool GetAction(int action, InputState state);
    }

    public class SimpleInput : IInputProxy
    {
        static bool GetInner(InputState state, UnityEngine.InputSystem.Controls.KeyControl key)
        {
            return state switch
            {
                InputState.Holding => key.isPressed,
                InputState.Triggered => key.wasPressedThisFrame,
                InputState.Released => key.wasReleasedThisFrame,
                _ => false
            };
        }

        public void Poll() { }
        public bool GetLeft(InputState state) => GetInner(state, Keyboard.current.leftArrowKey);
        public bool GetRight(InputState state) => GetInner(state, Keyboard.current.rightArrowKey);
        public bool GetUp(InputState state) => GetInner(state, Keyboard.current.upArrowKey);
        public bool GetDown(InputState state) => GetInner(state, Keyboard.current.downArrowKey);
        public bool GetConfirm(InputState state) => GetInner(state, Keyboard.current.zKey);
        public bool GetCancel(InputState state) => GetInner(state, Keyboard.current.xKey);
        public bool GetAction(int action, InputState state) => false;
    }
}
