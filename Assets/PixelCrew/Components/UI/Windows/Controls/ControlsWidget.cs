using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.PixelCrew.Components.UI.Windows.Controls
{
    public class ControlsWidget : MonoBehaviour, IItemRenderer<InputAction>
    {
        [SerializeField]
        private Text _controlName,
                     _keyboardKey;
        [SerializeField] private Image _gamepadIcon;
        [SerializeField] private GameObject _selector;

        private GameSession _session;
        private InputAction _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }

        public void SetData(InputAction data, int _)
        {
            _data = data;
            UpdateView();
        }

        private void UpdateView()
        {
            _controlName.text = LocalizationManager.I.Localize(_data.name);
            _keyboardKey.text = _data.GetBindingDisplayString(0);
            //_controlName.text = LocalizationManager.I.Localize(_data.Id);
            //var xboxIcons = DefsFacade.I.ControlIcons.XboxIcons;
            //var xboxIcon = xboxIcons.FirstOrDefault(x => x.XboxGamepadButton == _data.XboxGamepadButton).XboxGamepadIcon;
            //_gamepadIcon.sprite = xboxIcon;

            //var key = _data.KeyboardKey.Value;
            ////var keyboardIcons = DefsFacade.I.ControlIcons.KeyboardIcons;
            ////var keyIcon = keyboardIcons.FirstOrDefault(x => x.KeyboardButton == key).KeyboardIcon;
            //_keyboardKey.text = key.ToString();

            if (_session != null)
                _selector.gameObject.SetActive(_session.ControlsModel.InterfaceSelectedControl.Value == _data.name);

        }

        public void OnSelect() =>
            _session.ControlsModel.InterfaceSelectedControl.Value = _data.name;
    }
}