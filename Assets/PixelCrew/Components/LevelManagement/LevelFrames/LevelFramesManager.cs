using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace Assets.PixelCrew.Components.LevelManagement.LevelFrames
{
    public class LevelFramesManager : MonoBehaviour
    {
        [SerializeField] private LevelFrame[] _levelFrames;
        private LevelFrame _activeFrame;
        private Transform _heroTransform;
        private Camera _camera;

        private void Start()
        {
            _heroTransform = FindObjectOfType<Hero>().GetComponent<Transform>();
            _camera = FindObjectOfType<Camera>();
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
                if ( Mathf.Abs(heroFramePos.x) <= frame.Width / 2 && Mathf.Abs(heroFramePos.y) <= frame.Height / 2 )
                {
                    frame.IsActive = true;
                    _activeFrame = frame;
                    MoveCameraToActiveFrame();
                }
                else
                    frame.IsActive = false;
            }
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