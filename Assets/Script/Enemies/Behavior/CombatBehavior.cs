using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBehavior : StateMachineBehaviour
{
    private CombatMovement movementScript;
    private EnemyCombat combatScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementScript = animator.GetComponentInChildren<CombatMovement>();
        combatScript = animator.GetComponentInChildren<EnemyCombat>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementScript.move();
        if(movementScript.attackReady)
        {
            combatScript.tryAttack();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
