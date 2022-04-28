using PixelCrew.Components.UI.Widgets;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class HealthbarComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private RectTransform _barTransform;
        private ProgressBarWidget _healthBarWidget;

        private void Awake()
        {
            _healthBarWidget = GetComponent<ProgressBarWidget>();
        }

        private void Update()
        {
            var targetPosition = Camera.main.WorldToViewportPoint(transform.position);
            var screenPosition = new Vector2(
                (targetPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f),
                (targetPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)

            );

            //screenPosition += _offset;

            _barTransform.anchoredPosition = targetPosition;
        }
    }
}