using UnityEngine;

public class PlayerRollBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected Animator animator;
    protected PlayerSounds soundsScript;

    [Header("States")]
    protected bool isLoadedReferences = false;

    [Header("Stats")]
    protected float oldGravity;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.statsScript.SetSprintMode(false);
        this.PushForward();
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
        // Set stats
        this.statsScript.controlable = false;
        this.statsScript.rollable = false;
        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0;
        this.statsScript.hurtable = false;
        Physics2D.IgnoreLayerCollision(this.animator.gameObject.layer, this.statsScript.enemyLayer, true);
        this.soundsScript.PlayRandomRollSound();
    }

    protected void PushForward()
    {
        if (this.statsScript.isFacingRight)
            this.statsScript.rb2D.velocity = Vector2.right * this.statsScript.rollForce;
        else
            this.statsScript.rb2D.velocity = Vector2.left * this.statsScript.rollForce;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats();
    }

    protected void ResetStats()
    {
        // Stop roll
        this.movementScript.StopMoving();

        // Restore stats
        this.statsScript.rb2D.gravityScale = this.oldGravity;
        this.statsScript.controlable = true;
        this.statsScript.hurtable = true;
        Physics2D.IgnoreLayerCollision(this.animator.gameObject.layer, this.statsScript.enemyLayer, false);

        // Record rollFinishTime
        this.movementScript.lastRollFinishedTime = Time.time;
    }
}
