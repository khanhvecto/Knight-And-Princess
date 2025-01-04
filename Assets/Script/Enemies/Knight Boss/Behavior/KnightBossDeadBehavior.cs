using UnityEngine;

public class KnightBossDeadBehavior : StateMachineBehaviour
{
    [Header("------ KNIGHT BOSS DEAD BEHAVIOR ------")]
    [Header("References")]
    protected KnightBossMove movementScript;
    protected KnightBossStats statsScript;

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
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();

        this.isLoadedReferences = true;
    }

    protected virtual void SetStats()
    {
        this.statsScript.isDead = true;
        this.statsScript.rb2D.gameObject.layer = 9; //Dead layer
        this.movementScript.StopMoving();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected virtual void ResetStats()
    {
        this.statsScript.isDead = false;
        this.statsScript.SetHealthValue(this.statsScript.maxHealth);
    }
}
