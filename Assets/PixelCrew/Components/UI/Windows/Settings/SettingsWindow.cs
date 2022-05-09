using PixelCrew.Components.UI.Widgets;
using PixelCrew.Components.UI.Windows;
using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Components.UI.Windows.Settings
{
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;

        protected override void Start()
        {
            base.Start();

            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }

    }
}