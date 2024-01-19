using UnityEngine;

public class SlimeAttackMove : MonoBehaviour
{
    //Reference
    [SerializeField] protected SlimeState stateScript;
    [SerializeField] protected SlimeStats statsScript;
    [SerializeField] protected SlimeDamageSender combatScript;
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] protected Animator animator;

    public void ApproachEnemy()
    {
        if(NeedToFlip())
        {
            stateScript.Flip();
        }
        
        //Check for move
        if(FarAwayEnemy(1f))
        {
            MoveToEnemy();
        }
        else
        {
            Stop();
            this.ActionWhenCloseEnemy();
        }
    }

    //Check if need to move to hostile

    protected bool FarAwayEnemy(float multiplier)
    {
        if (this.stateScript.targetColl == null) return true;
        float distance = Vector2.Distance(transform.position, this.stateScript.targetColl.transform.position);
        return distance > this.statsScript.distance * multiplier;
    }

    protected virtual void MoveToEnemy()
    {
        if (this.stateScript.targetColl == null) return;

        if(this.statsScript.moveType == movingType.walk)
        {
            if (this.stateScript.facingLeft)
                this.rb2D.velocity = new Vector2(-this.statsScript.approachSpeed, this.rb2D.velocity.y);
            else 
                this.rb2D.velocity = new Vector2(this.statsScript.approachSpeed, this.rb2D.velocity.y);
        }
        else
        { 
            var direction = this.stateScript.targetColl.transform.position - transform.position;
            direction.Normalize();
            this.rb2D.velocity = direction * this.statsScript.approachSpeed;
        }
    }

    protected void Stop()
    {
        rb2D.velocity = new Vector2(0, 0);
    }

    protected virtual void ActionWhenCloseEnemy()
    {
        combatScript.TryAttack();
    }

    //Check if need to flip

    protected virtual bool NeedToFlip()
    {
        if (this.stateScript.targetColl == null) return false;
        return (stateScript.facingLeft && transform.position.x < this.stateScript.targetColl.transform.position.x) ||
            (!stateScript.facingLeft && transform.position.x > this.stateScript.targetColl.transform.position.x);
    }
}
