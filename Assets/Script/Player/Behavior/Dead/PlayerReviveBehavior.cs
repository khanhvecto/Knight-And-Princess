using UnityEngine;

public class PlayerReviveBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
    }

    protected void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();

        this.isLoadedReferences = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.controlable = true;
        this.statsScript.isDead = false;
        this.statsScript.hurtable = true;
        this.statsScript.stunnedable = true;
    }
}
