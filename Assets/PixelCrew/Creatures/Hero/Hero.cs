using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using PixelCrew.Model.Data;
using PixelCrew.Components.GoBased;
using PixelCrew.Model.Definitions;
using PixelCrew.Components.UI.Windows.EscMenu;
using PixelCrew.Model.Definitions.Items;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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
        [SerializeField] private float _gravityScale = 3;
        [SerializeField] private float _fallingGravityScale = 5;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private int _swordBurstAmount = 3;
        [SerializeField] private bool _allowDoubleJump = false;

        [Header("Potions Params")]
        [SerializeField] private int _redPotionHeal = 5;
        [SerializeField] private int _bluePotionHeal = 10;

        [Header("Interactions")]
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private float _interactionRadius;

        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private AnimatorController _animatorArmed;
        [SerializeField] private AnimatorController _animatorDisarmed;

        [SerializeField] private SpawnComponent _throwSpawner;

        [Header("Rope Params")]
        [SerializeField] private float _pushForce;
        public bool PlayerAttachedToRope;

        public CapsuleCollider2D HeroCollider;

        private bool _emulateGroundCondition;
        private float _groundTime = 0.1f;
        private float _groundTimer = 0.0f;
     
        private bool _hasDoubleJump;
        private bool _isDashing;

        private Coroutine _currentCoroutine;

        private GameSession _session;
        private static readonly int RopeAttached = Animator.StringToHash("rope_attached");

        private const string SwordId = "Sword";
        private const string RedPotion = "RedPotion";
        private const string BluePotion = "BluePotion";

        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        //private int PotionCount => _session.Data.Inventory.Count("Potion");

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        

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
            _session.Data.Inventory.OnChangedInventory += OnInventoryChanged;
            _session.Data.Inventory.OnChangedInventory += AnotherHandler;

            LoadSession();
        }

        private void OnDestroy()
        {
            if (!_session) return;
            _session.Data.Inventory.OnChangedInventory -= OnInventoryChanged;
            _session.Data.Inventory.OnChangedInventory -= AnotherHandler;
        }

        private void AnotherHandler(string id, int value)
        {
            Debug.Log($"Inventory changed: {id}: {value}");
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }

        private void LoadSession()
        {
            UpdateHeroHp();
            UpdateHeroWeapon();
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void UseInventory(IInputInteraction interaction)
        {
            if (!CanThrow)
            {

                return;
            }

            if (interaction is PressInteraction)
            {
                 Throw();
            }

            else if (interaction is HoldInteraction)
            {
                ThrowBurst();
            }
        }

        public void SetDash(bool isDashing)
        {
            _isDashing = isDashing;
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
            if (SelectedItemId == RedPotion)
            {
                UsePotion(RedPotion);
                return;
            }

            if (SelectedItemId == BluePotion)
            {
                UsePotion(BluePotion);
                return;
            }

            if (SelectedItemId == SwordId)
                base.Attack();

            if (SwordCount <= 0) return;
        }

        private void UsePotion(string potion)
        {
            if (potion == RedPotion)
            {
                HealthComp.Heal(_redPotionHeal);
                _session.Data.Inventory.Remove(RedPotion, 1);
            }

            if (potion == BluePotion)
            {
                HealthComp.Heal(_bluePotionHeal);
                _session.Data.Inventory.Remove(BluePotion, 1);
            }

            Particles.Spawn("Potion");
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady && CanThrow)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
                return;
            }
        }

        public void ThrowBurst()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(ThrowBurstCoroutine());
        }

        private IEnumerator ThrowBurstCoroutine()
        {
            for (var i = 0; i < _swordBurstAmount; i++)
            {
                Animator.SetTrigger(ThrowKey);
                Direction = Vector2.zero;

                yield return new WaitForSeconds(0.15f);
            }

            StopCoroutine(_currentCoroutine);
        }

        public void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);

            _throwSpawner.SetPrefub(throwableDef.Projectile);
            _throwSpawner.Spawn();

            _session.Data.Inventory.Remove(throwableId, 1);

        }

        private void UpdateHeroHp()
        {
            var maxHp = DefsFacade.I.Player.MaxHealth;
            _session.Data.Hp.Value = maxHp;
            HealthComponent health = GetComponent<HealthComponent>();
            health.SetHealth(maxHp);
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


        protected override float CalculateXVelocity()
        {
            var modifier = _isDashing ? 10 : 1;
            return base.CalculateXVelocity() * modifier;
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
