using UnityEngine;

public class PlayerRunBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected Animator animator;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
    }

    protected void LoadReferences(Animator animator)
    {
        // movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerIdleBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.WaitJumpInput();
        this.movementScript.CheckFalling();
        this.movementScript.CheckRunning();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isRunning", false);
    }
}
