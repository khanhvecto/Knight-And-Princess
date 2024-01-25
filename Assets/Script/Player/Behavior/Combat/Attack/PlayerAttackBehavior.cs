using UnityEngine;

public class PlayerAttackBehavior : StateMachineBehaviour
{
    [Header("References")]
    protected PlayerStats statsScript;
    protected PlayerSounds soundScript;
    protected PlayerMovement movementScript;

    [Header("States")]
    protected bool isLoadedReferences = false;
    protected bool isPushing;

    [Header("Stats")]
    protected float pushTimer;
    protected float oldGravity;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.isLoadedReferences)
            this.LoadReferences(animator);
        this.SetStats();
        this.statsScript.SetSprintMode(false);
    }
    
    protected void LoadReferences(Animator animator)
    {
        // Stats script
        this.statsScript = animator.GetComponentInChildren<PlayerStats>();
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerAttackStartBehavior");
        // Sound script
        this.soundScript = animator.GetComponentInChildren<PlayerSounds>();
        if (this.soundScript == null)
            Debug.LogError("Can't find sound script for PlayerAttackStartBehavior");
        // Movement script
        this.movementScript = animator.GetComponentInChildren<PlayerMovement>();
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerAttackStartBehavior");

        this.isLoadedReferences = true;
    }

    protected void SetStats()
    {
        // States
        this.statsScript.isAttacking = true;
        this.statsScript.blockable = false;
        this.statsScript.movable = false;

        this.soundScript.PlayRandomSwordSlashSound();
        
        // Push forward
        this.movementScript.StopMoving();
        this.oldGravity = this.statsScript.rb2D.gravityScale;
        this.statsScript.rb2D.gravityScale = 0f;
        this.pushTimer = 0f;
        this.isPushing = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(this.isPushing)
            this.PushForward();
    }

    protected void PushForward()
    {
        this.pushTimer += Time.deltaTime;
        if (this.pushTimer <= this.statsScript.pushTime)
        {
            this.movementScript.PushForward(this.statsScript.pushForce);
        }
        else
        {
            this.movementScript.StopMoving();
            this.isPushing = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.ResetStats(animator);
    }

    protected void ResetStats(Animator animator)
    {
        this.statsScript.isAttacking = false;
        this.statsScript.blockable = true;
        this.statsScript.movable = true;
        animator.ResetTrigger("attackCombo");

        this.statsScript.rb2D.gravityScale = this.oldGravity;
    }
}
