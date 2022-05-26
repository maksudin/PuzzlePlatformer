using Assets.PixelCrew.Model.Data.Properties;
using PixelCrew.Components.UI.Windows;
using PixelCrew.Model;
using UnityEngine;

namespace Assets.PixelCrew.Components.UI.HUD.Dialogs
{
    public class RemapDialogController : AnimatedWindow
    {
        private GameSession _session;

        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
        }

        public void OnRemap(KeyCode key)
        {
            if (_session == null) return;
            var controlsModel = _session.ControlsModel;
            var selected = controlsModel.InterfaceSelectedControl.Value;
            controlsModel.RemapButton(selected, key);
            Close();
        }
    }
}