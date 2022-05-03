using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PixelCrew.Utils
{
    public class CheatController : MonoBehaviour
    {
        private string _currentInput;
        [SerializeField] private float _inputTimeToLive;
        [SerializeField] private CheatItem[] _cheats;

        private float _inputTime;

        private void Awake()
        {
            // Подписываемся
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnDestroy()
        {
            // Отписываемся от события.
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char inputChar)
        {
            // Обработка введенных символов
            _currentInput += inputChar;
            _inputTime = _inputTimeToLive;

            FindAnyCheats();
        }

        private void FindAnyCheats()
        {
            foreach (CheatItem cheatItem in _cheats)
            {
                if (_currentInput.Contains(cheatItem.Name))
                {
                    cheatItem.Action.Invoke();
                    _currentInput = string.Empty;
                }
            }
        }

        private void Update()
        {
            if (_inputTime < 0)
            {
                _currentInput = string.Empty;
            }
            else
            {
                _inputTime -= Time.deltaTime;
            }
        }
    }

    [Serializable]
    public class CheatItem
    {
        public string Name;
        public UnityEvent Action;
    }
}

