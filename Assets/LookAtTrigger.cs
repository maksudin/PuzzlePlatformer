#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    [SerializeField] private Transform _playertr;
    [SerializeField] private float dotPB;
    [SerializeField, Range(0f, 90f)] private float angThresholdDeg = 30f;
    [SerializeField] private bool playerLookingAtTrigger;

    private void OnDrawGizmos()
    {
        var center = transform.position;
        var playerPos = _playertr.position;
        var playerLookDir = _playertr.up;
        var playetToTriggerDir = (center - playerPos).normalized;

        dotPB = Vector2.Dot(playetToTriggerDir, playerLookDir);
        dotPB = Mathf.Clamp(dotPB, -1, 1);
        var angRad = Mathf.Acos(dotPB);
        float angThreshRad = angThresholdDeg * Mathf.Deg2Rad;

        playerLookingAtTrigger = angRad < angThreshRad;

        Gizmos.color = playerLookingAtTrigger? Color.green : Color.red;
        Gizmos.DrawLine(playerPos, playerPos + playerLookDir);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(playerPos, playerPos + playetToTriggerDir);

        Handles.DrawSolidArc(playerPos, Vector3.forward, playetToTriggerDir, angThresholdDeg, 1f);
        Handles.DrawSolidArc(playerPos, Vector3.forward, playetToTriggerDir, -angThresholdDeg, 1f);
    }
}
#endif