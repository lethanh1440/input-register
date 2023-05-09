/*------------cont.lethanh@gmail.com---------------*/
using System.Collections.Generic;
using UnityEngine;
namespace RegInputEvent
{
    public class InputRegister : MonoBehaviour
    {
        [SerializeField] bool useLeftMouse = true;
        [SerializeField] bool useRightMouse;
        [SerializeField] bool useMiddleMouse;
        MouseTrack[] mouseTracks;
        private void Start()
        {
            var _mouseTracks = new List<MouseTrack>();
            if (useLeftMouse)
            {
                var left = new MouseTrack(MouseCode.Left);
                left.action = OnLeftMouse;
                _mouseTracks.Add(left);
            }
            if (useRightMouse)
            {
                var right = new MouseTrack(MouseCode.Right);
                right.action = OnRightMouse;
                _mouseTracks.Add(right);
            }
            if (useMiddleMouse) { _mouseTracks.Add(new MouseTrack(MouseCode.Mid)); }
            mouseTracks = _mouseTracks.ToArray();
        }
        private void OnLeftMouse(MousePressMode mousePressMode, Vector3 mousePos)
        {
            for (int i = 0; i < InputEvent.mouses.Count; i++)
            {
                if (InputEvent.mouses[i].mousePressMode == mousePressMode &&
                    InputEvent.mouses[i].mouseCode == MouseCode.Left)
                {
                    InputEvent.mouses[i].action(mousePos);
                }
            }
        }
        private void OnRightMouse(MousePressMode mousePressMode, Vector3 mousePos)
        {
            for (int i = 0; i < InputEvent.mouses.Count; i++)
            {
                if (InputEvent.mouses[i].mousePressMode == mousePressMode &&
                    InputEvent.mouses[i].mouseCode == MouseCode.Right)
                {
                    InputEvent.mouses[i].action(mousePos);
                }
            }
        }
        private void Update()
        {
            //keyboard
            for (int i = 0; i < InputEvent.keys.Count; i++)
            {
                if (Input.GetKeyDown(InputEvent.keys[i].keyCode)
                    && InputEvent.keys[i].keyMode == KeyPressMode.Down)
                {
                    InputEvent.keys[i].action?.Invoke();
                }
                else
                {
                    if (Input.GetKeyUp(InputEvent.keys[i].keyCode)
                    && InputEvent.keys[i].keyMode == KeyPressMode.Up)
                    {
                        InputEvent.keys[i].action?.Invoke();
                    }
                    else
                    {
                        if (Input.GetKey(InputEvent.keys[i].keyCode)
                                        && InputEvent.keys[i].keyMode == KeyPressMode.Hold)
                        {
                            InputEvent.keys[i].action?.Invoke();
                        }
                    }
                }
            }
            //mouse
            for (int i = 0; i < mouseTracks.Length; i++)
            {
                mouseTracks[i].Update();
            }
        }
    }
    public static class InputEvent
    {
        public static List<Key> keys = new List<Key>();
        public static List<Mouse> mouses = new List<Mouse>();
        public static void OnInput(KeyCode keyCode, KeyPressMode keyMode, System.Action action)
        {
            keys.Add(new Key()
            {
                action = action,
                keyCode = keyCode,
                keyMode = keyMode
            });
        }

    }
    public enum KeyPressMode
    {
        Down,
        Hold,
        Up
    };
    public enum MousePressMode
    {
        Down,
        HoldNotDrag,
        UpNotDrag,
        BeginDrag,
        Drag,
        EndDrag,
        UpEndDrag,
        UpMove,
    }
    public enum MouseCode
    {
        Left = 0, Mid = 2, Right = 1
    }
    public class MouseTrack
    {
        private int mouseCode;
        private bool isDown;
        private bool isDrag;
        private Vector3 mouseDownPos;
        private Vector3 mousePos;
        const float dragThreshold = 5f;
        public System.Action<MousePressMode, Vector3> action;
        public MouseTrack(MouseCode mouseCode)
        {
            this.mouseCode = (int)mouseCode;
        }
        private bool UIOver()
        {
#if UNITY_EDITOR
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
#else
        if (Input.touchCount > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            return true;
#endif
            return false;
        }
        public void Update()
        {
            if (Input.GetMouseButton(mouseCode))
            {
                if (!isDown && !UIOver())
                {
                    mouseDownPos = Input.mousePosition;
                    isDown = true;
                    action?.Invoke(MousePressMode.Down, Input.mousePosition);
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(mouseCode))
                {
                    if (isDrag)
                    {
                        action?.Invoke(MousePressMode.UpEndDrag, Input.mousePosition);
                    }
                    else
                    {
                        action?.Invoke(MousePressMode.UpNotDrag, Input.mousePosition);
                    }
                    isDrag = false;
                    isDown = false;
                }
            }
            if (isDown)
            {
                if (isDrag)
                {
                    action?.Invoke(MousePressMode.Drag, Input.mousePosition);
                }
                else
                {
                    if (Vector3.Distance(mouseDownPos, Input.mousePosition) > dragThreshold)
                    {
                        action?.Invoke(MousePressMode.BeginDrag, Input.mousePosition);
                        isDrag = true;
                    }
                }
            }
            else
            {
                if (action != null && Vector3.Distance(mousePos, Input.mousePosition) > 1f)
                {
                    action.Invoke(MousePressMode.UpMove, Input.mousePosition);
                }
            }
            mousePos = Input.mousePosition;
        }
    }
    public class Mouse
    {
        public MouseCode mouseCode;
        public MousePressMode mousePressMode;
        public System.Action<Vector3> action;
    }
    public class Key
    {
        public System.Action action;
        public KeyCode keyCode;
        public KeyPressMode keyMode;
    }
}