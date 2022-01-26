using UnityEngine;


namespace PixelCrew
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float offsetX;
        [SerializeField] private float offsetY;
        [SerializeField]  private float _damping;

        private void LateUpdate()
        {
            var destination = new Vector3(_target.position.x + offsetX, _target.position.y + offsetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * _damping);
        }
    }
}
