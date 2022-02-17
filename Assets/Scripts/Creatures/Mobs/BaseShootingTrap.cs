using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class BaseShootingTrap : MonoBehaviour
    {
        [SerializeField] protected LayerCheck Vision;

        [Header("Range")]
        [SerializeField] protected SpawnComponent ProjectileAttack;
        [SerializeField] protected Cooldown RangeCooldown;

        protected Animator Animator;
        protected static readonly int RangeKey = Animator.StringToHash("range_attack");

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (Vision.IsTouchingLayer)
            {
                if (RangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        protected virtual void RangeAttack()
        {
            RangeCooldown.Reset();
            Animator.SetTrigger(RangeKey);
        }

        public void OnRangeAttack()
        {
            ProjectileAttack.Spawn();
        }
    }
}