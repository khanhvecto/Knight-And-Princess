using UnityEngine;

public class PlayerFloatBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerMovement movementScript;
    protected PlayerStats statsScript;
    protected Animator animator;

    [Header("States")]
    protected bool isLoadedReferences = false;

    [Header("Stats")]
    protected float oldGravity;
    protected float floatTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.InitStats();
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
        // animator
        this.animator = animator;

        this.isLoadedReferences = true;
    }

    protected void InitStats()
    {
        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0.3f;
        //this.statsScript.rb2D.velocity = new Vector2(this.statsScript.rb2D.velocity.x, 0);  // Make vertical velocity to 0

        this.floatTimer = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.WaitAirJumpInput();
        this.CheckFloatTime();
    }

    protected void CheckFloatTime()
    {
        this.floatTimer += Time.deltaTime;
        if (this.floatTimer >= this.statsScript.floatTime)
        {
            this.animator.SetTrigger("endState");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.statsScript.rb2D.gravityScale = this.oldGravity;
        this.animator.ResetTrigger("endState");
    }
}
