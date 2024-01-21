using UnityEngine;

public class KnightBossReadyAttackBehavior : StateMachineBehaviour
{
    protected KnightBossMove movementScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.LoadReferences(animator);
        this.movementScript.ResetReadyTimer();
        this.movementScript.StopMoving();
    }

    protected void LoadReferences(Animator animator)
    {
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for KnightBossReadyAttackBehavior of " + animator.name);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.ReadyAttack();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
