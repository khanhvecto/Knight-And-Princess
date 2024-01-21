using UnityEngine;

public class PlayerFallBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected Animator animator;
    protected PlayerSounds soundsScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
    }

    protected void LoadReferences(Animator animator)
    {
        // movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerIdleBehavior of " + name);
        // stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerIdleBehavior of " + name);
        // Sounds script
        this.soundsScript = animator.GetComponentInChildren<PlayerSounds>();
        if (this.soundsScript == null)
            Debug.LogError("Can't find sounds script for PlayerIdleBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        // Make the fall faster
        this.statsScript.rb2D.gravityScale *= 1.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.LimitFallSpeed();
        this.CheckCoyoteTime();
        this.movementScript.WaitAirJumpInput();
        this.CheckLanding();
    }

    protected void LimitFallSpeed()
    {
        this.statsScript.rb2D.velocity = new Vector2(this.statsScript.rb2D.velocity.x, Mathf.Max(this.statsScript.rb2D.velocity.y, -this.statsScript.maxFallSpeed));
    }

    protected void CheckCoyoteTime()
    {
        if (!InputManager.Instance.GetJumpKeyDown()) return;
        
        if (this.statsScript.coyoteTime + this.movementScript.leftGroundTime >= Time.time)
        {
            this.movementScript.SetJump();
        }
    }

    protected void CheckLanding()
    {
        if (this.statsScript.isOnGround || this.statsScript.rb2D.velocity.y == 0)
        {
            this.soundsScript.PlayRandomLandingSound();
            this.animator.SetTrigger("endState");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.statsScript.rb2D.gravityScale /= 1.5f;
        animator.SetBool("isFalling", false);
        this.animator.ResetTrigger("endState");
    }
}
