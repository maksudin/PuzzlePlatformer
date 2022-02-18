using System;
using System.Collections;
using System.Collections.Generic;
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

            if (Vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                        MeleeAttack();
                    return;
                }
            }
        }

        public override void RangeAttack()
        {
            base.RangeAttack();
            Animator.SetTrigger(RangeKey);
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            Animator.SetTrigger(MeleeKey);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

    }
}