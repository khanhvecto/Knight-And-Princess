using UnityEngine;

public class BulletDamage : DamagableObj
{
    [Header("------ BULLET DAMAGE ------")]
    [SerializeField] protected BulletDespawn bulletDespawn;

    protected override void OnTouchingTarget()
    {
        base.OnTouchingTarget();
        this.bulletDespawn.ReleaseObject();
    }

    public void SetDamage(float damage)
    {
        base.damage = damage;
    }
    public void SetEnduranceDecrement(float enduranceDecrement)
    {
        base.enduranceDecrement = enduranceDecrement;
    }
}
