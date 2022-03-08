using System;
using System.Collections;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using static Cinemachine.CinemachineCore;
using UnityEngine.InputSystem.HID;

namespace PixelCrew.Creatures.Mobs
{
    public class FlyingMobAI : BaseMobAI
    {
        private FlyingCreature _creature;
        [SerializeField] private float _scanRadius;
        [SerializeField] private float _scanCooldown;
        [SerializeField] private string[] _tags;
        private RaycastHit2D[] _results = new RaycastHit2D[10];

        protected override void Awake()
        {
            base.Awake();
            _creature = GetComponent<FlyingCreature>();
        }

        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.HardlyVisibleGreen;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _scanRadius);
        }

        private void Start()
        {
            StartState(Scan());
        }

        private IEnumerator Scan()
        {
            while (enabled)
            {
                var num = Physics2D.Raycast(transform.position, _creature.Direction, new ContactFilter2D(), _results, _scanRadius);

                if (num != 0)
                {
                    foreach (var result in _results)
                    {
                        if (result.collider.CompareTag(tag))
                        {
                            Debug.DrawLine(transform.position, result.point, HandlesUtils.TransparentGreen, 0.5f);
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
            _creature.Direction = direction;
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = Target.transform.position - transform.position;
            return direction.normalized;
        }
        

        //public void OnHeroInVision(GameObject go)
        //{
        //    if (IsDead) return;
        //    Target = go;
        //    StartState(AgroToHero());
        //}

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
            SetDirectionToTarget();
            yield return null;

            _creature.Direction = Vector2.zero;
            //_particles.Spawn("Miss");
            //yield return new WaitForSeconds(_missHeroCooldown);
            //StartState(_patrol.DoPatrol());
        }

    }
}