using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using UnityEngine;
using UnityEngine.Profiling;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Creature : MonoBehaviour
    {
        [Header("Creature Params")] 
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed, JumpSpeed, AttackParticlesOffset;
        [SerializeField] private float _damageVelocity;
        [SerializeField] protected int Damage;

        [Header("Checkers")]
        [SerializeField] private LayerCheck GroundCheck;
        [SerializeField] protected CheckCircleOverlap AttackRange;
        [SerializeField] protected SpawnListComponent Particles;
        [HideInInspector] public Vector2 Direction;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected bool IsGrounded, FallIsLongEnough;
        protected HealthComponent Health;

        private static readonly int IsGroundKey = Animator.StringToHash("is_grounded"),
                                    VerticalVelocityKey = Animator.StringToHash("vertical_velocity"),
                                    IsRunningKey = Animator.StringToHash("is_running"),
                                    AttackKey = Animator.StringToHash("attack"),
                                    HitKey = Animator.StringToHash("hit");
        protected static readonly int ThrowKey = Animator.StringToHash("throw");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Health = GetComponent<HealthComponent>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        public void SetDirection(Vector2 direction) =>
            Direction = direction;

        protected virtual void Update()
        {
            IsGrounded = GroundCheck.IsTouchingLayer;
            //IsInWater = WaterCheck.IsTouchingLayer;
        }

        protected virtual void FixedUpdate()
        {
            var velocityX = CalculateXVelocity();
            // Движение по оси X.
            if (Rigidbody.velocity.y > 0 || Rigidbody.velocity.y < 0)
                velocityX *= 2;
            else if (Rigidbody.velocity.y > 10 || Rigidbody.velocity.y < 10)
                velocityX *= 1.3f;

            float velocityY;

            velocityY = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(velocityX, velocityY);

            UpdateAnimatorVals();
            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateXVelocity() => Direction.x * CalculateSpeed();
        protected virtual float CalculateSpeed() => _speed;

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded && FallIsLongEnough)
            {
                Particles.Spawn("Fall");
                FallIsLongEnough = false;
            }

            if (isJumpPressing)
            {
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }

            else if (Rigidbody.velocity.y > 0)
                yVelocity *= 0.5f;

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += JumpSpeed;
                DoJumpVfx();
            }

            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            Profiler.BeginSample("JumpVFXSample");
            Particles.Spawn("Jump");
            Profiler.EndSample();
            Sounds.Play("Jump");
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
                transform.localScale = new Vector3(multiplier, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
        }

        public virtual void TakeDamage()
        {
            Animator.SetTrigger(HitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y + _damageVelocity);
        }

        public virtual void Attack() => Animator.SetTrigger(AttackKey);

        public void OnDoAttack()
        {
            AttackRange.Check();
            Sounds.Play("Melee");

            if (Direction.x > 0 || Direction.x < 0)
                Particles.SpawnWithOffset("Attack", new Vector2(AttackParticlesOffset * Direction.x, 0));
            else
                Particles.Spawn("Attack");
        }
    }
}

