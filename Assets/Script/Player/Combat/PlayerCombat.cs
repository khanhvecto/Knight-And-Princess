using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageReceiver
{
    [Header("References")]
    [SerializeField] protected PlayerStats statsScript;
    [SerializeField] protected PlayerSounds soundsScript;
    [SerializeField] protected PlayerMovement movementScript;

    [Header("Stats")]
    [SerializeField] protected float currentParryTime;
    [SerializeField] protected float lastBlockPressedTime;

    protected void Start()
    {
        this.CheckReferences();
    }

    protected void CheckReferences()
    {
        // stats script
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerCombat of " + transform.parent.name);
        // Sounds script
        if (this.soundsScript == null)
            Debug.LogError("Can't find parry sounds script for PlayerCombat of " + transform.parent.name);
        // Movement script
        if (this.movementScript == null)
            Debug.LogError("Can't find movement script for PlayerCombat of " + transform.parent.name);
    }

    protected void Update()
    {
        this.RecordEarlyInput();

        if (!this.statsScript.controlAbility || !this.statsScript.controlable)
            return;

        if (this.statsScript.attackAbility && this.statsScript.attackable)
            this.WaitAttackInput();

        if (this.statsScript.blockAbility && this.statsScript.blockable)
            this.WaitBlockInput();
    }

    protected void RecordEarlyInput()
    {
        // Block
        if (InputManager.Instance.GetBlockKeyDown())
        {
            var blockPressedTime = Time.time;
            if (blockPressedTime - this.lastBlockPressedTime <= this.statsScript.parryBuffer)
                this.currentParryTime = this.statsScript.maxParryWindow / 10;
            else 
                this.currentParryTime = this.statsScript.maxParryWindow;
            this.lastBlockPressedTime = blockPressedTime;
        }
    }

    #region Block

    protected void WaitBlockInput()
    {
        if (this.statsScript.isBlocking)
            return;
        if (InputManager.Instance.GetBlockKeyDown() || InputManager.Instance.GetBlockKey())
            this.statsScript.animator.SetTrigger("block");
    }

    public void WaitEndBlockInput()
    {
        if (InputManager.Instance.GetBlockKeyUp() && this.statsScript.isBlocking)
            this.statsScript.animator.SetTrigger("endState");
    }

    #endregion

    #region Damage receiver

    public void GotHit(float damage, Vector3 attackPos, float enduranceDecrement)
    {
        if (!this.statsScript.hurtable)
            return;

        if(this.IsHeadingWrongWay(attackPos))
        {
            this.movementScript.Flip();
            this.GotHurt(damage, enduranceDecrement);
        }
        else
        {
            if(this.lastBlockPressedTime + this.currentParryTime >= Time.time)
                this.ParryingAttack(enduranceDecrement);
            else if (this.statsScript.isBlocking)   
                this.BlockingAttack(enduranceDecrement);
            else
                this.GotHurt(damage, enduranceDecrement);
        }
    }

    protected void ParryingAttack(float enduranceDecrement)
    {
        this.lastBlockPressedTime -= this.statsScript.parryBuffer;  // Allows player parry right after if successful parry, don't need to wait buffer again
        this.soundsScript.PlayRandomParrySound();
        var newEndurance = this.statsScript.CurrentEndurance - enduranceDecrement;
        if (newEndurance <= 0)  // Make sure endurance can't fall to 0
            newEndurance = 1f;
        this.statsScript.SetCurrentEnduranceValue(newEndurance);
    }

    protected void BlockingAttack(float enduranceDecrement)
    {
        var newEndurance = this.statsScript.CurrentEndurance - enduranceDecrement;
        this.statsScript.SetCurrentEnduranceValue(newEndurance);
        if (newEndurance <= this.statsScript.minEndurance && this.statsScript.stunnedable)
        {
            this.statsScript.animator.SetTrigger("stunned");
        }
        else
        {
            this.soundsScript.PlayRandomBlockSound();
        }
    }

    protected void GotHurt(float damage, float enduranceDecrement)
    {
        var newHealth = this.statsScript.CurrentHealth - damage;
        var newEndurance = this.statsScript.CurrentEndurance - enduranceDecrement;

        // To advoid unsynchronize with animator, need to separate each state
        if(newHealth < this.statsScript.CurrentHealth)
        {
            this.statsScript.animator.SetTrigger("hurt");
        }    
        else if (newEndurance <= this.statsScript.minEndurance && this.statsScript.stunnedable)
        {
            this.statsScript.animator.SetTrigger("stunned");
        }

        this.statsScript.SetCurrentHealthValue(newHealth);
        this.statsScript.SetCurrentEnduranceValue(newEndurance);
    }

    protected bool IsHeadingWrongWay(Vector3 attackPos)
    {
        if ((this.statsScript.isFacingRight && attackPos.x < transform.parent.position.x)
            || (!this.statsScript.isFacingRight && attackPos.x > transform.parent.position.x))
        return true;
        
        return false;
    }

    #endregion

    #region Attack

    protected void WaitAttackInput()
    {
        if (!InputManager.Instance.GetAttackKeyDown())
            return;

        if (this.statsScript.isAttacking)
        {
            // Check if player want to flip
            this.movementScript.horizontal = Input.GetAxisRaw("Horizontal");
            this.movementScript.CheckFlip();

            this.statsScript.animator.SetTrigger("attackCombo");
        }
        else
        {
            this.statsScript.animator.SetTrigger("attack");
        }
    }

    #endregion

    public void Revive()
    {
        this.statsScript.animator.SetBool("isDead", false);
    }
}
