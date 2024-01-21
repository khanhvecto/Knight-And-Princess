using UnityEngine;

public class PlayerRunBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerSounds soundsScript;
    protected PlayerStats statsScript;
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
        // Sounds script
        this.soundsScript = animator.GetComponentInChildren<PlayerSounds>();
        if (this.soundsScript == null)
            Debug.LogError("Can't find sounds script for PlayerIdleBehavior of " + name);
        // Stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerIdleBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.PlaySound();
        this.movementScript.WaitJumpInput();
        this.movementScript.CheckFalling();
        this.movementScript.CheckRunning();
    }
    protected void PlaySound()
    {
        if (this.statsScript.isSprinting)
            this.soundsScript.PlayRandomSprintSound();
        else
            this.soundsScript.PlayRandomRunSound();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
        this.StopSound();
    }

    protected void ResetStats()
    {
        animator.SetBool("isRunning", false);
    }

    protected void StopSound()
    {
        if (this.statsScript.isSprinting)
            this.soundsScript.StopSprintSound();
        else
            this.soundsScript.StopRunSound();
    }
}
