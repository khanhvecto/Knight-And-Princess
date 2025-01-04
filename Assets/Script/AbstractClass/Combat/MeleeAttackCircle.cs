using UnityEngine;

public abstract class MeleeAttackCircle : MeleeAttack
{
    [Header("Targeting")]
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected float range;

    protected override void FindTarget()
    {
        base.targetCollider = Physics2D.OverlapCircle((Vector2) transform.position, this.range, this.targetLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, this.range);
    }
}
