using PixelCrew.Components.UI.MainMenu;

namespace PixelCrew.Components.UI.EscMenu
{
    public class EscMenuWindow : MainMenuWindow
    {
        public void OnRestart()
        {

        }

        public override void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
            _closeAction?.Invoke();
        }
    }
}