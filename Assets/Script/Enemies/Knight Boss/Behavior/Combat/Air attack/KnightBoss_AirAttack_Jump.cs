using UnityEngine;

public class KnightBoss_AirAttack_Jump : StateMachineBehaviour
{
    // References
    protected KnightBossStats statsScript;

    // Stats
    protected float jumpHeight = 10f;
    protected float newPosHeight;
    protected float jumpSpeed = 3f;
    protected float oldGravity;

    // States
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isLoadedReferences)
            this.LoadReferences(animator);

        this.ResetStats();
    }

    private void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();
    }

    private void ResetStats()
    {
        // Gravity
        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0f;

        // Height
        this.newPosHeight = this.statsScript.targetColl.transform.position.y + this.jumpHeight;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.statsScript.targetColl == null)
        {
            animator.SetTrigger("endState");
            return;
        }

        // Jump to a height level and on top target in a time 
        Vector2 newPos = new Vector2(this.statsScript.targetColl.transform.position.x, this.newPosHeight);
        animator.transform.position = Vector2.Lerp(animator.transform.position, newPos, this.jumpSpeed*Time.deltaTime);

        if (Mathf.Abs(animator.transform.position.y - this.newPosHeight) <= 0.2)
            animator.SetTrigger("endState");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("endState");
        this.statsScript.rb2D.gravityScale = this.oldGravity;
    }
}
