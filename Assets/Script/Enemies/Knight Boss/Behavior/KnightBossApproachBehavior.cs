using UnityEngine;

public class KnightBossApproachBehavior : StateMachineBehaviour
{
    private KnightBossMove movementScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for KnightBossCombatBehavior of " + animator.name);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementScript.ApproachEnemy();
    }

    //protected void ApproachEnemy()
    //{
    //    if (!this.IsFarAwayEnemy(1f))
    //    {
    //        // Change behavior to ready attack
    //        this.stateScript.animator.SetBool("ready", true);
    //        return;
    //    }

    //    if (this.IsNeedToFlip())
    //        this.stateScript.Flip();

    //    this.MoveToEnemy();
    //}
}
