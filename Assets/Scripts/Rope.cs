using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class Rope : MonoBehaviour
    {
        private LineRenderer _lineRender;
        [SerializeField] private Transform ropeAnchor;
        [SerializeField] private Hero _hero;

        [SerializeField] private Transform _heroTransform;
        private Rigidbody2D _ropeRigidBody;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _detachTime = 0.5f;
        [SerializeField] private float _attachTime = 0.5f;
        private float _detachTimer = 0.0f;
        private float _attachTimer = 0.0f;

        private bool _attachTimerStart;
        private bool _ropeActivated;

        private BoxCollider2D _ropeCollider;
        private float _offsetHeroPositionX = -0.04f;
        private float _offsetHeroPositionY = 0.4f;

        private void Awake()
        {
            _lineRender = GetComponent<LineRenderer>();
            _ropeRigidBody = GetComponent<Rigidbody2D>();
            _ropeCollider = GetComponent<BoxCollider2D>();
            _detachTimer = _detachTime;
            _attachTimer = _attachTime;

            _ropeActivated = false;
            _attachTimerStart = false;

        }

        public void ActivateRope()
        {
            _hero.AttachPlayerToRope();
            _ropeActivated = true;
            _ropeCollider.isTrigger = false;
            _hero.HeroCollider.enabled = false;
            // Чтобы собирать монетки когда герой на верёвке.
            // (Т.к. коллайдер hero отключён)
            gameObject.tag = "Player";
        }

        private void FixedUpdate()
        {
            if (_hero.PlayerAttachedToRope && _ropeActivated)
            {

                _ropeRigidBody.AddForce(new Vector2(_pushForce * _hero.Direction.x, 0), ForceMode2D.Impulse);

                // Если нажат прыжок.
                if (_hero.Direction.y > 0 && _detachTimer < 0)
                {
                    SetParamsToDefault();
                    _hero.DetachPlayerFromRope();
                    //_heroDetached?.Invoke();
                    return;
                }

            }
        }

        // Update is called once per frame
        void Update()
        {
            DrawLine();
            AttachTimeDelay();

            if (_ropeActivated)
            {
                if (_detachTimer > 0) _detachTimer -= Time.deltaTime;

                if (_hero.Direction.x > 0 && _offsetHeroPositionX > 0)
                {
                    _offsetHeroPositionX *= -1;
                }

                if (_hero.Direction.x < 0 && _offsetHeroPositionX < 0)
                {
                    _offsetHeroPositionX *= -1;
                }
                _heroTransform.position = new Vector3(transform.position.x + _offsetHeroPositionX,
                                                      transform.position.y + _offsetHeroPositionY,
                                                      transform.position.z);
            }
        }

        private void DrawLine()
        {
            _lineRender.SetPosition(0, ropeAnchor.position);
            _lineRender.SetPosition(1, transform.position);
        }

        private void AttachTimeDelay()
        {
            // Не даю hero сразу цепляться за верёвку снова.
            // Чтобы не мешало его полёту.
            if (_attachTimerStart)       
            {
                _attachTimer -= Time.deltaTime;

                if (_attachTimer <= 0)
                {
                    _ropeCollider.enabled = true;
                    _ropeCollider.isTrigger = true;
                    // Останавливаю и сбрасываю таймер.
                    _attachTimerStart = false;
                    _attachTimer = _attachTime;
                }
            }
        }

        private void SetParamsToDefault()
        {
            _attachTimerStart = true;
            _hero.HeroCollider.enabled = true;
            _ropeActivated = false;
            _ropeCollider.enabled = false;
            _detachTimer = _detachTime;

            gameObject.tag = "Untagged";
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(ropeAnchor.position, transform.position);
        }
        
    }

}

