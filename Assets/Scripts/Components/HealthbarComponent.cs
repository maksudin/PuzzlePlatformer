using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealthbarComponent : MonoBehaviour
    {
        private HealthComponent _healthComponent;
        [SerializeField] private Slider _slider;
        [SerializeField] private Color _low;
        [SerializeField] private Color _high;
        [SerializeField] private Vector3 _offset;

        private Image _sliderImage;

        private void Awake()
        {
            _sliderImage = _slider.fillRect.GetComponentInChildren<Image>();
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void SetHealth(float health, float maxHealth)
        {
            //_slider.gameObject.SetActive(health < maxHealth);
            _slider.value = health;
            _slider.maxValue = maxHealth;

            _sliderImage.color = Color.Lerp(_low, _high, _slider.normalizedValue);
        }
        private void Update()
        {
            _slider.transform.position = Vector3.zero;
            SetHealth(_healthComponent.CurrentHealth, _healthComponent.MaxHealth);
        }
    }
}