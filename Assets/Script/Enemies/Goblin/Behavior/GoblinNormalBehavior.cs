using UnityEngine;

public class GoblinNormalBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected KnightBossMove movementScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.movementScript.ResetNormalMoveRange();
    }

    protected void LoadReferences(Animator animator)
    {
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();

        this.isLoadedReferences = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.NormalMove();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.StopMoving();
    }
}
