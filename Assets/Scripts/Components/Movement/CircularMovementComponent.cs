using System.Collections;
using System.Drawing;
using Assets.Scripts.Components.Movement;
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
        private PrefubInfo[] _prefubsInfo;

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

        private void OnEnable()
        {
            DestroyOldItems();

            float _degree = circleDegrees / count;
            float angle = _degree * Mathf.Deg2Rad;
            var _currentAngle = 0f;
            var point = transform.position;

            _prefubsInfo = new PrefubInfo[count];

            for (var i = 0; i < count; i++)
            {
                point.x = transform.position.x + (_radius * Mathf.Cos(_currentAngle));
                point.y = transform.position.y + (_radius * Mathf.Sin(_currentAngle));

                _prefubsInfo[i] = new PrefubInfo();
                _prefubsInfo[i].Go = (GameObject) Instantiate(_prefub, point, Quaternion.identity, transform);
                _prefubsInfo[i].Angle = _currentAngle;

                _currentAngle += angle;
            }

            foreach (var prefub in _prefubsInfo)
            {
                prefub.Go.GetComponent<VerticalLevitationComponent>().enabled = false;
            }
        }

        private void DestroyOldItems()
        {
            _itemsDestroy = GetComponentsInChildren<DestroyObjectComponent>();

            if (_itemsDestroy.Length > 1)
            {
                for (var i = 0; i < _itemsDestroy.Length; i++)
                {
                    _itemsDestroy[i].DestroyObject();
                }
            }
        }

        private void Update()
        {
            var countDestroyed = 0;
            foreach (var prefub in _prefubsInfo)
            {
                if (prefub.Go == null)
                {
                    countDestroyed++;
                    continue;
                }

                var point = prefub.Go.transform.position;
                var angle = prefub.Angle + 0.008f * _speed;

                point.x = transform.position.x + (_radius * Mathf.Cos(angle));
                point.y = transform.position.y + (_radius * Mathf.Sin(angle));

                prefub.Go.transform.position = new Vector3(point.x, point.y, transform.position.z);
                prefub.Angle = angle;
            }

            if (countDestroyed == count)
            {
                GetComponent<CircularMovementComponent>().enabled = false;
            }
        }

        public class PrefubInfo
        {
            public GameObject Go;
            public float Angle;
        }
    }
}