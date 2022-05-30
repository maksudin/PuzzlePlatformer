using System;
using System.Linq;
using Assets.PixelCrew.Model.Data.Properties;
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

        private static ControlsRepository _instance;
        public static ControlsRepository I => _instance == null ? LoadControls() : _instance;

        private static ControlsRepository LoadControls()
        {
            return _instance = Resources.Load<ControlsRepository>("Controls");
        }

        private void OnValidate()
        {
            foreach (var control in Controls)
                control.KeyboardKey.Validate();
        }

        private void OnEnable()
        {
            for (var i = 0; i < Controls.Length; i++)
                Controls[i].KeyboardKey = new KeyCodePersistentProperty(KeyCode.None, Controls[i].Id);
        }

        public void ReplaceKey(string id, KeyCode newKey)
        {
            for (var i = 0; i < Collection.Length; i++)
                if (I.Collection[i].Id == id)
                    I.Collection[i].KeyboardKey.Value = newKey;
        }
    }

    [Serializable]
    public struct ControlsMappingsDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private XboxGamepadButton _xboxGamepadButton;
        [SerializeField] private KeyCodePersistentProperty _keyboardKey;

        public string Id => _id;
        public XboxGamepadButton XboxGamepadButton => _xboxGamepadButton;
        public KeyCodePersistentProperty KeyboardKey { get => _keyboardKey; set => _keyboardKey = value; }
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