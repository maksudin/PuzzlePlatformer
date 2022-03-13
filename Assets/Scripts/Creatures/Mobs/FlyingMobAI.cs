using System;
using System.Collections;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using static Cinemachine.CinemachineCore;
using UnityEngine.InputSystem.HID;
using PixelCrew.Creatures.Mobs.Patrolling;

namespace PixelCrew.Creatures.Mobs
{
    public class FlyingMobAI : BaseMobAI
    {
        private FlyingCreature _creature;
        [SerializeField] private float _scanRadius;
        [SerializeField] private float _scanCooldown;
        [SerializeField] private float _missHeroCooldown = 0.2f;
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer;
        private bool _isAwake = false;

        protected override void Awake()
        {
            base.Awake();
            _creature = GetComponent<FlyingCreature>();
        }

        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _scanRadius);
        }

        public void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;
            Target = go;
            StartState(Scan());
        }

        private IEnumerator Scan()
        {
            yield return new WaitForSeconds(_scanCooldown);
            while (Vision.IsTouchingLayer)
            {
                var direction = ((Vector2)(Target.transform.position - transform.position)).normalized;
                var hit = Physics2D.Raycast(transform.position, direction, _scanRadius, _layer);
                
                if (hit.collider != null)
                {
                    //Debug.DrawLine(transform.position, hit.point);
                    if (hit.collider.CompareTag(_tag))
                    {
                        if (!_isAwake) StartState(AwakeMob());
                        else StartState(AgroToHero());
                    }
                }
                

                yield return new WaitForSeconds(_scanCooldown);
            }

            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
        }

        

        protected override void StartState(IEnumerator coroutine)
        {
            _creature.Direction = Vector3.zero;
            base.StartState(coroutine);
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(new Vector2(direction.x, direction.y));
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = Target.transform.position - transform.position;
            return direction.normalized;
        }

        private void LookAtHero()
        {
            Vector2 direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        public void OnAwake()
        {
            _isAwake = true;
            StartState(AgroToHero());
        }

        private IEnumerator AwakeMob()
        {
            _creature.AwakeFromSleep();
            yield return null;
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            yield return null;
            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            while (Vision.IsTouchingLayer)
            {
                SetDirectionToTarget();
                yield return null;
            }

            _creature.Direction = Vector2.zero;
            yield return new WaitForSeconds(_missHeroCooldown);

            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
        }

    }
}