using UnityEngine;

public class PlayerBlockBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;
    protected PlayerCombat combatScript;
    protected PlayerMovement movementScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
    }

    protected void LoadReferences(Animator animator)
    {
        // stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerBlockBehavior of " + name);
        // combat script
        this.combatScript = animator.GetComponentInChildren<PlayerCombat>();
        if (this.combatScript == null)
            Debug.LogError("Can't find combat script for PlayerBlockBehavior of " + name);
        // movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerBlockBehavior of " + name);

        this.isLoadedReferences = true;
    }

    protected virtual void SetStats()
    {
        this.statsScript.isBlocking = true;
        this.statsScript.movable = false;
        this.movementScript.StopMoving();
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
    }
}