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

        private void Start()
        {
            if (_camera == null) return;
            Height = 2f * _camera.orthographicSize * FrameHeightMultiplier;
            Width = Height * _camera.aspect * FrameWidthMultiplier;
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