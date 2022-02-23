using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
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

        protected HealthComponent HealthComp;



        private static readonly int IsGroundKey = Animator.StringToHash("is_grounded");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical_velocity");
        private static readonly int IsRunningKey = Animator.StringToHash("is_running");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        private static readonly int HitKey = Animator.StringToHash("hit");
        protected static readonly int ThrowKey = Animator.StringToHash("throw");



        [SerializeField] protected float AttackParticlesOffset;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            HealthComp = GetComponent<HealthComponent>();
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
            UpdateSpriteDirection(Direction);
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
            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);

        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage()
        {
            Animator.SetTrigger(HitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y + _damageVelocity);
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

