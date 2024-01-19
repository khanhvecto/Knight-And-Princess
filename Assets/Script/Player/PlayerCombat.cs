using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageReceiver
{
    [Header("References")]
    [SerializeField] protected PlayerStats statsScript;

    [Header("Stats")]
    public float health;
    public float endurance;

    protected void Start()
    {
        this.CheckReferences();
        this.ResetStats();
    }

    protected void CheckReferences()
    {
        // stats script
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerCombat of " + transform.parent.name);
    }

    protected void Update()
    {
        if (!this.statsScript.controlable)
            return;

        if (this.statsScript.blockable)
            this.WaitBlockInput();
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
        if (InputManager.Instance.GetBlockKeyUp())
            this.statsScript.animator.SetTrigger("endState");
    }

    #endregion

    #region Damage receiver

    protected void GotHurt(float damage, float enduranceDecrement)
    {
        this.health -= damage;
        this.statsScript.animator.SetTrigger("hurt");
    }

    public void GotHit(float damage, Transform attackPos, float enduranceDecrement)
    {
        if (!this.statsScript.hurtable)
            return;

        if (this.statsScript.isBlocking)
        {
            // Check if blocking the wrong way
            if ((this.statsScript.isFacingRight && attackPos.position.x < transform.parent.position.x)
                || (!this.statsScript.isFacingRight && attackPos.position.x > transform.parent.position.x))
            {
                this.GotHurt(damage, enduranceDecrement);
            }
        }
        else
        {
            this.GotHurt(damage, enduranceDecrement);
        }
    }

    #endregion

    public void ResetStats()
    {
        this.health = this.statsScript.health;
        this.endurance = this.statsScript.endurance;
    }
}
