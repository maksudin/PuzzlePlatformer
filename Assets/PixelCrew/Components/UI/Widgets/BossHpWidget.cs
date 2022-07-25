using System;
using System.Collections;
using Assets.PixelCrew.Utils;
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
        
        [ContextMenu("Show")]
        public void ShowUI()
        {
            this.LerpAnimated(start: 0, end: 1, time: 1, SetAlpha);
        }

        [ContextMenu("Hide")]
        public void HideUi()
        {
            this.LerpAnimated(start: 1, end: 0, time: 1, SetAlpha);
        }

        private void SetAlpha(float alpha)
        {
            _canvas.alpha = alpha;
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