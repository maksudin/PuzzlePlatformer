using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    [SerializeField] private Transform _playertr;
    [SerializeField] private float dotPB;
    [Range(0f, 1f)]
    [SerializeField] private float canSeeTreshold;
    [SerializeField] private bool playerLookingAtB;

    private void OnDrawGizmos()
    {
        var center = transform.position;
        var playerPos = _playertr.position;
        var playerLookDir = _playertr.right;
        var playetTriggerDir = (center - playerPos).normalized;

        dotPB = Vector2.Dot(playetTriggerDir, playerLookDir);
        playerLookingAtB = dotPB >= canSeeTreshold;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerPos, playerPos + playerLookDir);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(playerPos, playerPos + playetTriggerDir);
    }
}