using Assets.PixelCrew.Components.GoBased;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss
{
    public class BossNextStageState : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.Stage++;

            var changeLight = animator.GetComponent<ModifyLights>();
            changeLight.SetColor();
        }
    }
}