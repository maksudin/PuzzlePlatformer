﻿using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")] [SerializeField] private bool _invertScale;
        [Header("Params")]
        [SerializeField] private float _speed;
        [SerializeField] protected float JumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] protected int Damage;

        [Header("Checkers")]
        [SerializeField] private LayerCheck GroundCheck;
        [SerializeField] protected CheckCircleOverlap AttackRange;
        [SerializeField] protected SpawnListComponent Particles;

        protected Rigidbody2D Rigidbody;
        public Vector2 Direction;
        protected Animator Animator;
        protected bool IsGrounded;
        protected bool FallIsLongEnough;


        private static readonly int IsGround = Animator.StringToHash("is_grounded");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical_velocity");
        private static readonly int IsRunning = Animator.StringToHash("is_running");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        private static readonly int Hit = Animator.StringToHash("hit");
        

        [SerializeField] protected float AttackParticlesOffset;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = GroundCheck.IsTouchingLayer;
        }

        protected virtual void FixedUpdate()
        {
            // Движение по оси X.
            var velocityX = Direction.x * _speed;
            float velocityY;

            velocityY = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(velocityX, velocityY);

            UpdateAnimatorVals();
            UpdateSpriteDirection();
        }

        

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                if (FallIsLongEnough)
                {
                    Particles.Spawn("Fall");
                    FallIsLongEnough = false;
                }
            }

            if (isJumpPressing)
            {
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }

            else if (Rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                //SpawnJumpDustParticles();
                Particles.Spawn("Jump");
                yVelocity += JumpSpeed;
            }

            return yVelocity;
        }

        protected virtual void UpdateAnimatorVals()
        {
            Animator.SetBool(IsGround, IsGrounded);
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
            Animator.SetBool(IsRunning, Direction.x != 0);

        }

        private void UpdateSpriteDirection()
        {
            var multiplier = _invertScale ? -1 : 1;
            if (Direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (Direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage()
        {
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
            if (Direction.x > 0 || Direction.x < 0)
            {
                //_attackParticles.SpawnWithOffset(new Vector2 (_attackParticlesOffset * Direction.x, 0));
                Particles.SpawnWithOffset("Attack", new Vector2(AttackParticlesOffset * Direction.x, 0));
            }
            else
            {
                //_attackParticles.Spawn();
                Particles.Spawn("Attack");
            }
        }

        public void OnDoAttack()
        {
            AttackRange.Check();
            //foreach (GameObject go in gameobjects)
            //{
            //    HealthComponent hp = go.GetComponent<HealthComponent>();
            //    if (hp != null && go.CompareTag("Enemy"))
            //    {
            //        hp.ApplyDamage(Damage);
            //    }
            //}
        }
    }
}
