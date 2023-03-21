using System;
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
        public event Action OnUpperFrameEntered;
        private bool _isActiveFrameAboveUs => _camera.transform.position.y < _activeFrame.transform.position.y;


        private void Start()
        {
            _heroTransform = FindObjectOfType<Hero>().GetComponent<Transform>();
            _camera = FindObjectOfType<CinemachineVirtualCamera>();
            _cameraConfiner = FindObjectOfType<CinemachineConfiner>();
        }

        private void Update()
        {
            UpdateFrameParams(_levelFrames);
        }

        private void UpdateFrameParams(LevelFrame[] levelFrames)
        {
            foreach (var frame in levelFrames)
            {
                var heroFramePos = frame.transform.InverseTransformPoint(_heroTransform.position);

                if (heroFramePos.x < 0 || heroFramePos.y < 0)
                    SetFrameParams(activeParams: false, frame);

                else if (heroFramePos.x <= frame.Width && heroFramePos.y <= frame.Height)
                    SetFrameParams(activeParams: true, frame);

                else
                    SetFrameParams(activeParams: false, frame);
            }
        }

        private void SetFrameParams(bool activeParams, LevelFrame frame)
        {
            if (activeParams)
            {
                if (frame.IsActive) return;
                frame.IsActive = true;
                _activeFrame = frame;
                ResetCameraParams();
                if (_isActiveFrameAboveUs)
                    OnUpperFrameEntered.Invoke();

                MoveCameraToActiveFrame();
                SetCameraParams();
                
            }
            else
            {
                frame.IsActive = false;
            }
        }


        private void SetCameraParams()
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
                _activeFrame.transform.position.x + _activeFrame.Width / 2,
                _activeFrame.transform.position.y + _activeFrame.Height / 2,
                _camera.transform.position.z
            );
        }
    }
}