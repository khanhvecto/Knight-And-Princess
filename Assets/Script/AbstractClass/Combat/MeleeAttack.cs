using UnityEngine;

public abstract class MeleeAttack: MonoBehaviour
{
    [Header("Target")]
    protected Collider2D targetCollider;

    [Header("Combat stats")]
    protected float damage;
    protected float enduranceDecrement;

    protected void Start()
    {
        this.LoadCombatStats();
    }

    public void Attack()
    {
        this.FindTarget();

        IDamageReceiver receiveDamageScript = this.targetCollider?.GetComponentInChildren<IDamageReceiver>();
        receiveDamageScript?.GotHit(this.damage, transform.parent.parent.parent.position, this.enduranceDecrement);
    }

    protected abstract void LoadCombatStats();

    protected abstract void FindTarget();
}