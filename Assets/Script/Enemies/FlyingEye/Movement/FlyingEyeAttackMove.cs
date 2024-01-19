using System.Collections;
using UnityEngine;

public class FlyingEyeAttackMove: SlimeAttackMove
{
    protected bool waitingForAttack = false;

    [Header("Parameters")]
    protected float lastAttackTime;

    public void ReadyAttack()
    {
        if(base.FarAwayEnemy(2f))
        {
            base.animator.SetBool("ready", false);
            this.waitingForAttack = false;
            this.StopAllCoroutines();
            return;
        }

        if (base.NeedToFlip())
            base.stateScript.Flip();

        if (Time.time - this.lastAttackTime > base.statsScript.maxAttackDelayTime)
        {
            this.combatScript.TryAttack();
            this.lastAttackTime = Time.time;
        }
        else if (!waitingForAttack)
            StartCoroutine(WaitForAttack());
    }

    protected IEnumerator WaitForAttack()
    {
        base.Stop();

        this.waitingForAttack = true;
        int waitTime = Random.Range(-1, 3);
        yield return new WaitForSeconds(waitTime);
        
        this.waitingForAttack = false;
        this.lastAttackTime = Time.time;
        base.combatScript.TryAttack();
    }

    protected override void ActionWhenCloseEnemy()
    {
        base.animator.SetBool("ready", true);
    }
}