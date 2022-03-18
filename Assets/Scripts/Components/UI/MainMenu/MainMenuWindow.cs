using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.UI.MainMenu
{
    public class MainMenuWindow : AnimationWindow
    {
        private Action _closeAction;


        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("UI/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }

        public void OnStartGame()
        {
            _closeAction = () => {
                SceneManager.LoadScene("Level1");
            };
            Close();

        }

        public void OnExit()
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