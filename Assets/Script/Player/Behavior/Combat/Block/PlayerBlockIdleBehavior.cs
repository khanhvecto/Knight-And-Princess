using UnityEngine;

public class PlayerBlockIdleBehavior : PlayerBlockBehavior
{
    protected override void SetStats()
    {
        base.statsScript.isBlocking = true;
        base.statsScript.movable = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        this.movementScript.StopMoving();
    }
}
