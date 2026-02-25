#nullable enable

using UnityEngine;
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
        float MinRequiredAngle_deg { get; set; }
        Vector2 Move { get; }
        bool Left(InputState state);
        bool Right(InputState state);
        bool Down(InputState state);
        bool Up(InputState state);
        bool Submit(InputState state);
        bool Cancel(InputState state);
        bool Button(string actionName, InputState state);

        void Update();
        string DebugString(params string[] additonalActionNameOrIds);
    }

    public class SimpleInput : IInputProxy
    {
        static bool GetResult(bool current, bool prev, InputState state)
        {
            return state switch
            {
                InputState.Holding => current,
                InputState.Triggered => !prev && current,
                InputState.Released => prev && !current,
                _ => false,
            };
        }

        SimpleInputValue prev = default;
        SimpleInputValue current = default;

        public float MinRequiredAngle_deg { get; set; } = 22.5f;
        public Vector2 Move => this.current.move;
        public bool Left(InputState state) => GetResult(this.current.left, this.prev.left, state);
        public bool Right(InputState state) => GetResult(this.current.right, this.prev.right, state);
        public bool Down(InputState state) => GetResult(this.current.down, this.prev.down, state);
        public bool Up(InputState state) => GetResult(this.current.up, this.prev.up, state);
        public bool Submit(InputState state) => GetResult(this.current.submit, this.prev.submit, state);
        public bool Cancel(InputState state) => GetResult(this.current.cancel, this.prev.cancel, state);
        public bool Button(string actionNameOrId, InputState state)
        {
            var action = InputSystem.actions != null ? InputSystem.actions.FindAction(actionNameOrId) : null;
            if (action == null) return false;

            return state switch
            {
                InputState.Holding => action.IsPressed(),
                InputState.Triggered => action.WasPressedThisFrame(),
                InputState.Released => action.WasReleasedThisFrame(),
                _ => false
            };
        }

        public void Update()
        {
            if (InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsManually)
            {
                InputSystem.Update();
            }
            this.prev = this.current;
            this.current = InputSystem.actions != null ?
                new SimpleInputValue(InputSystem.actions, Mathf.Tan(this.MinRequiredAngle_deg * Mathf.Deg2Rad)) :
                default;
        }

        public virtual string DebugString(params string[] additonalActionNameOrIds)
        {
            static string DebugStringInner(string name, bool h, bool t, bool r)
            {
                if (!h && !r) return "";
                return name + (t ? "+" : "") + (r ? "-" : "");
            }

            var l = DebugStringInner("l", this.Left(InputState.Holding), this.Left(InputState.Triggered), this.Left(InputState.Released));
            var r = DebugStringInner("r", this.Right(InputState.Holding), this.Right(InputState.Triggered), this.Right(InputState.Released));
            var d = DebugStringInner("d", this.Down(InputState.Holding), this.Down(InputState.Triggered), this.Down(InputState.Released));
            var u = DebugStringInner("u", this.Up(InputState.Holding), this.Up(InputState.Triggered), this.Up(InputState.Released));
            var submit = DebugStringInner("Submit", this.Submit(InputState.Holding), this.Submit(InputState.Triggered), this.Submit(InputState.Released));
            var cancel = DebugStringInner("Cancel", this.Cancel(InputState.Holding), this.Cancel(InputState.Triggered), this.Cancel(InputState.Released));

            var add = "";
            foreach (var actionNameOrId in additonalActionNameOrIds)
            {
                if (InputSystem.actions != null)
                {
                    var action = InputSystem.actions.FindAction(actionNameOrId);
                    if (action != null && action.IsPressed())
                    {
                        add += $"{action.name}{(action.WasPressedThisFrame() ? "+" : "")}{(action.WasReleasedThisFrame() ? "-" : "")} ";
                    }
                }
            }

            return
                $"Move={this.Move}\n" +
                $"{l} {r} {d} {u} {submit} {cancel} {add}";
        }
    }

    public readonly struct SimpleInputValue
    {
        public readonly Vector2 move;
        public readonly bool left;
        public readonly bool right;
        public readonly bool down;
        public readonly bool up;

        public readonly bool submit;
        public readonly bool cancel;

        public SimpleInputValue(InputActionAsset actions, float minRequiredTan)
        {
            var moveAction = actions.FindAction("Move");
            if (moveAction == null)
            {
                this.move = Vector2.zero;
                this.left = false;
                this.right = false;
                this.down = false;
                this.up = false;
            }
            else
            {
                this.move = moveAction.ReadValue<Vector2>();

                // NOTE: 絶対値が小さい方の軸は、角度が浅い場合は無効化する（斜め入力になりにくくする）
                var x = this.move.x;
                var y = this.move.y;
                var absX = Mathf.Abs(x);
                var absY = Mathf.Abs(y);
                var isXDominant = absX >= absY;
                var xMinReq = isXDominant ? 0f : absY * minRequiredTan;
                var yMinReq = isXDominant ? absX * minRequiredTan : 0f;

                var isP = moveAction.IsPressed();
                this.left = isP && x <= -xMinReq;
                this.right = isP && x >= +xMinReq;
                this.down = isP && y <= -yMinReq;
                this.up = isP && y >= +yMinReq;
            }
            var submitAction = actions.FindAction("Submit");
            this.submit = submitAction != null && submitAction.IsPressed();
            var cancelAction = actions.FindAction("Cancel");
            this.cancel = cancelAction != null && cancelAction.IsPressed();
        }
    }
}
