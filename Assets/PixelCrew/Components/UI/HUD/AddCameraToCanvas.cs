using UnityEngine;

namespace PixelCrew.Components.UI.HUD
{
    public class AddCameraToCanvas : MonoBehaviour
    {
        private Canvas _canvas;
        [SerializeField] private string _layerName;

        private void Start()
        {
            var camera = FindObjectOfType<Camera>();
            _canvas = GetComponent<Canvas>();

            if (camera == null || _canvas == null) return;

            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = camera;
            _canvas.sortingLayerName = _layerName;
        }
    }
}