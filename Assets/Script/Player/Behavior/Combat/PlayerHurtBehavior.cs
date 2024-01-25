using UnityEngine;

public class PlayerHurtBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;
    protected Animator animator;
    protected PlayerMovement movementScript;
    protected PlayerSounds hurtSoundScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.movementScript.StopMoving();
        this.hurtSoundScript.PlayRandomHurtSound();
        this.statsScript.SetSprintMode(false);
        this.CheckDead();
    }

    protected void LoadReferences(Animator animator)
    {
        // stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerHurtBehavior of " + name);
        // movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerHurtBehavior of " + name);
        // Hurt sound script
        this.hurtSoundScript = animator.GetComponentInChildren<PlayerSounds>();
        if (this.hurtSoundScript == null)
            Debug.LogError("Can't find hurt sound script for PlayerHurtBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.statsScript.stunnedable = false;
        this.statsScript.controlable = false;
        this.statsScript.enduranceRestoreable = false;
    }

    protected void CheckDead()
    {
        if (this.statsScript.CurrentHealth == this.statsScript.minHealth)
            this.animator.SetTrigger("dead");
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
        this.statsScript.SetCurrentEnduranceValue(this.statsScript.maxEndurance);
    }
}
