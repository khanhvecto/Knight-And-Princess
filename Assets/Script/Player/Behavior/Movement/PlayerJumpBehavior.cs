using UnityEngine;

public class PlayerJumpBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected Animator animator;
    protected PlayerSounds soundScript;

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
        // Sound script
        this.soundScript = animator.GetComponentInChildren<PlayerSounds>();
        if (this.soundScript == null)
            Debug.LogError("Can't find sound script for PlayerIdleBehavior of " + name);
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.movementScript.jumpTakenAmount++;
        this.statsScript.rb2D.velocity = new Vector2(this.statsScript.rb2D.velocity.x, this.statsScript.jumpForce);
        this.soundScript.PlayRandomJumpSound();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.CheckVariableJump();
        this.movementScript.WaitAirJumpInput();
        this.CheckHeight();
    }

    protected void CheckVariableJump()
    {
        // Jump shorter caused released jump button
        if (InputManager.Instance.GetJumpKeyUp())
            this.statsScript.rb2D.velocity = new Vector2(this.statsScript.rb2D.velocity.x, this.statsScript.rb2D.velocity.y * 0.25f);
    }

    protected void CheckHeight()
    {
        var verticalVelocity = this.statsScript.rb2D.velocity.y;
        if (verticalVelocity <= 0.1f)
            this.animator.SetTrigger("endState");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        this.animator.SetBool("isJumping", false);
        this.animator.ResetTrigger("endState");
    }
}
