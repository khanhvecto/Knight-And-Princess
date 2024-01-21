using UnityEngine;

public class KnightBoss_AirAttack_Jump : StateMachineBehaviour
{
    // References
    protected SlimeState stateScript;

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
        this.stateScript = animator.GetComponentInChildren<SlimeState>();
        if (this.stateScript == null) Debug.LogError("Can't find state script for KnightBoss_AirAttack_Jump of " + animator.name);
    }

    private void ResetStats()
    {
        // Gravity
        this.oldGravity = this.stateScript.rb2D.gravityScale;
        this.stateScript.rb2D.gravityScale = 0f;

        // Height
        this.newPosHeight = this.stateScript.targetColl.transform.position.y + this.jumpHeight;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.stateScript.targetColl == null)
        {
            animator.SetTrigger("endAttack");
            return;
        }

        // Jump to a height level and on top target in a time 
        Vector2 newPos = new Vector2(this.stateScript.targetColl.transform.position.x, this.newPosHeight);
        animator.transform.position = Vector2.Lerp(animator.transform.position, newPos, this.jumpSpeed*Time.deltaTime);

        if (Mathf.Abs(animator.transform.position.y - this.newPosHeight) <= 0.2)
            animator.SetTrigger("endAttack");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.stateScript.rb2D.gravityScale = this.oldGravity;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
