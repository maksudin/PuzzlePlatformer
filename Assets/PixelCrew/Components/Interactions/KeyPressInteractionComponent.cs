using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PixelCrew.Components.Interactions
{
    public class KeyPressInteractionComponent : MonoBehaviour
    {
        [SerializeField] private OnKeyPressEvent _keyPressEvent;
        private bool _keyPressed;

        void OnGUI()
        {
            Event e = Event.current;
            if (_keyPressed) return;

            if (e.type == EventType.KeyDown && e.keyCode != KeyCode.None)
            {
                Debug.Log("Detected key code: " + e.keyCode);
                _keyPressEvent?.Invoke(e.keyCode);
                _keyPressed = true;
            }
        }
    }

    [Serializable]
    public class OnKeyPressEvent : UnityEvent<KeyCode> { }
}