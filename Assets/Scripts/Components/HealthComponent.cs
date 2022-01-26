using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [Range (1, 100)]
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;

        private void Awake()
        {
            _currentHealth = _maxHealth;   
        }

        public void ApplyDamage(int damageValue)
        {
            _currentHealth -= damageValue;
            _onDamage?.Invoke();    // Проверка на null
            if (_currentHealth <= 0)
            {
                _onDie?.Invoke();
            }

        }

        public void Heal(int healValue)
        {
            if (_currentHealth + healValue > _maxHealth)
            {
                _currentHealth = _maxHealth;
                
            } else
            {
                _currentHealth += healValue;
            }

            _onHeal?.Invoke();
        }


    }
}


