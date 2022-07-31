using System;
using System.Linq;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.PixelCrew.Components.ColliderBased
{
    public class CheckRaycastHit : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnRaycastHitEvent _hitEvent;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentGreen;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif

        public void Check(GameObject target)
        {
            var direction = ((Vector2)(target.transform.position - transform.position)).normalized;
            var hit = Physics2D.Raycast(transform.position, direction, _radius, _mask);

            if (hit.collider != null)
            {
                bool isInTags = _tags.Any(tag => hit.collider.CompareTag(tag));
                if (isInTags)
                    _hitEvent?.Invoke(hit.collider.gameObject);
            }
        }

        [Serializable]
        public class OnRaycastHitEvent : UnityEvent<GameObject> {}
    }
}