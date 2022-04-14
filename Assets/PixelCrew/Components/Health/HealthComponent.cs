using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [Range (1, 100)]
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public UnityEvent OnDie;
        [SerializeField] public HealthChangeEvent OnChange;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;   
        }

        public void ApplyDamage(int damageValue)
        {
            _currentHealth -= damageValue;
            OnChange?.Invoke(_currentHealth);
            _onDamage?.Invoke();    // Проверка на null
            if (_currentHealth <= 0)
            {
                OnDie?.Invoke();
            }

        }

        public void Heal(int healValue)
        {
            if (_currentHealth + healValue > _maxHealth)
            {
                _currentHealth = _maxHealth;
                
            } 
            else
            {
                _currentHealth += healValue;
            }

            OnChange?.Invoke(_currentHealth);
            _onHeal?.Invoke();
        }

        public void SetHealth(int hp)
        {
            _maxHealth = hp;
            _currentHealth = hp;
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int> { }
    }
}


