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
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Utils.Disposables;
using Assets.PixelCrew.Model.Definitions.Player;
using Assets.PixelCrew.Utils;
using UnityEngine.Events;
using Assets.PixelCrew;
using Assets.PixelCrew.Effects.CameraRelated;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature, ICanAddInInventory
    {
        [Header("Hero Params")]
        [SerializeField] private float _gravityScale = 3;
        [SerializeField] private float _fallingGravityScale = 5;
        [SerializeField] private Cooldown _throwCooldown;
        public CapsuleCollider2D HeroCollider;
        private Cooldown _superThrowCooldown = new Cooldown();

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

        [SerializeField] private SpawnComponent _throwSpawner;

        [Header("Rope Params")]
        [SerializeField] private float _pushForce;
        public bool AttachedToRope;

        [Header("Hook Params")]
        [SerializeField] private CheckCircleOverlap _hookCheck;
        [SerializeField] private float _hookSpeed;
        [SerializeField] private float _hookPushForce;
        [SerializeField] private UnityEvent _onHookComplete;
        [SerializeField] private Cooldown _hookCooldown;
        [Space]
        [SerializeField] private CandleController _candle;

        private CameraShakeEffect _cameraShake;
        private bool _emulateGroundCondition;
        private float _groundTime = 0.1f;
        private float _groundTimer = 0.0f;
     
        private bool _hasDoubleJump;
        private bool _isDashing;

        private Coroutine _currentCoroutine;

        private GameSession _session;
        private static readonly int RopeAttached = Animator.StringToHash("rope_attached");

        private const string SwordId = "Sword";

        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private int CoinCount => _session.Data.Inventory.Count("Coin");

        private string SelectedItemId => _session.QuickInventory.SelectedDef.Id;

        private CompositeDisposable _trash = new CompositeDisposable();

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
            _groundTimer = _groundTime;
        }

        private void Start()
        {
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChangedInventory += OnInventoryChanged;
            _session.Data.Inventory.OnChangedInventory += AnotherHandler;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;

            LoadSession();
            _superThrowCooldown.Value = DefsFacade.I.Perks.Get("super-throw").Cooldown;

            var health = (int)_session.StatsModel.GetValue(StatId.Hp);
            _session.Data.Hp.Value = health;
            Health.SetHealth(health);
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int)_session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = health;
                    Health.SetHealth(health);
                    break;
                case StatId.Speed:
                    break;
                case StatId.RangeDamage:
                    break;
            }
        }

        private void OnDestroy()
        {
            if (!_session) return;
            _session.Data.Inventory.OnChangedInventory -= OnInventoryChanged;
            _session.Data.Inventory.OnChangedInventory -= AnotherHandler;

            _trash.Dispose();
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
                if (IsSelectedItem(ItemTag.Potion))
                    UsePotion();
                if (IsSelectedItem(ItemTag.Candle))
                    ReplanishCandle();
                return;
            }
            
            if (interaction is HoldInteraction && _session.PerksModel.IsSuperThrowSupported)
                SuperThrow();
            else
                Throw();

        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        public void SetDash(bool isDashing)
        {
            _isDashing = isDashing;
        }

        public void CallMenu()
        {
            var menu = FindObjectOfType<EscMenuWindow>();
            if (menu == null)
                WindowUtils.CreateWindow("UI/EscMenuWindow");
            else
                menu.Close();
        }

        public void AttachPlayerToRope()
        {
            AttachedToRope = true;
        }

        public void DetachPlayerFromRope()
        {
            AttachedToRope = false;
            _emulateGroundCondition = true;
        }

        public override void Attack()
        {
            if (SwordCount <= 0) return;

            base.Attack();
        }

        private bool _candleActive;

        public void UseCandle()
        {
            if (!_candleActive)
            {
                _candle.TurnOnCandle();
                _candleActive = true;
            }
            else
            {
                _candle.TurnOffCandle();
                _candleActive = false;
            }
        }

        private void ReplanishCandle()
        {
            _candle.ResetCapacity();
            _session.Data.Inventory.Remove(SelectedItemId, 1);
        }

        public void CandleRanOut()
        {
            _candleActive = false;
        }

        private Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    Health.Heal((int)potion.Value);
                    break;
                case Effect.SpeedUp:
                    _speedUpCooldown.Value = _speedUpCooldown.TimeLasts + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }

            _session.Data.Inventory.Remove(potion.Id, 1);
            Particles.Spawn("Potion");
        }

        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            float defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
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

        public void SuperThrow()
        {
            if (!_session.Data.Perks.UsedPerkCooldown.IsReady) return;
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

            _session.Data.Perks.UsedPerkCooldown.Reset();
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
            if (_session.PerksModel.IsHookSupported)
                _hookCheck.Check();
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

        private bool _isHooking;
        private Vector3 _hookTarget;

        protected override void FixedUpdate()
        {
            if (_isHooking)
            {
                var direction = _hookTarget - transform.position;
                var ease = EasingUtils.EaseInSine(start: 0, end: _hookSpeed, value: 0.67f);
                Rigidbody.MovePosition(transform.position + direction.normalized * ease * Time.fixedDeltaTime);
                if (Vector3.Distance(transform.position, _hookTarget) < 0.5f || _hookCooldown.IsReady)
                {
                    _onHookComplete?.Invoke();
                    _isHooking = false;
                }    
            }
            else 
                base.FixedUpdate();
        }

        public void HookTo(GameObject go)
        {
            _isHooking = true;
            _hookTarget = go.transform.position;
            _hookCooldown.Reset();
        }

        protected override void UpdateAnimatorVals()
        {
            base.UpdateAnimatorVals();
            Animator.SetBool(RopeAttached, AttachedToRope);
        }

        protected override float CalculateXVelocity()
        {
            var modifier = _isDashing ? 10 : 1;
            return base.CalculateXVelocity() * modifier;
        }

        protected override float CalculateYVelocity()
        {
            var velocityY = Rigidbody.velocity.y;

            if (AttachedToRope)
            {
                // Замедление падения после detach.
                velocityY = Rigidbody.velocity.y / 100f;
                return velocityY;
            }

            if (IsGrounded && _session.PerksModel.IsDoubleJumpSupported || _allowDoubleJump)
                _hasDoubleJump = true;

            // Падение с двойного прыжка имеет velocity около -15.
            if (Rigidbody.velocity.y < -15)
                FallIsLongEnough = true;

            // Гравитация меняется при падении.
            Rigidbody.gravityScale = Rigidbody.velocity.y >= 0 ? _gravityScale : _fallingGravityScale;

            return base.CalculateYVelocity();
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

            if (_cameraShake != null)
                _cameraShake.Shake();

            if (CoinCount > 0)
                SpawnCoins();
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
