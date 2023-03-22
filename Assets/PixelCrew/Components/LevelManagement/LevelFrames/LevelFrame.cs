using System;
using UnityEditor;
using UnityEngine;

namespace Assets.PixelCrew.Components.LevelManagement.LevelFrames
{
    [Serializable]
    public class LevelFrame : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField, Range(1, 100)]
        private int FrameWidthUnits = 1,
                    FrameHeightUnits = 1;

        [NonSerialized] public float Width, Height;
        [NonSerialized] public bool IsActive = false;
        [NonSerialized] public PolygonCollider2D _polygonCollider;

        private void Start()
        {
            if (_camera == null) return;
            Height = FrameHeightUnits;
            Width = FrameWidthUnits;
            _polygonCollider = GetComponent<PolygonCollider2D>();
            if (_polygonCollider != null)
                _polygonCollider.points = GeneratePolygonVertices(Width, Height);
        }

        private Vector2[] GeneratePolygonVertices(float width, float height)
        {
            var vertices = new Vector2[4]
            {
                new Vector2(0, height),
                new Vector2(width, height),
                new Vector2(width, 0),
                new Vector2(0, 0)
            };

            return vertices;
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_camera == null) return;

            Height = FrameHeightUnits;
            Width = FrameWidthUnits;

            Handles.color = IsActive ? Color.green : Color.red;
            var cubePosition = CalculateFrameOffset(Width, Height);
            Handles.DrawWireCube(cubePosition, new Vector3( Width, Height, 1 ));
        }
#endif

        private Vector3 CalculateFrameOffset(float width, float height)
        {
            float xOffset, yOffset;
            xOffset = width / 2;
            yOffset = height / 2;

            return new Vector3(transform.position.x + xOffset,
                               transform.position.y + yOffset,
                               transform.position.z);
        }
    }
}