using UnityEngine;

namespace PixelCrew
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _barrel;
        private Collider2D _collider;

        public bool IsTouchingLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer) || _collider.IsTouchingLayers(_barrel);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer) || _collider.IsTouchingLayers(_barrel);
        }
    }
}

