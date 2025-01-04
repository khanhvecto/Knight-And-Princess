using UnityEngine;

public class KnightBossMeleeComboJumpBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected KnightBossStats statsScript;
    protected KnightBossMove movementScript;
    protected LayerMask groundLayerMask;

    [Header("States")]
    protected bool isLoadedReferences = false;

    [Header("Stats")]
    protected float oldGravity;
    protected int oldLayer;
    protected Vector2 newPos;
    protected float jumpSpeed = 3f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats(animator);
    }

    protected void LoadReferences(Animator animator)
    {
        this.statsScript = animator.GetComponentInChildren<KnightBossStats>();
        this.movementScript = animator.GetComponentInChildren<KnightBossMove>();
        this.groundLayerMask = LayerMask.GetMask("Ground");

        this.isLoadedReferences = true;
    }

    protected void SetStats(Animator animator)
    {
        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0f;

        this.oldLayer = animator.gameObject.layer;
        animator.gameObject.layer = 9;  // Dead layer

        if(this.statsScript.targetColl != null)
        {
            var newPosVertical = this.statsScript.targetColl.transform.position.y + this.statsScript.rb2D.GetComponent<BoxCollider2D>().size.y;
            var newPosVerticalOffset = newPosVertical - animator.transform.position.y;

            // Find new horizontal position
            RaycastHit2D hit = Physics2D.Raycast(animator.transform.position, new Vector2(-1,1), newPosVerticalOffset*Mathf.Sqrt(2f), this.groundLayerMask);   // Check if hit wall
            float newPosHorizontal;
            if (hit)
            {
                newPosHorizontal = animator.transform.position.x + newPosVerticalOffset;
            }
            else
                newPosHorizontal = animator.transform.position.x - newPosVerticalOffset;

            this.newPos = new Vector2(newPosHorizontal, newPosVertical);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.movementScript.CheckFlip();
        if (this.statsScript.targetColl != null)
            this.Jump(animator);
        else
            animator.SetTrigger("endState");
    }

    protected void Jump(Animator animator)
    {
        animator.transform.position = Vector2.Lerp(animator.transform.position, this.newPos, this.jumpSpeed * Time.deltaTime);

        if (Mathf.Abs(animator.transform.position.y - this.newPos.y) <= 0.1)
            animator.SetTrigger("endState");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats(animator);
    }

    protected void ResetStats(Animator animator)
    {
        this.statsScript.rb2D.gravityScale = this.oldGravity;
        animator.gameObject.layer = this.oldLayer;
        animator.ResetTrigger("endState");
    }
}
