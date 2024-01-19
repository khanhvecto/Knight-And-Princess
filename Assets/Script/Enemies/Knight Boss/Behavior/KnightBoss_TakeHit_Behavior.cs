using UnityEngine;

public class KnightBoss_TakeHit_Behavior : StateMachineBehaviour
{
    // References
    protected KnightBoss_Combat combatScript;

    // states
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.CounterAttack();
    }

    protected void LoadReferences(Animator animator)
    {
        // combat script
        this.combatScript = animator.GetComponentInChildren<KnightBoss_Combat>();
        if (this.combatScript == null)
            Debug.LogError("Can't find combat script for KnightBoss_TakeHit_Behavior of " + animator.name);

        this.isLoadedReferences = true;
    }

    protected void CounterAttack()
    {
        this.combatScript.ChooseAttack();
    }
}
