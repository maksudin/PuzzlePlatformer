﻿using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class SinucidalProjectile : BaseProjectile
    {
        [SerializeField]
        private float _frequency = 1f,
                      _amplitude = 1f;

        private float _originalY, _time;

        protected override void Start()
        {
            base.Start();
            _originalY = Rigidbody.position.y;
        }

        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += Direction * Speed;
            position.y = _originalY + Mathf.Sin(_time * _frequency) * _amplitude;
            Rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}