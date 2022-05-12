using PixelCrew.Components.LevelManagement;
using PixelCrew.Components.UI.Windows.MainMenu;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.UI.Windows.EscMenu
{
    public class EscMenuWindow : MainMenuWindow
    {
        private ReloadLevelComponent _reloadLevel;

        protected override void Start()
        {
            base.Start();
            _reloadLevel = GetComponent<ReloadLevelComponent>();

            Pause();
        }

        public override void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }

        public void OnRestart()
        {
            Close();
            _closeAction = () => {
                _reloadLevel.Reload();
            };
        }

        public override void OnCloseAnimationComplete()
        {
            UnPause();
            Destroy(gameObject);
            _closeAction?.Invoke();
        }
    }
}