using UnityEngine;

public class KnightBossApproachBehavior : StateMachineBehaviour
{
    [Header("References")]
    private KnightBossMove movementScript;

    [Header("States")]
    protected bool isLoadReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadReferences)
            this.LoadReferences(animator);
    }

    protected void LoadReferences(Animator animator)
    {
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();

        this.isLoadReferences = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movementScript.ApproachEnemy();
    }
}
