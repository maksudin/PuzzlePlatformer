using System;
using System.Collections;
using PixelCrew.Components.Health;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace Assets.PixelCrew.Components.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private ProgressBarWidget _hpBar;
        [SerializeField] private CanvasGroup _canvas;
        private float _maxHealth;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _maxHealth = _health.CurrentHealth;
            _trash.Retain(_health.OnChange.Subscribe(OnHpChanged));
        }

        public void ShowUI()
        {

        }

        private void HideUi()
        {

        }

        private void OnHpChanged(int health)
        {
            _hpBar.SetProgress(health / _maxHealth);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}