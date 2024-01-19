using UnityEngine;

public class KnightBossMove: MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SlimeState stateScript;
    [SerializeField] protected SlimeStats statScript;
    [SerializeField] protected KnightBoss_Combat combatScript;

    [Header("Parameters")]
    protected float attackWaitTime;
    protected float startOfWaitTime;

    protected void Start()
    {
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        // state script
        if (this.stateScript == null)
            Debug.LogError("Can't find state script for KnightBossMove of " + transform.parent.name);
        // stat script
        if (this.statScript == null) 
            Debug.LogError("Can't find stat script for KnightBossMove of " + transform.parent.name);
        // combat script
        if (this.combatScript == null) 
            Debug.LogError("Can't find conbat script for KnightBossMove of " + transform.parent.name);
    }

    public void Stop()
    {
        this.stateScript.rb2D.velocity = Vector2.zero;
    }

    public void ApproachEnemy()
    {
        if(!this.IsFarAwayEnemy(1f))
        {
            // Change behavior to ready attack
            this.stateScript.animator.SetBool("ready", true);
            return;
        }

        if (this.IsNeedToFlip())
            this.stateScript.Flip();

        this.MoveToEnemy();
    }

    public void ReadyAttack()
    {
        if(this.IsFarAwayEnemy(2f))
        {
            // Change behavior to approach
            this.stateScript.animator.SetBool("ready", false);
            return;
        }

        if(this.IsNeedToFlip())
            this.stateScript.Flip();

        if (Time.time - this.startOfWaitTime >= this.attackWaitTime)
        {
            this.combatScript.ChooseAttack();
        }
    }

    public void ResetReadyTimer()
    {
        // Make Knight boss ready attack timer count again
        this.attackWaitTime = Random.Range(0, this.statScript.maxAttackDelayTime);
        this.startOfWaitTime = Time.time;
    }

    protected virtual bool IsNeedToFlip()
    {
        if (this.stateScript.targetColl == null) return false;
        return (stateScript.facingLeft && transform.position.x < this.stateScript.targetColl.transform.position.x) ||
            (!stateScript.facingLeft && transform.position.x > this.stateScript.targetColl.transform.position.x);
    }

    protected bool IsFarAwayEnemy(float multiplier)
    {
        if (this.stateScript.targetColl == null) return true;
        float distance = Vector2.Distance(transform.position, this.stateScript.targetColl.transform.position);
        return distance > this.statScript.distance * multiplier;
    }

    protected void MoveToEnemy()
    {
        // Move on ground
        if (this.stateScript.facingLeft)
            this.stateScript.rb2D.velocity = new Vector2(-this.statScript.approachSpeed, this.stateScript.rb2D.velocity.y);
        else
            this.stateScript.rb2D.velocity = new Vector2(this.statScript.approachSpeed, this.stateScript.rb2D.velocity.y);
    }
}