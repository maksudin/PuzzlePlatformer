using Assets.PixelCrew.Components.GoBased;
using Assets.PixelCrew.Creatures.Mobs.Boss;
using UnityEngine;

public class BossNextStageState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        if (spawner != null)
            spawner.Stage++;

        var changeLight = animator.GetComponent<ModifyLights>();
        if (changeLight != null)
            changeLight.SetColor();
    }
}
