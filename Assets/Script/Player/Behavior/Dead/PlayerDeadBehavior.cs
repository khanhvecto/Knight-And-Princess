using UnityEngine;

public class PlayerDeadBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected PlayerCombat combatScript;
    protected UIFunction _UIScript;
    protected Animator animator;
    protected PlayerSounds deadSound;

    [Header("States")]
    protected bool isLoadedReferences = false;

    [Header("Stats")]
    protected int oldLayer;

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
            Debug.LogError("Can't find movement script for PlayerDeadBahavior of " + name);
        // stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerDeadBahavior of " + name);
        // Combat script
        this.combatScript = animator.GetComponentInChildren<PlayerCombat>();
        if (this.combatScript == null)
            Debug.LogError("Can't find combat script for PlayerDeadBehavior of " + name);
        // UI script
        this._UIScript = animator.GetComponentInChildren<UIFunction>();
        if (this._UIScript == null)
            Debug.LogError("Can't find UI script for PlayerDeadBehavior of " + name);
        // Dead sound
        this.deadSound = animator.GetComponentInChildren<PlayerSounds>();
        if (this.deadSound == null)
            Debug.LogError("Can't find dead sound for PlayerDeadBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        // States
        this.statsScript.controlable = false;
        this.statsScript.isDead = true;
        this.statsScript.hurtable = false;
        this.statsScript.stunnedable = false;
        this.movementScript.StopMoving();
        // Layers
        this.oldLayer = animator.gameObject.layer;
        this.animator.gameObject.layer = this.statsScript.deadLayer;
        // Effects
        this._UIScript.ShowDeadScreen(true);
        this.deadSound.PlayRandomDeadSound();

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        // Layers
        this.animator.gameObject.layer = this.oldLayer;
        // Stats
        this.statsScript.ResetCurrentStasts();
        // Animator trigger
        this.animator.ResetTrigger("endState");
    }
}
