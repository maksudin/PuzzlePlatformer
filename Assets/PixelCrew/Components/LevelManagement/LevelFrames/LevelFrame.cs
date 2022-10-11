using System;
using UnityEditor;
using UnityEngine;

namespace Assets.PixelCrew.Components.LevelManagement.LevelFrames
{
    [Serializable]
    public class LevelFrame : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float FrameWidthMultiplier = 1f;
        [SerializeField] private float FrameHeightMultiplier = 1f;
        [NonSerialized] public float Width;
        [NonSerialized] public float Height;
        [NonSerialized] public bool IsActive = false;
        [NonSerialized] public PolygonCollider2D _polygonCollider;

        private void Start()
        {
            if (_camera == null) return;
            Height = 2f * _camera.orthographicSize * FrameHeightMultiplier;
            Width = Height * _camera.aspect * FrameWidthMultiplier;
            _polygonCollider = GetComponent<PolygonCollider2D>();
            if (_polygonCollider != null)
                _polygonCollider.points = GeneratePolygonVertices(Width, Height);
        }

        private Vector2[] GeneratePolygonVertices(float width, float height)
        {
            var framePos = transform.TransformPoint(transform.position);
            var hw = width / 2;
            var hh = height / 2;
            var vertices = new Vector2[4]
            {
                new Vector2(-1 * hw, framePos.y + hh),
                new Vector2(hw, framePos.y + hh),
                new Vector2(hw, -1 * hh),
                new Vector2(-1 * hw, -1 * hh)
            };

            return vertices;
        }


        private void OnDrawGizmos()
        {
            if (_camera == null) return;

            Height = 2f * _camera.orthographicSize * FrameHeightMultiplier;
            Width = Height * _camera.aspect * FrameWidthMultiplier;

            Handles.color = IsActive ? Color.green : Color.red;
            Handles.DrawWireCube(transform.position, new Vector3(Width, Height, 1));
        }
    }
}