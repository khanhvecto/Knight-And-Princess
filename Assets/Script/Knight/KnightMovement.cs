using System.Collections;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    private static KnightMovement instance;
    public static KnightMovement Instance { get => instance; }

    [Header("Reference")]
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private Vector2 boxGroundSize;
    [SerializeField] private float castBoxDistance;

    private Rigidbody2D rigidBody;
    public Rigidbody2D RigidBody => rigidBody;
    private Animator animator;

    [Header("Stats")]
    [SerializeField] private float speed = 8f;
    public float Speed { get => speed; }
    [SerializeField] private float jumpForce = 15f;
    public bool falling = false;

    // State
    public bool moveable = true;

    // Parameter
    private float horizontal;
    public float Horizontal { get => horizontal; set => horizontal = value; }
    private bool isGround;
    public bool IsGround { get => isGround; }
        // Gravity
    public float defaultGravity = 3f;
    public float rollGravity = 0f;

    [Header("Jump")]
        // Jump buffer
    [SerializeField] private float jumpBuffer = 0.2f;
    private float lastJumpPressedTime = -3; // Set to -3 to advoid auto jump when game just start
        // Coyote time
    [SerializeField] protected float coyoteTime = 0.2f;
    protected float leftGroundTime;

    // Falling
    protected bool isFallingFaster = false;

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
        if (KnightState.Instance.controlable && this.moveable)
        {
            checkGround();
            horizontal = Input.GetAxisRaw("Horizontal");
            //Flip 
            if ((horizontal < 0 && KnightState.Instance.facingRight) || (horizontal > 0 && !KnightState.Instance.facingRight)) KnightState.Instance.flip();
            //Implement jump action
            CheckJump();
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

    private void CheckJump()
    {
        if (this.isGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                this.Jump();
            }
            else if (this.lastJumpPressedTime + this.jumpBuffer >= Time.time)
            {
                this.Jump();
            }
        }
        else
        {
            // Jump shorter caused released jump button
            if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.2f);
            }

            // Jump at edge
            if (Input.GetButtonDown("Jump"))
            {
                this.lastJumpPressedTime = Time.time;
                if(this.coyoteTime + this.leftGroundTime >= Time.time)
                {
                    this.Jump();
                }
            }

            // Falling faster
            if(rigidBody.velocity.y < 0 && !this.isFallingFaster) 
            {
                StartCoroutine(FallingFaster());
            }
        }
    }
    protected void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        this.lastJumpPressedTime = Time.time;
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

    protected IEnumerator FallingFaster()
    {
        this.isFallingFaster = true;
        this.rigidBody.gravityScale += 1.5f;

        while(rigidBody.velocity.y < 0 || KnightRoll.Instance.rolling) yield return null;
        this.isFallingFaster = false;
        this.rigidBody.gravityScale = this.defaultGravity;
        yield return null;
    }

    private void checkGround()
    {
        // Set up coyote time
        bool wasOnGround = false;
        if (isGround) wasOnGround = true;

        isGround = Physics2D.BoxCast(transform.position, boxGroundSize, 0f, Vector2.down, castBoxDistance, layerGround);
        
        if(!isGround && wasOnGround) this.leftGroundTime = Time.time;
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