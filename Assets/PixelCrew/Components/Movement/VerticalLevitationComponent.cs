using UnityEngine;

namespace PixelCrew.Components.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _frequency = 5f;
        [SerializeField] private float _amplitude = 0.1f;
        [SerializeField] private bool _randomize;
        private float _seed;

        private float _originalY;
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _originalY = _rigidbody.transform.position.y;
            if (_randomize)
                _seed = Random.value * Mathf.PI * 2;

        }

        private void Update()
        {
            var pos = _rigidbody.position;
            pos.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
            _rigidbody.MovePosition(pos);
        }
    }
}