using UnityEngine;

public class PlayerBlockBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;
    protected PlayerCombat combatScript;
    protected PlayerMovement movementScript;
    protected Animator animator;
    //protected PlayerSounds soundsScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.statsScript.SetSprintMode(false);
    }

    protected void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        this.combatScript = animator.GetComponentInChildren<PlayerCombat>();
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        //this.soundsScript = animator.GetComponentInChildren<PlayerSounds>();
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected virtual void SetStats()
    {
        this.statsScript.isBlocking = true;
        this.statsScript.movable = false;
        this.movementScript.StopMoving();
        //this.soundsScript.PlayRandomShieldSwingSound();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.combatScript.WaitEndBlockInput();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.isBlocking = false;
        this.statsScript.movable = true;
        this.animator.ResetTrigger("endState");
    }
}