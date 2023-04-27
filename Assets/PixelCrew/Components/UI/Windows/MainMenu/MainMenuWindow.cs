using System;
using Assets.PixelCrew.Components.UI.LevelsLoader;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        protected Action _closeAction;

        public void OnShowSettings() =>
            WindowUtils.CreateWindow("UI/SettingsWindow");

        public void OnStartGame()
        {
            _closeAction = () => 
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Ropes level");
            };
            Close();
        }

        public void OnControls() =>
            WindowUtils.CreateWindow("UI/ControlsWindow");

        public void OnLanguages() =>
            WindowUtils.CreateWindow("UI/LocalizationWindow");

        public virtual void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };


            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            SceneManager.LoadScene("Level1");
            _closeAction?.Invoke();
        }
    }
}