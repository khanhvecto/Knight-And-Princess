using UnityEngine;

public class KnightBossNormalBehavior: StateMachineBehaviour
{
    private KnightBossMove moveScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveScript = animator.GetComponentInChildren<KnightBossMove>();
        if (this.moveScript == null) Debug.Log("Can't find KnightBossMove Script for KnightBossNormalBehavior in " + animator.name);

        if (this.moveScript != null) this.moveScript.Stop();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}