using UnityEngine;

public class PlayerIdleBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected Animator animator;

    [Header("States")]
    protected bool isLoadedReferences = false;
    protected bool isCheckedOnGround = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
    }

    protected void LoadReferences(Animator animator)
    {
        // movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerIdleBehavior of " + name);
        // stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerIdleBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.isCheckedOnGround = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isCheckedOnGround)
            this.CheckOnGround();
        this.movementScript.WaitJumpInput();
        this.movementScript.CheckFalling();
        this.movementScript.CheckRunning();
    }

    protected void CheckOnGround()
    {
        if (this.statsScript.isOnGround)
        {
            this.statsScript.rollable = true;
            this.movementScript.jumpTakenAmount = 0;

            this.isCheckedOnGround = true;
        }
    }
}
