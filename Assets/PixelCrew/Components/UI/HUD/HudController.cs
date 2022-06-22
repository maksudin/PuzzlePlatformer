using Assets.PixelCrew.Model.Definitions.Player;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Components.UI.Windows.EscMenu;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;
            OnHealthChanged(_session.Data.Hp.Value, 0);
        }

        public void OnMenu()
        {
            var menu = FindObjectOfType<EscMenuWindow>();
            if (menu == null)
                WindowUtils.CreateWindow("UI/EscMenuWindow");
            else
                menu.Close();
        }

        public void OnDebug()
        {
            WindowUtils.CreateWindow("UI/PerkWindow");
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float) newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }

    }
}