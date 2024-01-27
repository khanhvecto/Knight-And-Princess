using UnityEngine;

public class PlayerRunBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerSounds soundsScript;
    protected PlayerStats statsScript;
    protected Animator animator;
    protected ParticleSystem sprintEffect;

    [Header("States")]
    protected bool isLoadedReferences = false;
    protected bool isCheckedOnGround;

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
        this.sprintEffect = animator.transform.Find("Effects").Find("Dust").GetComponent<ParticleSystem>();

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
        this.CheckIfSprint();
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
        }

        this.isCheckedOnGround = true;
    }

    protected void CheckIfSprint()
    {
        if (this.statsScript.isSprinting)
        {
            this.sprintEffect.Play();
            this.soundsScript.PlayRandomSprintSound();
        }
        else
        {
            this.sprintEffect.Stop();
            this.soundsScript.PlayRandomRunSound();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
        this.StopSound();
    }

    protected void ResetStats()
    {
        animator.SetBool("isRunning", false);

        // Stop sound
        if (this.statsScript.isSprinting)
            this.soundsScript.StopSprintSound();
        else
            this.soundsScript.StopRunSound();

        // Stop effect
        this.sprintEffect.Stop();
    }

    protected void StopSound()
    {
        if (this.statsScript.isSprinting)
            this.soundsScript.StopSprintSound();
        else
            this.soundsScript.StopRunSound();
    }
}
