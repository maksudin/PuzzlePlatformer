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
        [SerializeField] private string _tag;
        [SerializeField] private string _ignoreTag;
        [SerializeField] private Vector2 _playerAttackOffset;
        [SerializeField] private LayerMask _layer;

        private RaycastHit2D[] _results = new RaycastHit2D[25];
        private Collider2D _collider;

        protected override void Awake()
        {
            base.Awake();
            _creature = GetComponent<FlyingCreature>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _scanRadius);
        }

        private void Start()
        {
            StartState(Scan());
        }

        //public void OnHeroInVision(GameObject go)
        //{
        //    if (IsDead) return;
        //    Target = go;
        //    StartState(Scan());
        //}

        private IEnumerator Scan()
        {
            while (enabled)
            {
                //var filter = new ContactFilter2D() {layerMask = _layer , useLayerMask = true };
                var num = _collider.Raycast(Target.transform.position, _results, _scanRadius);
                Debug.DrawLine(transform.position, Target.transform.position, HandlesUtils.TransparentGreen);

                if (num != 0)
                {
                    foreach (var result in _results)
                    {
                        if (result.collider.CompareTag(_tag))
                        {
                            //Debug.DrawLine(Target.transform.position, transform.position, HandlesUtils.TransparentGreen, 0.5f);
                            StartState(AgroToHero());
                        }
                    }
                }

                yield return new WaitForSeconds(_scanCooldown);
            }
            
        }

        protected override void StartState(IEnumerator coroutine)
        {
            _creature.Direction = Vector3.zero;
            base.StartState(coroutine);
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.Direction = new Vector2(direction.x + _playerAttackOffset.x, direction.y + _playerAttackOffset.y);
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
            _creature.AwakeFromSleep();
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            //_particles.Spawn("Exclamation");
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
            //Particles.Spawn("Miss");
            //yield return new WaitForSeconds(_missHeroCooldown);
            //StopCoroutine(CurrentCoroutine);
        }

    }
}