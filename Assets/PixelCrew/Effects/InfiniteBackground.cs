using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class InfiniteBackground : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _container;

        private Bounds _containerBounds;
        private Bounds _allBounds;

        private Vector3 _boundsToTransformDelta;
        private Vector3 _containerDelta;

        private void Start()
        {
            var sprites = _container.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                _containerBounds.Encapsulate(sprite.bounds);
            }

            _allBounds = _containerBounds;

            _boundsToTransformDelta = transform.position - _allBounds.center;
            _containerDelta = _container.position - _allBounds.center;
        }


        private void OnDrawGizmosSelected()
        {
            GizmosUtils.DrawBounds(_allBounds, Color.magenta);
        }
    }
}