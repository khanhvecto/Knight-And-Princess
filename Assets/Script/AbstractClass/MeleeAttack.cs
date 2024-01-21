using UnityEngine;

public abstract class MeleeAttack : MonoBehaviour
{
    [Header("--- STATS ---")]
    [Header("Collider")]
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected float range;
    [Header("Combat")]
    protected float damage;
    protected float enduranceDecrement;

    protected virtual void Start()
    {
        this.LoadReferences();
        this.LoadStats();
    }

    protected abstract void LoadReferences();

    protected abstract void LoadStats();

    public void Attack()
    {
        Collider2D hostileColl = Physics2D.OverlapCircle((Vector2) transform.position, this.range, this.targetLayer);
        if (hostileColl == null)
            return;

        IDamageReceiver receiveDamageScript = hostileColl.GetComponentInChildren<IDamageReceiver>();
        receiveDamageScript?.GotHit(this.damage, transform.parent.parent.parent, this.enduranceDecrement);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, this.range);
    }
}
