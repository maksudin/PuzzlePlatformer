using System;
using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions.Controls
{
    [CreateAssetMenu(menuName = "Defs/ControlsDef", fileName = "ControlsDef")]
    public class ControlsDef : ScriptableObject
    {
        [SerializeField] private GamepadIcons[] _xboxIcons;
        [SerializeField] private KeyboardIcons[] _keyboardIcons;

        public GamepadIcons[] XboxIcons => _xboxIcons;
        public KeyboardIcons[] KeyboardIcons => _keyboardIcons;
    }

    [Serializable]
    public struct GamepadIcons
    {
        [SerializeField] private XboxGamepadButton _xboxGamepadButton;
        [SerializeField] private Sprite _xboxGamepadIcon;

        public XboxGamepadButton XboxGamepadButton => _xboxGamepadButton;
        public Sprite XboxGamepadIcon => _xboxGamepadIcon;
    }

    [Serializable]
    public struct KeyboardIcons
    {
        [SerializeField] private KeyCode _keyboardButton;
        [SerializeField] private Sprite _keyboardIcon;

        public KeyCode KeyboardButton => _keyboardButton;
        public Sprite KeyboardIcon => _keyboardIcon;
    }

    public enum XboxGamepadButton
    {
        A, B, X, Y,
        LB, RB, LT, RT,
        Back, Start,
        DPADLeft, DPADRight, DPADUp, DPADDown,
        LSBLeft, LSBRight, LSBUp, LSBDown
    }
}