using System.Linq;
using Assets.PixelCrew.Model.Definitions.Controls;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelCrew.Components.UI.Windows.Controls
{
    public class ControlsWidget : MonoBehaviour, IItemRenderer<ControlsMappingsDef>
    {
        [SerializeField] private Text _controlName;
        [SerializeField] private Image _gamepadIcon;
        [SerializeField] private Text _keyboardKey;
        [SerializeField] private GameObject _selector;

        private GameSession _session;
        private ControlsMappingsDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }

        public void SetData(ControlsMappingsDef data, int index)
        {
            _data = data;
            UpdateView();
        }

        private void UpdateView()
        {
            _controlName.text = LocalizationManager.I.Localize(_data.Id);
            var xboxIcons = DefsFacade.I.ControlIcons.XboxIcons;
            var xboxIcon = xboxIcons.FirstOrDefault(x => x.XboxGamepadButton == _data.XboxGamepadButton).XboxGamepadIcon;
            _gamepadIcon.sprite = xboxIcon;
            
            var key = _data.KeyboardKey;
            //var keyboardIcons = DefsFacade.I.ControlIcons.KeyboardIcons;
            //var keyIcon = keyboardIcons.FirstOrDefault(x => x.KeyboardButton == key).KeyboardIcon;
            _keyboardKey.text = _data.KeyboardKey.ToString();

            if (_session != null)
                _selector.gameObject.SetActive(_session.ControlsModel.InterfaceSelectedControl.Value == _data.Id);

        }

        public void OnSelect()
        {
            _session.ControlsModel.InterfaceSelectedControl.Value = _data.Id;
        }
    }
}