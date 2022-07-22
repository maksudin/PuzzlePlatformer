using Assets.PixelCrew.Components.GoBased;
using UnityEngine;

public class BossShootState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.LaunchProjectiles();
    }
}
