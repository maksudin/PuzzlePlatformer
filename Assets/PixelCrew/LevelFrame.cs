using UnityEditor;
using UnityEngine;

public class LevelFrame : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void OnDrawGizmos()
    {
        if (_camera == null) return;

        float height = 2f * _camera.orthographicSize;
        float width = height * _camera.aspect;

        Handles.color = Color.red;
        Handles.DrawWireCube(transform.position, new Vector3(width, height, 1));
    }

}
