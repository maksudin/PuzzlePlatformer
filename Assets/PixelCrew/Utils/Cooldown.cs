using System;
using PixelCrew.Utils.Disposables;
using UnityEngine;


namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;
        public event Action OnReset;
        private float _timesUp;

        public float Value {
            get => _value;
            set => _value = value; 
        }

        public void Reset()
        {
            OnReset?.Invoke();
            _timesUp = Time.time + _value;
        }

        public IDisposable Subscribe(Action call)
        {
            OnReset += call;
            return new ActionDisposable(() => OnReset -= call);
        }

        public float TimeLasts => Mathf.Max(_timesUp - Time.deltaTime, 0);

        public bool IsReady => _timesUp <= Time.time;
    }
}