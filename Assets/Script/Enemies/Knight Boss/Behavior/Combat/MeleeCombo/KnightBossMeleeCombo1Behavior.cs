using UnityEngine;

public class KnightBossMeleeCombo1Behavior : StateMachineBehaviour
{
    [Header("References")]
    protected KnightBossStats statsScript;
    protected KnightBossMove movementScript;
    protected float dashTime;

    [Header("States")]
    protected bool isLoadedReferences = false;
    protected bool isDashing;

    [Header("Stats")]
    protected Vector2 dashForce;
    protected float dashTimer;
    protected float maxDifferentHeight = 0.3f;
    protected float oldGravity;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
        {
            this.LoadReferences(animator);
            this.SetDashTime();
        }
        this.SetStats(animator);
        this.movementScript.CheckFlip();
    }

    protected void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();

        this.isLoadedReferences = true;
    }

    protected void SetStats(Animator animator)
    {
        //Gravity
        this.oldGravity = statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0;

        this.isDashing = true;
        this.dashTimer = 0.0f;

        //Calculate dash force
        if(this.statsScript.targetColl != null)
        {
            this.CheckHeight(animator);

            var direction = this.statsScript.targetColl.transform.position - animator.transform.position;
            Vector2 initialVelocity = 2 * (direction / this.dashTime);
            this.dashForce = this.statsScript.rb2D.mass * initialVelocity;

            this.movementScript.DashForward(this.dashForce);
        }
    }

    protected void CheckHeight(Animator animator)
    {
        var targetHeight = this.statsScript.targetColl.transform.position.y;
        var knightBossHeight = animator.transform.position.y;
        if (targetHeight - knightBossHeight >= this.maxDifferentHeight)
            animator.SetTrigger("endState");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.isDashing)
            this.Dash(animator);
    }

    protected void Dash(Animator animator)
    {
        this.dashTimer += Time.deltaTime;
        if (this.dashTimer >= this.dashTime)
        {
            this.movementScript.StopMoving();
            this.isDashing = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats(animator);
    }

    protected void ResetStats(Animator animator)
    {
        this.statsScript.rb2D.gravityScale = this.oldGravity;
        animator.ResetTrigger("endState");
    }

    protected virtual void SetDashTime()
    {
        this.dashTime = 0.33f;
    }
}
