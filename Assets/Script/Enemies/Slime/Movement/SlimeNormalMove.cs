using UnityEngine;

public class SlimeNormalMove: MonoBehaviour
{
    //Reference
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SlimeState stateScript;
    [SerializeField] protected EnemyStats statSO;

    [Header("Layer")]
    [SerializeField] protected LayerMask groundLayer;

    [Header("Basic stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected float defaultHorizontalMoveRange;
    protected float horizontalMoveRange;
    protected Vector2 spawnPlace;

    [Header("Check wall stuck")]
    protected Vector2 capturedPos;
    protected float capturedTime = 0f;

    protected virtual void Start()
    {
        speed = statSO.normalSpeed;
        this.defaultHorizontalMoveRange = this.statSO.horizontalMoveRange;
    }

    public virtual void Move()
    {
        float moveDistance = transform.position.x - spawnPlace.x;

        if (IsStuck())
        {
            stateScript.Flip();

            //Reset spawnPoint
            if (moveDistance < 0)
            {
                this.spawnPlace = ((Vector2) transform.position + this.spawnPlace + Vector2.right*this.horizontalMoveRange) / 2;
                this.horizontalMoveRange = this.spawnPlace.x - transform.position.x;
            }
            else
            {
                this.spawnPlace = ((Vector2)transform.position + this.spawnPlace - Vector2.right * this.horizontalMoveRange) / 2;
                this.horizontalMoveRange = transform.position.x - this.spawnPlace.x;
            }
        }
        //If need to turn around
        else if ((moveDistance >= horizontalMoveRange && !stateScript.facingLeft) ||
                (moveDistance <= -horizontalMoveRange && stateScript.facingLeft))
        {
            stateScript.Flip();
        }
        else
        {
            SetMove();
        }
    }

    protected virtual void SetMove()
    {
        if (stateScript.facingLeft)
        {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        }
    }
    
    protected bool IsStuck()
    {
        // Check if slime freezed in a place for 2s
        if (Time.time - this.capturedTime < 2) return false;

        if(this.capturedPos == (Vector2) transform.position)
        { 
            return true;
        }

        this.capturedPos = (Vector2)transform.position;
        this.capturedTime = Time.time;
        return false;
    }
    public void ResetNormalMoveRange()
    {
        this.spawnPlace = (Vector2)transform.position;
        this.horizontalMoveRange = this.defaultHorizontalMoveRange;
    }
}
