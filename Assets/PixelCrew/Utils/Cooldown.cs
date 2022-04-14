using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;

        private float _timeUp;

        public void Reset()
        {
            _timeUp = Time.time + _value;
        }

        public bool IsReady => _timeUp <= Time.time;
    }
}