using PixelCrew.Components.LevelManagement;
using PixelCrew.Components.UI.MainMenu;
using UnityEngine;

namespace PixelCrew.Components.UI.EscMenu
{
    public class EscMenuWindow : MainMenuWindow
    {
        private ReloadLevelComponent _reloadLevel;

        public override void Start()
        {
            base.Start();
            _reloadLevel = GetComponent<ReloadLevelComponent>();
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