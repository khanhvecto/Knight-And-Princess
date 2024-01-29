using UnityEngine;

public class KnightBoss_TakeHit_Behavior : StateMachineBehaviour
{
    // References
    protected KnightBoss_Combat combatScript;
    protected KnightBossMove moveScript;

    // states
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!this.isLoadedReferences)
            this.LoadReferences(animator);
        
        this.CounterAttack();
        this.moveScript.StopMoving();
    }

    protected void LoadReferences(Animator animator)
    {
        // combat script
        this.combatScript = animator.GetComponentInChildren<KnightBoss_Combat>();
        // Move script
        this.moveScript = animator.GetComponentInChildren<KnightBossMove>();

        this.isLoadedReferences = true;
    }

    protected virtual void CounterAttack()
    {
        this.combatScript.ChooseAttack();
    }
}
