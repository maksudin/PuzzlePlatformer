using PixelCrew.Components;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace PixelCrew
{
    public class Hero : Creature
    {
        [SerializeField] private float _gravityScale = 3;
        [SerializeField] private float _fallingGravityScale = 5;

        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private float _interactionRadius;

        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private AnimatorController _animatorArmed;
        [SerializeField] private AnimatorController _animatorDisarmed;

        private bool _allowDoubleJump;
        public bool PlayerAttachedToRope;
        [SerializeField] private float _pushForce;

        public CapsuleCollider2D HeroCollider;

        private bool _emulateGroundCondition;
        [SerializeField] private float _groundTime = 0.1f;
        private float _groundTimer = 0.0f;
        private GameSession _session;
        private CheckPointComponent _checkPoint;

        private static readonly int RopeAttached = Animator.StringToHash("rope_attached");

        protected override void Awake()
        {
            base.Awake();
            HeroCollider = GetComponent<CapsuleCollider2D>();
            FallIsLongEnough = false;
            PlayerAttachedToRope = false;
            _groundTimer = _groundTime;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _checkPoint = FindObjectOfType<CheckPointComponent>();

            UpdateHeroCollectables();
            UpdateHeroHp();
            var isArmed = _session.SavedData.IsArmed;
            _session.LocalData.IsArmed = isArmed;
            if (isArmed) UpdateHeroWeapon();

            if (_session.SavedData.CheckPointPos != Vector3.zero)
            {
                transform.position = _session.SavedData.CheckPointPos;
            }
        }

        public void SaveSession()
        {
            _session.SavePlayer();
        }

        public void SetCheckPoint(Transform checkPoint)
        {
            _session.LocalData.CheckPointPos = checkPoint.position;
        }

        public void ClearCheckPoint()
        {
            _session.LocalData.CheckPointPos = Vector3.zero;
            _session.SavedData.CheckPointPos = Vector3.zero;
        }

        public void AttachPlayerToRope()
        {
            HeroCollider.enabled = false;
            PlayerAttachedToRope = true;
        }

        public void DetachPlayerFromRope()
        {
            HeroCollider.enabled = true;
            PlayerAttachedToRope = false;
            _emulateGroundCondition = true;
        }

        public override void Attack()
        {
            if (!_session.LocalData.IsArmed) return;
            base.Attack();
        }

        public void ArmHero()
        {
            _session.LocalData.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroHp()
        {
            var savedHp = _session.SavedData.Hp;
            HealthComponent health = GetComponent<HealthComponent>();
            health.SetHealth(savedHp);
            _session.LocalData.Hp = savedHp;
        }

        private void UpdateHeroCollectables()
        {
            _session.LocalData.Coins = _session.SavedData.Coins;
        }

        private void UpdateHeroWeapon()
        {
            if (_session.LocalData.IsArmed)
            {
                Animator.runtimeAnimatorController = _animatorArmed;
            } 
            else
            {
                Animator.runtimeAnimatorController = _animatorDisarmed;
            }
        }

        public void OnHealthChange(int currentHealth)
        {
            _session.LocalData.Hp = currentHealth;
        }

        

        public void Interact()
        {
            _interactionCheck.Check();
        }
        

        protected override void Update() 
        {
            if (_emulateGroundCondition)
            {
                IsGrounded = true;
                _groundTimer -= Time.deltaTime;

                if (_groundTimer <= 0)
                {
                    IsGrounded = false;
                    _emulateGroundCondition = false;
                    _groundTimer = _groundTime;
                }
            }
            else
            {
                base.Update();
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        protected override void UpdateAnimatorVals()
        {
            base.UpdateAnimatorVals();
            Animator.SetBool(RopeAttached, PlayerAttachedToRope);
        }


        protected override float CalculateYVelocity()
        {

            var velocityY = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (PlayerAttachedToRope)
            {
                // Чтобы hero не падал быстро после detach.
                velocityY = Rigidbody.velocity.y / 100f;
                return velocityY;
            }

            if (IsGrounded)
            {
                _allowDoubleJump = true;
            }

            // Падение с двойного прыжка имеет velocity около -15.
            if (Rigidbody.velocity.y < -15)
            {
                FallIsLongEnough = true;
            }

            // Гравитация меняется при падении.
            Rigidbody.gravityScale = Rigidbody.velocity.y >= 0 ? _gravityScale : _fallingGravityScale;

            return base.CalculateYVelocity(); ;
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump)
            {
                Particles.Spawn("Jump");
                yVelocity = JumpSpeed;
                _allowDoubleJump = false;
                return JumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.LocalData.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.LocalData.Coins, 5);
            _session.LocalData.Coins -= numCoinsToDispose;


            Burst burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;

            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
    }
}
