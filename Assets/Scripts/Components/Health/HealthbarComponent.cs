using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.UI.Widgets;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components.Health
{
    [RequireComponent(typeof(HealthComponent))]
    [ExecuteInEditMode]
    public class HealthbarComponent : MonoBehaviour
    {
        private HealthComponent _healthComponent;
        //[SerializeField] private Slider _slider;
        [SerializeField] private Vector3 _offset;

        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private Canvas _canvas;

        private void Awake()
        {
            //_sliderImage = _slider.fillRect.GetComponentInChildren<Image>();
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void SetHealth(float health, float maxHealth)
        {
            maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)health / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void Update()
        {
            var targetPosition = Camera.main.WorldToViewportPoint(transform.position);
            //var screenPostion = new Vector2((targetPosition.x * _canvas.rect))

            _healthBar.transform.position = targetPosition;
            SetHealth(_healthComponent.CurrentHealth, _healthComponent.MaxHealth);
        }
    }
}