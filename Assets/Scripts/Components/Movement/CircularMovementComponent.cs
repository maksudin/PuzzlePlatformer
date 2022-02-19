using System.Collections;
using System.Drawing;
using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    public class CircularMovementComponent : MonoBehaviour
    {
        [SerializeField] private int count;
        [SerializeField] private float _radius;
        [SerializeField] private float _speed;
        [SerializeField] private GameObject _prefub;

        private DestroyObjectComponent[] _itemsDestroy;
        private Rigidbody2D[] _itemsRB;

        const float circleDegrees = 360f;

        private void OnDrawGizmos()
        {
            float _degree = circleDegrees / count;
            float angle = _degree * Mathf.Deg2Rad;
            var _currentDegree = 0f;
            var point = transform.position;

            for (var i = 0; i < count; i++)
            {
                point.x = transform.position.x + (_radius * Mathf.Cos(_currentDegree));
                point.y = transform.position.y + (_radius * Mathf.Sin(_currentDegree));

                _currentDegree += angle;

                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        private void Awake()
        {
            _itemsDestroy = GetComponentsInChildren<DestroyObjectComponent>();

            if (_itemsDestroy.Length > 1)
            {
                for (var i = 1; i < _itemsDestroy.Length; i++)
                {
                    _itemsDestroy[i].DestroyObject();
                }
            }
        }

        private void OnEnable()
        {
            float _degree = circleDegrees / count;
            float angle = _degree * Mathf.Deg2Rad;
            var _currentAngle = 0f;
            var point = transform.position;

            for (var i = 0; i < count; i++)
            {
                point.x = transform.position.x + (_radius * Mathf.Cos(_currentAngle));
                point.y = transform.position.y + (_radius * Mathf.Sin(_currentAngle));
                
                _currentAngle += angle;

                Instantiate(_prefub, point, Quaternion.identity, transform);
            }

            _itemsRB = GetComponentsInChildren<Rigidbody2D>();
            foreach (var itemRB in _itemsRB)
            {
                itemRB.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        private void Update()
        {
            foreach (var itemRB in _itemsRB)
            {
                GetDegree(transform.position, itemRB.transform.position);
                //itemRB.MovePosition()
            }
        }

        private float GetDegree(Vector2 point1, Vector2 point2)
        {
            var degree = 0f;
            var x = point2.x - point1.x;
            var y = point2.y - point1.y;

            if (x == 0) return y > 0 ? 90 : 270;
            if (y == 0) return x > 0 ? 0 : 180;

            degree = Mathf.Atan(y/x) * Mathf.Rad2Deg;
            degree = degree > 0 ? degree : degree + 180;

            return degree;

        }

    }
}