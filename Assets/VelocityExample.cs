using UnityEngine;

public class VelocityExample : MonoBehaviour
{
    private Vector3 velocity = Vector3.right;

    private void Update()
    {
        Vector3 velMetersPerFrame = Time.deltaTime * velocity;
        transform.position += velMetersPerFrame;
    }
}
