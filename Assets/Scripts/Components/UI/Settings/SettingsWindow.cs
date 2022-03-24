using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Components.UI.Settings
{
    public class SettingsWindow : AnimationWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;

        public override void Start()
        {
            base.Start();

            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }

    }
}