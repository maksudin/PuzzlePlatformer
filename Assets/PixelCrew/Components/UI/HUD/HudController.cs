using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
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
            WindowUtils.CreateWindow("UI/EscMenuWindow");
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }

    }
}