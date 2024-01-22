using UnityEngine;

public class KnightBossMeleeComboStartBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected Transform effectPos;

    [Header("States")]
    protected bool isLoadReferences = false;

    [Header("Stats")]
    protected float dangerInformTime = 0.3f;
    protected float dangerInformTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!this.isLoadReferences) 
            this.LoadReferences(animator);
        this.SetStats();
        this.PlayEffect();
    }

    protected void LoadReferences(Animator animator)
    {
        this.effectPos = animator.transform.Find("Effects").Find("ReusableEffectPos").Find("DangerInform");

        this.isLoadReferences = true;
    }

    protected void SetStats()
    {
        this.dangerInformTimer = 0f;
    }

    protected void PlayEffect()
    {
        ParticleSystem effect = DangerInformPool.Instance.Get().GetComponent<ParticleSystem>();
        if (effect == null)
            return;

        effect.transform.position = this.effectPos.position;
        effect.Play();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.CheckEndState(animator);
    }

    protected void CheckEndState(Animator animator)
    {
        this.dangerInformTimer += Time.deltaTime;
        if (this.dangerInformTimer >= this.dangerInformTime)
            animator.SetTrigger("endState");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats(animator);
    }

    protected void ResetStats(Animator animator)
    {
        animator.ResetTrigger("endState");
    }
}
