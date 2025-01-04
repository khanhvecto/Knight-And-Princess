using System.Collections;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    private static KnightMovement instance;
    public static KnightMovement Instance { get => instance; }

    [Header("Reference")]
    [SerializeField] private LayerMask layerGround;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private Vector2 boxGroundSize;
    [SerializeField] private float castBoxDistance;

    private Rigidbody2D rigidBody;
    public Rigidbody2D RigidBody => rigidBody;
    private Animator animator;

    [Header("Stats")]
    public float speed = 8f;
    [SerializeField] private float jumpForce = 15f;

    [Header("States")]
    public bool falling = false;
    public bool moveable = true;

    [Header("Paremeters")]
    public float horizontal;
    public bool isGround;

    [Header("Gravity")]
    public float defaultGravity = 3f;
    public float rollGravity = 0f;

    [Header("Jump")]
        // Jump buffer
    [SerializeField] private float jumpBuffer = 0.3f;
    private float lastJumpPressedTime = -3; // Set to -3 to advoid auto jump when game just start
        // Coyote time
    [SerializeField] protected float coyoteTime = 0.2f;
    protected float leftGroundTime;

    [Header("Falling")]
    [SerializeField] protected float maxFallSpeed;

    [Header("Floating at highest point")]
    [SerializeField] protected float floatingAtHighestPointTime;
    protected bool floatingAtHighestPoint;
    protected Vector2 preVelocity;

    private void Awake()
    {
        //Design pattern
        if (KnightMovement.instance != null) Debug.LogError("Only 1 KnightMovement allow to exist!");
        instance = this;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.RecordJump();

        if (KnightState.Instance.controlable && this.moveable)
        {
            CheckGround();
            horizontal = Input.GetAxisRaw("Horizontal");
            //Flip 
            if ((horizontal < 0 && KnightState.Instance.facingRight) || (horizontal > 0 && !KnightState.Instance.facingRight)) 
                KnightState.Instance.Flip();
            //Implement jump action
            CheckJump();
            //Check fall
            this.CheckFall();
            //Gravity
            this.SetGravity();
        }
        else
        {
            this.StopAllCoroutines();
        }

        //Set parameters for animation
        this.SetAnimation();
    }

    private void FixedUpdate()
    {
        if (!KnightState.Instance.controlable) return;
        if (!this.moveable) return;

        rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
    }

    protected void RecordJump()
    {
        if (Input.GetButtonDown("Jump")) this.lastJumpPressedTime = Time.time;
    }

    private void CheckJump()
    {
        if (this.isGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                this.Jump();
                return;
            }
            
            if (this.lastJumpPressedTime + this.jumpBuffer >= Time.time)
            {
                this.Jump();
            }

            return;
        }

        if (this.rigidBody.velocity.y > 0)
        {
            // Jump shorter caused released jump button
            if (Input.GetButtonUp("Jump"))
                this.rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.2f);

            return;
        }

        
        if (Input.GetButtonDown("Jump"))
        {
            if (this.coyoteTime + this.leftGroundTime >= Time.time)
            {
                this.Jump();
            }
        }
    }

    protected void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        this.lastJumpPressedTime = Time.time;
    }

    protected void CheckFall()
    {
        if (this.rigidBody.velocity.y >= 0) return;

        // Limit max falling speed
        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, Mathf.Max(this.rigidBody.velocity.y, -this.maxFallSpeed));
    }

    protected void SetGravity()
    {
        if (this.floatingAtHighestPoint) return;
        if(this.IsStartFloatAtHighestPoint())
        {
            StartCoroutine(this.FloatingAtHighestPoint());
            return;
        }

        if (this.rigidBody.velocity.y < 0)
            this.rigidBody.gravityScale = this.defaultGravity + 2f; // Fall faster
        else
            this.rigidBody.gravityScale = this.defaultGravity;
    }

    protected bool IsStartFloatAtHighestPoint()
    {
        if (this.isGround) return false;

        if(this.preVelocity.y > 0 && this.rigidBody.velocity.y <= 0)
        {
            this.preVelocity = this.rigidBody.velocity;
            return true;
        }

        this.preVelocity = this.rigidBody.velocity;
        return false;
    }

    protected IEnumerator FloatingAtHighestPoint()
    {
        this.floatingAtHighestPoint = true;
        this.rigidBody.gravityScale = 0;

        yield return new WaitForSeconds(this.floatingAtHighestPointTime);

        this.rigidBody.gravityScale = this.defaultGravity;
        this.floatingAtHighestPoint = false;
    }

    private void SetAnimation()
    {
        if (horizontal != 0 && isGround) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        if (!isGround && rigidBody.velocity.y > 0) animator.SetBool("isJumping", true);
        else animator.SetBool("isJumping", false);
        if (!isGround && rigidBody.velocity.y <= 0)
        {
            animator.SetBool("isFalling", true);
            this.falling = true;
        }
        else
        {
            animator.SetBool("isFalling", false);
            this.falling = false;
        }
    }

    private void CheckGround()
    {
        // Set up coyote time
        bool wasOnGround = false;
        if (isGround) wasOnGround = true;

        isGround = Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, layerGround)
            || Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, this.enemyLayer);

        if (!isGround && wasOnGround && !Input.GetButtonDown("Jump") && !Input.GetButton("Jump"))
        {
            this.leftGroundTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If touch enemy
        if (collision.gameObject.layer == 8)
        {
            KnightHurt.Instance.TakeDamage(KnightStats.Instance.touchDamage, collision.transform);
        }
    }

    private void OnDrawGizmosSelected() //To show groundCheckBox
    {
        //Draw checkground box
        Gizmos.DrawWireCube((Vector2) transform.position + Vector2.down * castBoxDistance , boxGroundSize);
    }
}