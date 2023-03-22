using PixelCrew.Components.ColliderBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingMeleeTrapAI : BaseShootingTrap
    {
        [Header("Melee")]
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private Cooldown _meleeCooldown;
        private static readonly int MeleeKey = Animator.StringToHash("melee_attack");

        protected override void Update()
        {
            base.Update();
            bool inVision = Vision.IsTouchingLayer;
            bool inAttackRange = _meleeCanAttack.IsTouchingLayer;
            bool coolDownReady = _meleeCooldown.IsReady;
            if (inVision && inAttackRange && coolDownReady)
                MeleeAttack();
        }

        public override void RangeAttack() => base.RangeAttack();
        public void OnMeleeAttack() => _meleeAttack.Check();

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            Animator.SetTrigger(MeleeKey);
            Sounds?.Play("Melee");
        }
    }
}