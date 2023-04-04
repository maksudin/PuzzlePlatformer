using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.SpriteManagement
{
    [Serializable]
    public class AnimationState
    {
        public string StateName;
        public bool Loop;
        public Sprite[] Sprites;
        public UnityEvent OnComplete;
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class StatesSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private bool _allowNext;
        [SerializeField] private string _startState;
        [SerializeField] private AnimationState[] _animationStates;
        private int _currentAnimationIndex, _currentSpriteIndex;
        private SpriteRenderer _renderer;
        private float _secondsPerFrame, _nextFrameTime;
        private bool _loop;
        private Sprite[] _sprites;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            SetClip(_startState);
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentSpriteIndex = 0;
        }


        private int FindAnimationIndex(string stateName)
        {
            for (int i = 0; i < _animationStates.Length; i++)
                if (_animationStates[i].StateName == stateName)
                    return i;

            return -1;
        }


        public void SetClip(string name)
        {
            _currentAnimationIndex = FindAnimationIndex(name);
            _currentSpriteIndex = 0;
            var state = _animationStates[_currentAnimationIndex];
            _loop = state.Loop;
            _sprites = state.Sprites;

            if (enabled == false)
                enabled = true;
        }


        private void NextAnimation()
        {
            if (_currentAnimationIndex >= _animationStates.Length)
                _currentAnimationIndex = 0;

            _currentAnimationIndex++;
            var state = _animationStates[_currentAnimationIndex];
            SetClip(state.StateName);
        }

        private void CompleteEvent()
        {
            var state = _animationStates[_currentAnimationIndex];
            state.OnComplete?.Invoke();
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            // Анимация закончилась.
            if (_currentSpriteIndex >= _sprites.Length)
            {
                if (_loop)
                    _currentSpriteIndex = 0;

                else  
                {
                    if (_allowNext)
                        NextAnimation();
                    else
                    {
                        enabled = false;
                        CompleteEvent();
                    }

                    _currentSpriteIndex = 0;
                    return;
                }
            }

            _renderer.sprite = _sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }
    }

}

