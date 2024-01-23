using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected PlayerStats statsScript;

    [Header("--- STATS ---")]
    [Header("Run")]
    public float horizontal;
    [Header("Jump")]
    protected float lastJumpPressedTime = -3f;  // Need to set to negative to advoid auto jumping when start
    public float leftGroundTime;
    public int jumpTakenAmount;
    [Header("Roll")]
    public float lastRollFinishedTime;
    public float lastRollPressedTime = -3f; // Need to set to negative to advoid auto jumping when start

    [Header("--- CHECK GROUND BOX ---")]
    [SerializeField] protected Vector2 checkGroundBoxSize;
    [SerializeField] private float checkGroundBoxDistance;

    void Start()
    {
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        // stats script
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for Player_Movement of " + transform.parent.name);
    }

    void Update()
    {
        this.CheckGround();
        this.RecordEarlyInputs();

        if (!this.statsScript.controlable) 
            return;

        if (this.statsScript.movable)
        {
            this.CheckFlip();
            if(this.statsScript.sprintable)
                this.CheckSprint();
            this.CheckLookFurther();
        }
        if (this.statsScript.rollable)
            this.WaitRollInput();
    }

    protected void FixedUpdate()
    {
        if (!this.statsScript.controlable) 
            return;

        if(this.statsScript.movable)
            this.SetHorizontalMovement();
    }

    #region Compulsory called every frame

    protected void RecordEarlyInputs()
    {
        // Jump
        if (InputManager.Instance.GetJumpKeyDown()) 
            this.lastJumpPressedTime = Time.time;
        // Roll
        if (InputManager.Instance.GetRollKeyDown())
            this.lastRollPressedTime = Time.time;
    }

    protected void CheckGround()
    {
        // Also find left ground time
        bool wasOnGround = false;
        if (this.statsScript.isOnGround) wasOnGround = true;

        this.statsScript.isOnGround = Physics2D.BoxCast(transform.position, checkGroundBoxSize, 0f, Vector2.down, checkGroundBoxDistance, this.statsScript.groundLayerMask)
            || Physics2D.BoxCast(transform.position, checkGroundBoxSize, 0f, Vector2.down, checkGroundBoxDistance, this.statsScript.enemyLayerMask);

        if (!statsScript.isOnGround && wasOnGround && !InputManager.Instance.GetJumpKeyDown() && !InputManager.Instance.GetJumpKey())
        {
            this.leftGroundTime = Time.time;
        }
    }

    #endregion

    #region Called every frame if controllable

    public void CheckFlip()
    {
        if ((horizontal < 0 && this.statsScript.isFacingRight) || (horizontal > 0 && !this.statsScript.isFacingRight))
            this.Flip();
    }

    protected void CheckSprint()
    {
        if (InputManager.Instance.GetSprintKeyDown() || InputManager.Instance.GetSprintKey())
        {
            if (!this.statsScript.isSprinting)
            {
                this.statsScript.speed *= this.statsScript.sprintCoef;
                this.statsScript.isSprinting = true;
            }
        }
        else if (InputManager.Instance.GetSprintKeyDown())
        {
            if (this.statsScript.isSprinting)
            {
                this.statsScript.speed /= this.statsScript.sprintCoef;
                this.statsScript.isSprinting = false;
            }
        }
        else
        {
            if(this.statsScript.isSprinting)
            {
                this.statsScript.speed /= this.statsScript.sprintCoef;
                this.statsScript.isSprinting = false;
            }
        }
    }

    protected void CheckLookFurther()
    {
        // If don't
        if (this.statsScript.rb2D.velocity != Vector2.zero)
        {
            CameraFollow.Instance.SetLookUp(false);
            CameraFollow.Instance.SetLookDown(false);
        }
        else if (!InputManager.Instance.GetLookDownKey() && !InputManager.Instance.GetLookUpKey())
        {
            CameraFollow.Instance.SetLookUp(false);
            CameraFollow.Instance.SetLookDown(false);
        }
        // If look further
        else if(InputManager.Instance.GetLookDownKey())
        {
            CameraFollow.Instance.SetLookDown(true);
        }
        else
        {
            CameraFollow.Instance.SetLookUp(true);
        }
    }

    protected void SetHorizontalMovement()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal");
        this.statsScript.rb2D.velocity = new Vector2(horizontal * this.statsScript.speed, this.statsScript.rb2D.velocity.y);
    }

    protected void WaitRollInput()
    {
        if (InputManager.Instance.GetRollKeyDown() || this.lastRollPressedTime + this.statsScript.rollBuffer >= Time.time)
        {
            if (this.lastRollFinishedTime + this.statsScript.rollCooldown < Time.time)
            {
                this.horizontal = Input.GetAxisRaw("Horizontal");
                this.CheckFlip();
                this.statsScript.animator.SetTrigger("roll");
            }
        }
    }

    #endregion

    #region Check changing between states

    public void CheckRunning()
    {
        if (this.horizontal != 0 && this.statsScript.isOnGround)
            this.statsScript.animator.SetBool("isRunning", true);
        else this.statsScript.animator.SetBool("isRunning", false);
    }

    public void WaitJumpInput()
    {
        if (!this.statsScript.isOnGround) return;

        if (InputManager.Instance.GetJumpKeyDown())
        {
            this.SetJump();
            return;
        }

        if (this.lastJumpPressedTime + this.statsScript.jumpBuffer >= Time.time)
        {
            this.SetJump();
        }
    }

    public void WaitAirJumpInput()
    {
        if (InputManager.Instance.GetJumpKeyDown() && this.jumpTakenAmount < this.statsScript.stackJumpsAmount) 
            this.statsScript.animator.SetTrigger("airJump");
    }

    public void CheckFalling()
    {
        if (!this.statsScript.isOnGround && this.statsScript.rb2D.velocity.y < 0)
            this.statsScript.animator.SetBool("isFalling", true);
    }

    #endregion

    #region Actual movement

    public void SetJump()
    {
        this.jumpTakenAmount = 0;
        this.statsScript.animator.SetBool("isJumping", true);
    }

    public void StopMoving()
    {
        this.statsScript.rb2D.velocity = Vector2.zero;
    }

    public void Flip()
    {
        this.statsScript.isFacingRight = !this.statsScript.isFacingRight;
        transform.parent.Rotate(0f, 180f, 0f);
    }

    public void PushForward(float force)
    {
        this.statsScript.rb2D.AddForce(transform.right * force, ForceMode2D.Impulse);
    }

    #endregion

    protected void OnDrawGizmos()
    {
        //Draw checkground box
        Gizmos.DrawWireCube((Vector2)transform.position + Vector2.down * checkGroundBoxDistance, checkGroundBoxSize);
    }
}