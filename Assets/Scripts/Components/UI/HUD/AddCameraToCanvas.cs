using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.UI.HUD
{
    public class AddCameraToCanvas : MonoBehaviour
    {
        private Canvas _canvas;

        private void Start()
        {
            var camera = FindObjectOfType<Camera>();
            _canvas = GetComponent<Canvas>();

            if (camera == null || _canvas == null) return;

            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = camera;
            _canvas.sortingLayerName = "Health Bar";
        }
    }
}