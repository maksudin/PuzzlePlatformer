using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        public Vector2 Direction;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _gravityScale = 3;
        [SerializeField] private float _fallingGravityScale = 5;

        [SerializeField] private float _damageJumpSpeed;

        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private CheckCircleOverlap _attackRange;

        [SerializeField] private float _interactionRadius;
        private Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;

        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpDustParticles;
        [SerializeField] private SpawnComponent _fallDustParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private SpawnComponent _attackParticles;
        [SerializeField] private float _attackParticlesOffset;

        [SerializeField] private AnimatorController _animatorArmed;
        [SerializeField] private AnimatorController _animatorDisarmed;

        private Animator _animator;
        private static readonly int IsGround = Animator.StringToHash("is_grounded");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical_velocity");
        private static readonly int IsRunning = Animator.StringToHash("is_running");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int RopeAttached = Animator.StringToHash("rope_attached");

        private bool _isGrounded;
        private bool _allowDoubleJump;
        public bool PlayerAttachedToRope;
        private bool _fallIsLongEnough;
        [SerializeField] private float _pushForce;

        public CapsuleCollider2D HeroCollider;

        private bool _emulateGroundCondition;
        [SerializeField] private float _groundTime = 0.1f;
        private float _groundTimer = 0.0f;
        private GameSession _session;
        private CheckPointComponent _checkPoint;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            HeroCollider = GetComponent<CapsuleCollider2D>();
            _animator = GetComponent<Animator>();
            _fallIsLongEnough = false;
            PlayerAttachedToRope = false;
            _groundTimer = _groundTime;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _checkPoint = FindObjectOfType<CheckPointComponent>();

            HealthComponent health = GetComponent<HealthComponent>();
            health.SetHealth(_session.LocalData.Hp);
            UpdateHeroWeapon();

            if (_session.SavedData.CheckPointPos != null)
            {
                transform.position = _session.SavedData.CheckPointPos.position;
            }

        }

        public void SaveSession()
        {
            _session.SavePlayer();
        }

        public void SetCheckPoint(Transform checkPoint)
        {
            _session.LocalData.CheckPointPos = checkPoint;
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

        public void Attack()
        {
            if (!_session.LocalData.IsArmed) return;
            _animator.SetTrigger(AttackKey);
            if (Direction.x > 0 || Direction.x < 0)
            {
                _attackParticles.SpawnWithOffset(new Vector2 (_attackParticlesOffset * Direction.x, 0));
            }
            else
            {
                _attackParticles.Spawn();
            }
            
        }

        public void ArmHero()
        {
            _session.LocalData.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            if (_session.LocalData.IsArmed)
            {
                _animator.runtimeAnimatorController = _animatorArmed;
            } 
            else
            {
                _animator.runtimeAnimatorController = _animatorDisarmed;
            }
        }

        public void OnHealthChange(int currentHealth)
        {
            _session.LocalData.Hp = currentHealth;
        }

        public void OnHeroAttack()
        {
            GameObject[] gameobjects = _attackRange.GetObjectsInRange();
            foreach (GameObject go in gameobjects)
            {
                HealthComponent hp = go.GetComponent<HealthComponent>();
                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ApplyDamage(_damage);
                }
            }
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position,
                                                      _interactionRadius,
                                                      _interactionResult,
                                                      _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                interactable?.Interact();
            }
        }


        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        private void Update() 
        {
            if (_emulateGroundCondition)
            {
                _isGrounded = true;
                _groundTimer -= Time.deltaTime;

                if (_groundTimer <= 0)
                {
                    _isGrounded = false;
                    _emulateGroundCondition = false;
                    _groundTimer = _groundTime;
                }
            }
            else
            {
                _isGrounded = IsGrounded();
            }
        }

        private void FixedUpdate()
        {
            // Движение по оси X.
            var velocityX = Direction.x * _speed;
            float velocityY;

            velocityY = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(velocityX, velocityY);

            UpdateAnimatorVals();
            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var velocityY = _rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (PlayerAttachedToRope)
            {
                // Чтобы hero не падал быстро после detach.
                velocityY = _rigidbody.velocity.y / 100f;
                return velocityY;
            }

            if (_isGrounded)
            {
                _allowDoubleJump = true;

                if (_fallIsLongEnough)
                {
                    SpawnFallParticles();
                    _fallIsLongEnough = false;
                }
            }

            if (isJumpPressing)
            {
                velocityY = CalculateJumpVelocity(velocityY);
            }

            else if (_rigidbody.velocity.y > 0)
            {
                velocityY *= 0.5f;
            }

            // Падение с двойного прыжка имеет velocity около -15.
            if (_rigidbody.velocity.y < -15)
            {
                _fallIsLongEnough = true;
            }

            // Гравитация меняется при падении.
            _rigidbody.gravityScale = _rigidbody.velocity.y >= 0 ? _gravityScale : _fallingGravityScale;

            return velocityY;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                SpawnJumpDustParticles();
                yVelocity += _jumpSpeed;
            }
            else if (_allowDoubleJump)
            {
                SpawnJumpDustParticles();
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
            }

            return yVelocity;
        }

        private void UpdateAnimatorVals() 
        {
            _animator.SetBool(IsGround, _isGrounded);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
            _animator.SetBool(IsRunning, Direction.x != 0);
            _animator.SetBool(RopeAttached, PlayerAttachedToRope);
        }

        private void UpdateSpriteDirection()
        {
            if (Direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (Direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }



#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Handles.color = _isGrounded ? Color.green : Color.red;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, 0.09f);
        }
#endif

        public void SaySomething()
        {
            Debug.Log("Something");
        }


        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

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



        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        public void SpawnJumpDustParticles()
        {
            _jumpDustParticles.Spawn();
        }

        public void SpawnFallParticles()
        {
            _fallDustParticles.Spawn();
        }
    }
}
