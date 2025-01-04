using UnityEngine;

public class KnightBossReadyAttackBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected KnightBossMove movementScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.movementScript.ResetReadyTimer();
        this.movementScript.StopMoving();
    }

    protected void LoadReferences(Animator animator)
    {
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();

        this.isLoadedReferences = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.ReadyAttack();
    }
}
