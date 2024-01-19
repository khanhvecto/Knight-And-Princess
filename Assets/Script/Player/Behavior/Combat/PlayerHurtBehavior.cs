using UnityEngine;

public class PlayerHurtBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;
    protected Animator animator;
    protected PlayerMovement movementScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.movementScript.StopMoving();
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
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.statsScript.controlable = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.controlable = true;
    }
}
