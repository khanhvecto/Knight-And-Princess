using UnityEngine;

public abstract class MeleeAttackBox : MeleeAttack
{
    [Header("Targeting")]
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Vector2 range;

    protected override void FindTarget()
    {
        base.targetCollider = Physics2D.OverlapBox((Vector2)transform.position, this.range, this.targetLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, this.range);
    }
}
