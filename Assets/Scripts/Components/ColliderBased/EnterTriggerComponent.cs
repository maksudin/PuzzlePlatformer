using UnityEngine;
using System;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var IsInLayer = (_layer.value & (1 << collision.transform.gameObject.layer)) > 0;
            if (!IsInLayer && _layer != 0) return;
            if (!string.IsNullOrEmpty(_tag) && !collision.gameObject.CompareTag(_tag)) return;

            _action?.Invoke(collision.gameObject);
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {
    }

}
