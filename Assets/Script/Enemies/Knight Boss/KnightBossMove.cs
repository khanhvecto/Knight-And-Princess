using UnityEngine;

public class KnightBossMove: MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected KnightBossStats statScript;
    [SerializeField] protected KnightBoss_Combat combatScript;
    [SerializeField] protected Transform hudObj;

    [Header("Parameters")]
    protected float attackWaitTime;
    protected float startOfWaitTime;

    public void StopMoving()
    {
        this.statScript.rb2D.velocity = Vector2.zero;
    }

    #region Approach behavior

    public void ApproachEnemy()
    {
        if(!this.IsFarAwayEnemy(1f))
        {
            // Change behavior to ready attack
            this.statScript.animator.SetBool("ready", true);
            return;
        }

        this.CheckFlip();

        this.MoveToEnemy();
    }
    protected void MoveToEnemy()
    {
        // Move on ground
        if (this.statScript.facingLeft)
            this.statScript.rb2D.velocity = new Vector2(-this.statScript.approachSpeed, this.statScript.rb2D.velocity.y);
        else
            this.statScript.rb2D.velocity = new Vector2(this.statScript.approachSpeed, this.statScript.rb2D.velocity.y);
    }

    #endregion

    #region Ready attack behavior

    public void ReadyAttack()
    {
        if(this.IsFarAwayEnemy(2f))
        {
            // Change behavior to approach
            this.statScript.animator.SetBool("ready", false);
            return;
        }

        this.CheckFlip();

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

    #endregion

    protected void CheckFlip()
    {
        if (this.statScript.targetColl == null)
            return;

        if ((statScript.facingLeft && transform.position.x < this.statScript.targetColl.transform.position.x) 
            || (!statScript.facingLeft && transform.position.x > this.statScript.targetColl.transform.position.x))
        {
            this.statScript.rb2D.velocity = new Vector2(-this.statScript.rb2D.velocity.x, this.statScript.rb2D.velocity.y);
            transform.parent.Rotate(0f, 180f, 0f);
            this.hudObj.Rotate(0f, 180f, 0f);
            this.statScript.facingLeft = !this.statScript.facingLeft;
            this.statScript.combatRangeOffset.x = -this.statScript.combatRangeOffset.x;
        }
    }

    protected bool IsFarAwayEnemy(float multiplier)
    {
        if (this.statScript.targetColl == null) return true;
        float distance = Vector2.Distance(transform.position, this.statScript.targetColl.transform.position);
        return distance > this.statScript.distance * multiplier;
    }

}