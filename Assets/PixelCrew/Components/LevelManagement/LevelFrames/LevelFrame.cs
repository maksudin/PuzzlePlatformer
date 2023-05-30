using System;
using UnityEditor;
using UnityEngine;

namespace Assets.PixelCrew.Components.LevelManagement.LevelFrames
{
    [Serializable]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class LevelFrame : MonoBehaviour
    {
        [SerializeField, Range(1, 100)]
        private int FrameWidthUnits = 1,
                    FrameHeightUnits = 1;

        public float Width => FrameWidthUnits;
        public float Height => FrameHeightUnits;
        [SerializeField] public bool IsActive = false;
        [NonSerialized] public PolygonCollider2D PolygonCollider;

        private void Start()
        {
            PolygonCollider = GetComponent<PolygonCollider2D>();
            if (PolygonCollider != null)
                PolygonCollider.points = GeneratePolygonVertices(Width, Height);
        }

        private Vector2[] GeneratePolygonVertices(float width, float height)
        {
            return new Vector2[4]
            {
                new Vector2(0, height),
                new Vector2(width, height),
                new Vector2(width, 0),
                new Vector2(0, 0)
            };
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = IsActive ? Color.green : Color.red;
            var cubePosition = CalculateFrameOffset(Width, Height);
            Handles.DrawWireCube(cubePosition, new Vector3( Width, Height, 1 ));
        }
#endif

        private Vector3 CalculateFrameOffset(float width, float height)
        {
            return new Vector3(transform.position.x + width / 2,
                               transform.position.y + height / 2,
                               transform.position.z);
        }
    }
}