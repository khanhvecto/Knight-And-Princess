using UnityEngine;

public class FlyingEyeAttack1Behavior : StateMachineBehaviour
{
    [Header("References")]
    protected KnightBossStats statsScript;
    protected KnightBossMove movementScript;

    [Header("States")]
    protected bool isLoadedReferences = false;
    protected bool isDashing;

    [Header("Stats")]
    protected float dashTime = 0.5f;
    protected float dashTimer;
    protected Vector2 momentum;
    protected float oldGravity;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.SetDash(animator);
    }

    protected void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        this.isDashing = true;
        this.dashTimer = 0f;

        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0f;
    }

    protected void SetDash(Animator animator)
    {
        if (this.statsScript.targetColl == null)
            return;

        var direction = this.statsScript.targetColl.transform.position - animator.transform.position;
        Vector2 initialVelocity = 2 * (direction / this.dashTime);
        this.momentum = this.statsScript.rb2D.mass * initialVelocity;

        this.movementScript.DashForward(this.momentum);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.isDashing)
            this.CountDashTime(animator);
    }

    protected void CountDashTime(Animator animator)
    {
        this.dashTimer += Time.deltaTime;
        if (this.dashTimer >= this.dashTime)
        {
            this.movementScript.StopMoving();
            this.isDashing = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset stats
        this.statsScript.rb2D.gravityScale = this.oldGravity;
    }
}
