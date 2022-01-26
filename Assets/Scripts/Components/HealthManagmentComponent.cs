using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class HealthManagmentComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _healValue;

        public void TakeDamage(GameObject target)
        {
            HealthComponent health = target.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.ApplyDamage(_damage);
            }
        }


        public void Heal(GameObject target)
        {
            HealthComponent health = target.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.Heal(_healValue);
            }
        }
    }

}