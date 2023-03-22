using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class HealthbarComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private RectTransform _canvasRect, _barTransform;

        private void Update()
        {
            var targetPosition = Camera.main.WorldToViewportPoint(transform.position);
            _barTransform.anchoredPosition = targetPosition;
        }
    }
}