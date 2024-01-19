using UnityEngine;

public class PlayerDeadBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected Animator animator;

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
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.movementScript.StopMoving();
        this.statsScript.controlable = false;
        this.statsScript.isDead = true;
        this.statsScript.hurtable = false;
        this.oldLayer = animator.gameObject.layer;
        this.animator.gameObject.layer = this.statsScript.deadLayer;
    }

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.controlable = true;
        this.animator.gameObject.layer = this.oldLayer;
        this.statsScript.isDead = false;
        this.statsScript.hurtable = true;
    }
}
