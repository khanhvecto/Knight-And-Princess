using UnityEngine;

public class KnightBossMove: MonoBehaviour
{
    [Header("--- KNIGHT BOSS MOVE ---")]
    [Header("References")]
    [SerializeField] protected KnightBossStats statsScript;
    [SerializeField] protected KnightBoss_Combat combatScript;

    [Header("Wait attack")]
    protected float attackWaitTime;
    protected float startOfWaitTime;

    [Header("Normal movement")]
    protected float horizontalMoveRange;
    protected Vector2 spawnPlace;
    [Header("Check wall stuck")]
    protected Vector2 capturedPos;
    protected float capturedTime = 0f;

    #region Normal Behavior

    public void ResetNormalMoveRange()
    {
        this.spawnPlace = (Vector2)transform.position;
        this.horizontalMoveRange = this.statsScript.defaultHorizontalMoveRange;
    }

    public void NormalMove()
    {
        float moveDistance = transform.position.x - spawnPlace.x;

        if (IsStuck())
        {
            this.Flip();

            //Reset spawnPoint
            if (moveDistance < 0)
            {
                this.spawnPlace = ((Vector2)transform.position + this.spawnPlace + Vector2.right * this.horizontalMoveRange) / 2;
                this.horizontalMoveRange = this.spawnPlace.x - transform.position.x;
            }
            else
            {
                this.spawnPlace = ((Vector2)transform.position + this.spawnPlace - Vector2.right * this.horizontalMoveRange) / 2;
                this.horizontalMoveRange = transform.position.x - this.spawnPlace.x;
            }
        }
        //If need to turn around
        else if ((moveDistance >= horizontalMoveRange && !statsScript.facingLeft) ||
                (moveDistance <= -horizontalMoveRange && statsScript.facingLeft))
        {
            this.Flip();
        }
        else
        {
            SetMove();
        }
    }

    protected virtual void SetMove()
    {
        if(this.statsScript.moveType == MovingType.walk)
        {
            if (this.statsScript.facingLeft)
                this.statsScript.rb2D.velocity = new Vector2(- this.statsScript.normalSpeed, this.statsScript.rb2D.velocity.y);
            else
                this.statsScript.rb2D.velocity = new Vector2(this.statsScript.normalSpeed, this.statsScript.rb2D.velocity.y);
        }
        else
        {
            if (this.statsScript.facingLeft)
                this.SetMoveVertical(- this.statsScript.normalSpeed);
            else
                this.SetMoveVertical(this.statsScript.normalSpeed);
        }
    }

    protected void SetMoveVertical(float horizontalSpeed)
    {
        if (Mathf.Abs(transform.position.y - this.spawnPlace.y) < this.statsScript.defaultVerticalMoveRange)    // If still in vertical range
        {
            float rand = Random.Range(- this.statsScript.normalSpeed / 5, this.statsScript.normalSpeed / 5);
            this.statsScript.rb2D.velocity = new Vector2(horizontalSpeed, this.statsScript.rb2D.velocity.y + rand);
        }
        else
        {
            if (transform.position.y > this.spawnPlace.y)    // If enemy need to go lower
                this.statsScript.rb2D.velocity = new Vector2(horizontalSpeed, - this.statsScript.normalSpeed / 3);
            else                                            // If enemy need to go higher
                this.statsScript.rb2D.velocity = new Vector2(horizontalSpeed, + this.statsScript.normalSpeed / 3);
        }
    }

    protected bool IsStuck()
    {
        // Check if Knight Boss freezed in a place for 2s
        if (Time.time - this.capturedTime < 2) return false;

        if (this.capturedPos.x == transform.position.x)
        {
            return true;
        }

        this.capturedPos = (Vector2)transform.position;
        this.capturedTime = Time.time;
        return false;
    }

    #endregion

    #region Approach behavior

    public void ApproachEnemy()
    {
        if(!this.IsFarAwayEnemy(1f))
        {
            // Change behavior to ready attack
            this.statsScript.animator.SetBool("ready", true);
            return;
        }

        this.CheckFlip();

        this.MoveToEnemy();
    }

    protected void MoveToEnemy()
    {
        if (this.statsScript.targetColl == null) return;

        if (this.statsScript.moveType == MovingType.walk)
        {
            if (this.statsScript.facingLeft)
                this.statsScript.rb2D.velocity = new Vector2(-this.statsScript.approachSpeed, this.statsScript.rb2D.velocity.y);
            else
                this.statsScript.rb2D.velocity = new Vector2(this.statsScript.approachSpeed, this.statsScript.rb2D.velocity.y);
        }
        else
        {
            var direction = this.statsScript.targetColl.transform.position - transform.position;
            direction.Normalize();
            this.statsScript.rb2D.velocity = direction * this.statsScript.approachSpeed;
        }
    }

    #endregion

    #region Ready attack behavior

    public void ReadyAttack()
    {
        if(this.IsFarAwayEnemy(2f))
        {
            // Change behavior to approach
            this.statsScript.animator.SetBool("ready", false);
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
        this.attackWaitTime = Random.Range(0, this.statsScript.maxAttackDelayTime);
        this.startOfWaitTime = Time.time;
    }

    #endregion

    #region Check state

    protected bool IsFarAwayEnemy(float multiplier)
    {
        if (this.statsScript.targetColl == null) return true;
        float distance = Vector2.Distance(transform.position, this.statsScript.targetColl.transform.position);
        return distance > this.statsScript.approachDistance * multiplier;
    }

    # endregion

    #region Specific movement
    public void CheckFlip()
    {
        if (this.statsScript.targetColl == null)
            return;

        if ((statsScript.facingLeft && transform.position.x < this.statsScript.targetColl.transform.position.x)
            || (!statsScript.facingLeft && transform.position.x > this.statsScript.targetColl.transform.position.x))
            this.Flip();
    }

    protected virtual void Flip()
    {
        this.statsScript.rb2D.velocity = new Vector2(-this.statsScript.rb2D.velocity.x, this.statsScript.rb2D.velocity.y);
        transform.parent.Rotate(0f, 180f, 0f);
        this.statsScript.facingLeft = !this.statsScript.facingLeft;
        this.statsScript.combatRangeOffset.x = -this.statsScript.combatRangeOffset.x;
    }

    public void StopMoving()
    {
        this.statsScript.rb2D.velocity = Vector2.zero;
    }

    public void DashForward(Vector2 force)
    {
        this.statsScript.rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    #endregion
}