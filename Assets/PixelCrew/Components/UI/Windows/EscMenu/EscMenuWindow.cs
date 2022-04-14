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
        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();
            _reloadLevel = GetComponent<ReloadLevelComponent>();

            // Пауза.
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public override void OnExit()
        {
            //base.OnExit();
            SceneManager.LoadScene("MainMenu");
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }

        public void OnRestart()
        {
            _reloadLevel?.Reload();
        }

        public override void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
            _closeAction?.Invoke();
        }
    }
}