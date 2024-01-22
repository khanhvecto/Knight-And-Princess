using UnityEngine;

public class KnightBoss_AirAttack_Jump : StateMachineBehaviour
{
    // References
    protected KnightBossStats statsScript;

    // Stats
    protected float jumpHeight = 7f;
    protected Vector2 newPos;
    protected float jumpSpeed = 3f;
    protected float oldGravity;

    // States
    protected bool isLoadedReferences = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isLoadedReferences)
            this.LoadReferences(animator);

        this.ResetStats(animator);
    }

    private void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();
    }

    private void ResetStats(Animator animator)
    {
        // Gravity
        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0f;

        // Height
        var newPosHorizontal = (this.statsScript.targetColl.transform.position.x + animator.transform.position.x)/2;
        var newPosHeight = this.statsScript.targetColl.transform.position.y + this.jumpHeight;
        this.newPos = new Vector2(newPosHorizontal, newPosHeight);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.statsScript.targetColl == null)
        {
            animator.SetTrigger("endState");
            return;
        }

        // Jump to a height level and on top target in a time 
        animator.transform.position = Vector2.Lerp(animator.transform.position, this.newPos, this.jumpSpeed*Time.deltaTime);

        if (Mathf.Abs(animator.transform.position.y - this.newPos.y) <= 0.2)
            animator.SetTrigger("endState");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("endState");
        this.statsScript.rb2D.gravityScale = this.oldGravity;
    }
}
