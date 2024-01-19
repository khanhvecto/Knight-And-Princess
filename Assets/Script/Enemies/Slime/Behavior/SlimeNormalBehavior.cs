using UnityEngine;

public class SlimeNormalBehavior : StateMachineBehaviour
{
    private SlimeNormalMove moveScript;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //movementScript = animator.GetComponent<NormalMovement>();
        moveScript = animator.GetComponentInChildren<SlimeNormalMove>();
        moveScript.ResetNormalMoveRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveScript.Move();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}
