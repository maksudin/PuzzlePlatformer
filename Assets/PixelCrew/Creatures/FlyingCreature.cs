using PixelCrew.Components.Audio;
using PixelCrew.Components.Health;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FlyingCreature: MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed;
        [SerializeField] protected int Damage;
        public Vector2 Direction;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected HealthComponent HealthComp;
        protected Rigidbody2D Rigidbody;
        private static readonly int IsAwake = Animator.StringToHash("awake");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            HealthComp = GetComponent<HealthComponent>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        protected virtual void FixedUpdate()
        {
            var velocityX = Direction.x * _speed;
            var velocityY = Direction.y * _speed;
            Rigidbody.velocity = new Vector2(velocityX, velocityY);
            
            UpdateSpriteDirection(Direction);
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
                transform.localScale = new Vector3(multiplier, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
        }

        public void SetDirection(Vector2 direction) => Direction = direction.normalized;
        public void AwakeFromSleep() => Animator.SetBool(IsAwake, true);
    }
}