using UnityEngine;

public class PlayerStunnedBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;
    protected PlayerMovement movementScript;
    protected Animator animator;
    protected PlayerSounds soundScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    [Header("Stats")]
    protected float stunnedTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.statsScript.SetSprintMode(false);
    }

    protected void LoadReferences(Animator animator)
    {
        // Stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerStunnedBehavior of " + name);
        // Movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerStunnedBehavior of " + name);
        // Sound script
        this.soundScript = animator.GetComponentInChildren<PlayerSounds>();
        if (this.soundScript == null)
            Debug.LogError("Can't find sound script for PlayerStunnedBehavior of " + name);
        // Animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.statsScript.controlable = false;
        this.statsScript.stunnedable = false;
        this.statsScript.enduranceRestoreable = false;
        this.stunnedTimer = 0f;
        this.movementScript.StopMoving();
        this.soundScript.PlayRandomHurtSound(); 
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.CheckStunnedEnd();
    }

    protected void CheckStunnedEnd()
    {
        this.stunnedTimer += Time.deltaTime;
        if (this.stunnedTimer >= this.statsScript.stunTime)
            this.animator.SetTrigger("endState");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.controlable = true;
        this.statsScript.stunnedable = true;
        this.statsScript.enduranceRestoreable = true;
        this.statsScript.SetCurrentEnduranceValue(this.statsScript.maxEndurance / 2);
        this.animator.ResetTrigger("endState");
    }
}
