using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.SpriteManagement
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField]
        private bool _loop,
                     _randomize;
        
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame, _nextFrameTime, _seed;
        private int _currentSpriteIndex;

        private void Start()
        {
            if (_randomize)
                _seed = Random.value;
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame + _seed;
            _currentSpriteIndex = 0;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _sprites.Length)
            {
                if (_loop)
                    _currentSpriteIndex = 0;
                else
                {
                    enabled = false;
                    _onComplete.Invoke();
                    return;
                }
            }

            _renderer.sprite = _sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }
    }
}

