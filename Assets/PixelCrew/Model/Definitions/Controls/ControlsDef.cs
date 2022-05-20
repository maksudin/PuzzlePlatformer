using System;
using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions.Controls
{
    [CreateAssetMenu(menuName = "Defs/ControlsDef", fileName = "ControlsDef")]
    public class ControlsDef : ScriptableObject
    {
        [SerializeField] private GamepadIcons[] _xboxIcons;
        public GamepadIcons[] XboxIcons => _xboxIcons;
    }

    [Serializable]
    public struct GamepadIcons
    {
        [SerializeField] private XboxGamepadButton _xboxGamepadButton;
        [SerializeField] private Sprite _xboxGamepadIcon;

        public XboxGamepadButton XboxGamepadButton => _xboxGamepadButton;
        public Sprite XboxGamepadIcon => _xboxGamepadIcon;
    }
}