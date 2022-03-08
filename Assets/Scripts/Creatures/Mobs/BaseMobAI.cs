using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Patrolling;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace PixelCrew.Creatures.Mobs
{
    public class BaseMobAI : MonoBehaviour
    {
        [SerializeField] protected LayerCheck Vision;

        protected Coroutine CurrentCoroutine;
        protected GameObject Target;
        protected Animator Animator;
        protected SpawnListComponent Particles;

        protected static readonly int IsDeadKey = Animator.StringToHash("is_dead");
        protected bool IsDead;

        protected virtual void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void StartState(IEnumerator coroutine)
        {
            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);

            CurrentCoroutine = StartCoroutine(coroutine);
        }
    }
}