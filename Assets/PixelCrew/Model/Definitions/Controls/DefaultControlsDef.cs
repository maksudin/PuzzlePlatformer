using System;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions.Controls
{
    [CreateAssetMenu(menuName = "Defs/DefaultControls", fileName = "DefaultControls")]
    public class DefaultControlsDef : ScriptableObject
    {
        [SerializeField] private DefaultMappingsDef[] _defaultControls;
        public DefaultMappingsDef[] DefaultControls => _defaultControls;
    }

    [Serializable]
    public struct DefaultMappingsDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private XboxGamepadButton _xboxGamepadButton;
        [SerializeField] private KeyCode _keyboardKey;

        public string Id => _id;
        public XboxGamepadButton XboxGamepadButton => _xboxGamepadButton;
        public KeyCode KeyboardKey { get => _keyboardKey; set => _keyboardKey = value; }
    }
}