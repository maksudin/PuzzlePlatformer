using UnityEngine;
using PixelCrew.Creatures.Hero;
using PixelCrew.Utils;

namespace PixelCrew.Components
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private Transform ropeAnchor;
        [SerializeField] private float _pushForce = 0.15f;
        [SerializeField] private Cooldown _attachCooldown, _detachCooldown;
        private BoxCollider2D _ropeCollider;
        private LineRenderer _lineRenderer;
        private Hero _hero;
        private Transform _heroTransform;
        private Rigidbody2D _ropeRigidBody;
        private bool _ropeActivated;
        private float _offsetHeroPositionX = -0.04f,
                      _offsetHeroPositionY = 0.4f;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _ropeRigidBody = GetComponent<Rigidbody2D>();
            _ropeCollider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
            _heroTransform = _hero.transform;
        }

        public void ActivateRope()
        {
            if (_hero.AttachedToRope && _attachCooldown.IsReady) 
                return;

            _hero.AttachPlayerToRope();
            _ropeCollider.isTrigger = false;
            _ropeActivated = true;
            _detachCooldown.Reset();
        }

        private void FixedUpdate()
        {
            if (!_hero.AttachedToRope || !_ropeActivated) return;

            _ropeRigidBody.AddForce(new Vector2(_pushForce * _hero.Direction.x, 0), ForceMode2D.Impulse);

            // Нажат прыжок.
            if (_hero.Direction.y > 0)
            {
                if (!_detachCooldown.IsReady) return;

                _hero.DetachPlayerFromRope();
                _ropeCollider.isTrigger = true;
                _ropeActivated = false;
                _attachCooldown.Reset();
            }
        }

        void Update()
        {
            DrawLine();
            if (!_hero.AttachedToRope || !_ropeActivated)
                return;

            if (_hero.Direction.x > 0 && _offsetHeroPositionX > 0)
                _offsetHeroPositionX *= -1;

            if (_hero.Direction.x < 0 && _offsetHeroPositionX < 0)
                _offsetHeroPositionX *= -1;

            _heroTransform.position = new Vector3(transform.position.x + _offsetHeroPositionX,
                                                  transform.position.y + _offsetHeroPositionY,
                                                  transform.position.z);
        }

        private void DrawLine()
        {
            _lineRenderer.SetPosition(0, ropeAnchor.position);
            _lineRenderer.SetPosition(1, transform.position);
        }

        private void OnDrawGizmos() =>
            Gizmos.DrawLine(ropeAnchor.position, transform.position);
    }
}

