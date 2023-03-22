using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class BaseShootingTrap : MonoBehaviour
    {
        [SerializeField] protected LayerCheck Vision;
        [Header("Range"), SerializeField] protected SpawnComponent ProjectileAttack;
        [SerializeField] public Cooldown RangeCooldown;
        [Space, SerializeField] public bool IsTotem;
        protected PlaySoundsComponent Sounds;
        protected Animator Animator;
        protected static readonly int RangeKey = Animator.StringToHash("range_attack");

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        protected virtual void Update()
        {
            if (!IsTotem)
                RangeAttack();
        }

        public virtual void RangeAttack()
        { 
            if (Vision.IsTouchingLayer && RangeCooldown.IsReady)
            {
                RangeCooldown.Reset();
                Animator.SetTrigger(RangeKey);
                Sounds?.Play("Range");
            }
        }

        public void OnRangeAttack() => ProjectileAttack.Spawn();
    }
}