using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;

        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onDamage?.Invoke();    // Проверка на null
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }

        }

        public void Heal(int healValue)
        {
            if (_health + healValue > _maxHealth)
            {
                _health = _maxHealth;
                
            } else
            {
                _health += healValue;
            }

            _onHeal?.Invoke();
        }


    }
}


