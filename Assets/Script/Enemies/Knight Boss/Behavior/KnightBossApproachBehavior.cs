using UnityEngine;

public class KnightBossApproachBehavior : StateMachineBehaviour
{
    private KnightBossMove movementScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for KnightBossCombatBehavior of " + animator.name);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementScript.ApproachEnemy();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
