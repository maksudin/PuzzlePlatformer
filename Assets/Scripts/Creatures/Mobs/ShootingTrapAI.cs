using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace Assets.Scripts.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Melee")]
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private Cooldown _meleeCooldown;

        [Header("Range")]
        [SerializeField] private SpawnComponent _projectileAttack;
        [SerializeField] private Cooldown _rangeCooldown;

        private Animator _animator;
        private static readonly int MeleeKey = Animator.StringToHash("melee_attack");
        private static readonly int RangeKey = Animator.StringToHash("range_attack");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                        MeleeAttack();
                    return;
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }

            }
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(RangeKey);
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(MeleeKey);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _projectileAttack.Spawn();
        }
    }
}