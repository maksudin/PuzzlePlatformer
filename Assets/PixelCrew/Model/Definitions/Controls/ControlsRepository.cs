using System;
using System.Linq;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace Assets.PixelCrew.Model.Definitions.Controls
{
    [CreateAssetMenu(menuName = "Defs/Repository/Controls", fileName = "Controls")]
    public class ControlsRepository : DefRepository<ControlsMappingsDef>
    {
        [SerializeField] private ControlsMappingsDef[] _controls;
        public ControlsMappingsDef[] Controls => _controls;

        public ControlsMappingsDef GetControl(string id) => _controls.FirstOrDefault(x => x.Id == id);
    }

    [Serializable]
    public struct ControlsMappingsDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private XboxGamepadButton _xboxGamepadButton;
        [SerializeField] private KeyCode _keyboardKey;

        public string Id => _id;
        public XboxGamepadButton XboxGamepadButton => _xboxGamepadButton;
        public KeyCode KeyboardKey => _keyboardKey;
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