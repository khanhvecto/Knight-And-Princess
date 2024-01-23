using UnityEngine;

public class KnightBossNormalBehavior: StateMachineBehaviour
{
    private KnightBossMove moveScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveScript = animator.GetComponentInChildren<KnightBossMove>();

        if (this.moveScript != null) this.moveScript.StopMoving();
    }
}