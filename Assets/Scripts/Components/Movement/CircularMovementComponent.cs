using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    [ExecuteInEditMode]
    public class CircularMovementComponent : MonoBehaviour
    {
        private Transform[] _items;
        [SerializeField] private float _radius;
        [SerializeField] private float _speed;
        const float circleDegrees = 360f;
        private float _degree;
        private float _itemX, _itemY;

        private void Awake()
        {
            _items = GetComponentsInChildren<Transform>();
        }

        private void OnEnable()
        {
            _degree = circleDegrees / _items.Length;
        }

        private void Update()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                _degree += _degree;
                _itemX = _radius * Mathf.Cos(_degree);
                _itemX = _radius * Mathf.Sin(_degree);
                _items[i].position = new Vector3(_itemX, _itemY, _items[i].position.z);
            }
        }


    }
}