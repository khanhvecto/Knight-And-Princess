using Unity.VisualScripting;
using UnityEngine;

public class SlimeNormalMove: MonoBehaviour
{
    //Reference
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [SerializeField] private SlimeState stateScript;
    [SerializeField] private CombatStats statSO;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Basic stats")]
    [SerializeField] private float speed;
    [SerializeField] private float defaultMoveRange = 1.5f;
    private float moveRange;
    private Vector2 spawnPlace;

    [Header("Check wall stuck")]
    protected Vector2 capturedPos;
    protected float capturedTime = 0f;

    private void Start()
    {
        speed = statSO.normalSpeed;
    }

    public void Move()
    {
        float moveDistance = transform.position.x - spawnPlace.x;

        if (IsStuck())
        {
            stateScript.Flip();

            //Reset spawnPoint
            if (moveDistance < 0)
            {
                this.spawnPlace = ((Vector2) transform.position + this.spawnPlace + Vector2.right*this.moveRange) / 2;
                this.moveRange = this.spawnPlace.x - transform.position.x;
            }
            else
            {
                this.spawnPlace = ((Vector2)transform.position + this.spawnPlace - Vector2.right * this.moveRange) / 2;
                this.moveRange = transform.position.x - this.spawnPlace.x;
            }
        }
        //If need to turn around
        else if ((moveDistance >= moveRange && !stateScript.facingLeft) ||
                (moveDistance <= -moveRange && stateScript.facingLeft))
        {
            stateScript.Flip();
        }
        else
        {
            MoveHorizontal();
        }
    }

    void MoveHorizontal()
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
        this.moveRange = this.defaultMoveRange;
    }
}
