using UnityEngine;

public class RotationExample : MonoBehaviour
{
    private void Rotations()
    {
        // Relative.
        Quaternion rotationAdd = Quaternion.AngleAxis(90f, Vector3.up);

        // Absolute.
        Quaternion rot = Quaternion.LookRotation(transform.forward, Vector3.up);

        // "Add" rotation.
        Quaternion noot = rotationAdd * rot;

        // "Substract" rotation
        Quaternion noots = Quaternion.Inverse(rotationAdd) * rot;
    }
}
