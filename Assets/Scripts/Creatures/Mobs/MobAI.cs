using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PixelCrew.Components;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Creatures.Mobs.Patrolling;

namespace PixelCrew.Creatures.Mobs
{
    public class MobAI : BaseMobAI
    {
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 0.5f;

        private Creature _creature;
        private Patrol _patrol;

        protected override void Awake()
        {
            base.Awake();
            _creature = GetComponent<Creature>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;
            Target = go;
            StartState(AgroToHero());
        }

        public IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        private void LookAtHero()
        {
            Vector2 direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        private IEnumerator GoToHero()
        {
            while(Vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }

            _creature.Direction = Vector2.zero;
            Particles.Spawn("Miss");
            yield return new WaitForSeconds(_missHeroCooldown);
            StartState(_patrol.DoPatrol());
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(new Vector2(direction.x, direction.y));
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = Target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public void OnDie()
        {
            IsDead = true;
            Animator.SetBool(IsDeadKey, true);
            _creature.Direction = Vector3.zero;

            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
        }

    }
}