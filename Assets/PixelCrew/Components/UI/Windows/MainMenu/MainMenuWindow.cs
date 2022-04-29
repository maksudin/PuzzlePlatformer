using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimationWindow
    {
        protected Action _closeAction;

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/PerkWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () => {
                SceneManager.LoadScene("Level1");
            };
            Close();
        }

        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }


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