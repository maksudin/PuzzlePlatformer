using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using PixelCrew.Model.Data;
using PixelCrew.Components.UI.EscMenu;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature, ICanAddInInventory
    {
        [Header("Menu")]
        [SerializeField] private GameObject _menuPrefub;
        [SerializeField] private Transform _canvasTransform;
        private bool _menuShown = false;
        private GameObject _menuInstance;

        [Header("Hero Params")]
        [SerializeField] private int _potionHeal;
        [SerializeField] private float _gravityScale = 3;
        [SerializeField] private float _fallingGravityScale = 5;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private int _swordBurstAmount = 3;
        [SerializeField] private bool _allowDoubleJump = false;

        [Header("Interactions")]
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private float _interactionRadius;

        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private AnimatorController _animatorArmed;
        [SerializeField] private AnimatorController _animatorDisarmed;


        [Header("Rope Params")]
        [SerializeField] private float _pushForce;
        public bool PlayerAttachedToRope;

        public CapsuleCollider2D HeroCollider;

        private bool _emulateGroundCondition;
        private float _groundTime = 0.1f;
        private float _groundTimer = 0.0f;
        private bool _hasDoubleJump;

        private Coroutine _currentCoroutine;

        private GameSession _session;
        private static readonly int RopeAttached = Animator.StringToHash("rope_attached");

        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int PotionCount => _session.Data.Inventory.Count("Potion");

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
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.Data.Inventory.OnChanged += AnotherHandler;

            LoadSession();
        }

        private void OnDestroy()
        {
            if (!_session) return;
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.Data.Inventory.OnChanged -= AnotherHandler;
        }

        private void AnotherHandler(string id, int value)
        {
            Debug.Log($"Inventory changed: {id}: {value}");
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        private void LoadSession()
        {
            UpdateHeroHp();
            UpdateHeroWeapon();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void UsePotion()
        {
            if (PotionCount > 0)
            {
                HealthComp.Heal(_potionHeal);
                _session.Data.Inventory.Remove("Potion", 1);
                Particles.Spawn("Potion");
            }

        }

        public void CallMenu()
        {
            if (!_canvasTransform) return;
            if (!_menuShown)
            {
                _menuInstance = Instantiate(_menuPrefub, _canvasTransform);
                _menuShown = true;
            }
            else
            {
                _menuInstance?.GetComponent<EscMenuWindow>().Close();
                _menuShown = false;
            }
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
            if (SwordCount <= 0) return;
            base.Attack();
        }

        public void Throw(bool hasCooldown)
        {
            if (hasCooldown)
            {
                if (_throwCooldown.IsReady && SwordCount > 1)
                {
                    Sounds.Play("Range");
                    Animator.SetTrigger(ThrowKey);
                    _throwCooldown.Reset();
                    _session.Data.Inventory.Remove("Sword", 1);
                    return;
                }
            }
            else if (SwordCount > 1)
            {
                Sounds.Play("Range");
                Animator.SetTrigger(ThrowKey);
                _session.Data.Inventory.Remove("Sword", 1);
            }
        }

        public void ThrowBurst()
        {
            if (SwordCount > 1)
            {
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);
                _currentCoroutine = StartCoroutine(ThrowBurstCoroutine());
            }
        }

        private IEnumerator ThrowBurstCoroutine()
        {
            for (var i = 0; i < _swordBurstAmount; i++)
            {
                Throw(false);
                Direction = Vector2.zero;

                yield return new WaitForSeconds(0.15f);
            }

            StopCoroutine(_currentCoroutine);
        }

        public void OnDoThrow()
        {
            Particles.Spawn("Throw");
        }

        private void UpdateHeroHp()
        {
            var savedHp = _session.Data.Hp.Value;
            HealthComponent health = GetComponent<HealthComponent>();
            health.SetHealth(savedHp);
            _session.Data.Hp.Value = savedHp;
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _animatorArmed : _animatorDisarmed;

        }

        public void OnHealthChange(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
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

            if (IsGrounded && _allowDoubleJump)
            {
                _hasDoubleJump = true;
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
            if (!IsGrounded && _hasDoubleJump)
            {
                yVelocity = JumpSpeed;
                _hasDoubleJump = false;
                DoJumpVfx();
                return JumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);


            Burst burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;

            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
    }
}
