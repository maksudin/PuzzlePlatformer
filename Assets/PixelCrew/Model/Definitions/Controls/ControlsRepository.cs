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
        public ControlsMappingsDef[] Controls => Collection;
        public ControlsMappingsDef GetControl(string id) => Controls.FirstOrDefault(x => x.Id == id);

        public void ReplaceKey(string id, KeyCode newKey)
        {
            for (var i = 0; i < Collection.Length; i++)
            {
                if (Collection[i].Id == id)
                    Collection[i].KeyboardKey = newKey;
            }
        }
    }

    [Serializable]
    public struct ControlsMappingsDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private XboxGamepadButton _xboxGamepadButton;
        [SerializeField] private KeyCode _keyboardKey;

        public string Id => _id;
        public XboxGamepadButton XboxGamepadButton => _xboxGamepadButton;
        public KeyCode KeyboardKey { get => _keyboardKey; set => _keyboardKey = value; }
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