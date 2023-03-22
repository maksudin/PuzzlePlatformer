using System;
using Assets.PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Range(1, 100)] private int _maxHealth, _currentHealth;
        [SerializeField] public UnityEvent OnDamage, OnDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public HealthChangeEvent OnChange;
        private Lock _immune = new Lock();

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public Lock Immune => _immune;

        private void Awake() => _currentHealth = _maxHealth;   

        public void ApplyDamage(int damageValue)
        {
            if (Immune.isLocked) return;
            _currentHealth -= damageValue;
            OnChange?.Invoke(_currentHealth);
            OnDamage?.Invoke();
            if (_currentHealth <= 0) OnDie?.Invoke();
        }

        public void Heal(int healValue)
        {
            if (_currentHealth + healValue > _maxHealth)
                _currentHealth = _maxHealth;
            else
                _currentHealth += healValue;

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


