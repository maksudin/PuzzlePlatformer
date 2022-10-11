using Cinemachine;
using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace Assets.PixelCrew.Components.LevelManagement.LevelFrames
{
    public class LevelFramesManager : MonoBehaviour
    {
        [SerializeField] private LevelFrame[] _levelFrames;
        private LevelFrame _activeFrame;
        private Transform _heroTransform;
        private CinemachineVirtualCamera _camera;
        private CinemachineConfiner _cameraConfiner;

        private void Start()
        {
            _heroTransform = FindObjectOfType<Hero>().GetComponent<Transform>();
            _camera = FindObjectOfType<CinemachineVirtualCamera>();
            _cameraConfiner = FindObjectOfType<CinemachineConfiner>();
        }

        private void Update()
        {
            ActivateHeroLevelFrame(_levelFrames);
        }

        private void ActivateHeroLevelFrame(LevelFrame[] levelFrames)
        {
            foreach (var frame in levelFrames)
            {
                var heroFramePos = frame.transform.InverseTransformPoint(_heroTransform.position);
                if (Mathf.Abs(heroFramePos.x) <= frame.Width / 2 && Mathf.Abs(heroFramePos.y) <= frame.Height / 2)
                    SetFrameParams(activeParams: true, frame);
                else
                    SetFrameParams(activeParams: false, frame);
            }
        }

        private void SetFrameParams(bool activeParams, LevelFrame frame)
        {
            if (activeParams && !frame.IsActive)
            {
                frame.IsActive = true;
                _activeFrame = frame;
                ResetCameraParams();
                MoveCameraToActiveFrame();
                SetFrameCameraParams();
                // Каждый фрейм почему то вызывается!
            }
            else
            {
                frame.IsActive = false;
            }
        }


        private void SetFrameCameraParams()
        {
            if (_activeFrame._polygonCollider == null || _cameraConfiner == null) return;
            
            _cameraConfiner.m_BoundingShape2D = _activeFrame._polygonCollider;
            _camera.Follow = _heroTransform;
        }

        private void ResetCameraParams()
        {
            if (_cameraConfiner == null) return;
            _cameraConfiner.m_BoundingShape2D = null;
            _camera.Follow = null;
        }

        private void MoveCameraToActiveFrame()
        {
            _camera.transform.position = new Vector3(
                _activeFrame.transform.position.x,
                _activeFrame.transform.position.y,
                _camera.transform.position.z
            );
        }
    }
}