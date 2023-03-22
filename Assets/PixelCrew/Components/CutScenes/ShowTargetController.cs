using Cinemachine;
using UnityEngine;

namespace Assets.PixelCrew.Components.CutScenes
{
    public class ShowTargetController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineVirtualCamera _camera;
        private static readonly int ShowKey = Animator.StringToHash("show");

        public void SetState(bool isShown) => _animator.SetBool(ShowKey, isShown);

        public void SetPosition(Vector3 targetPosition)
        {
            targetPosition.z = _camera.transform.position.z;
            _camera.transform.position = targetPosition;
        }

    }
}