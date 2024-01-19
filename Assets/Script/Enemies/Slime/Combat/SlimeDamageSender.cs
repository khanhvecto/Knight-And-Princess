using UnityEngine;

public class SlimeDamageSender : SlimeDamageReceiver
{
    [Header("Attack setup")]
    [SerializeField] protected Transform attackPoint1;
    [SerializeField] protected Transform attackPoint2;
    [SerializeField] protected float attackRange1;
    [SerializeField] protected float attackRange2;

    //Set attack
    public void TryAttack() 
    {
        //Chose a random attack
        int randNum = Random.Range(1, 3);
        this.Attack(randNum);
    }
    protected virtual void Attack(int attackType)
    {
        this.stateScript.animator.SetTrigger("attack" + attackType);
    }

    public void SendDamage(int type)
    {
        if(this.stateScript.targetColl == null) return;

        // Define attack type
        Transform attackPoint;
        float attackRange;
        if (type == 1)
        {
            attackPoint = this.attackPoint1;
            attackRange = this.attackRange1;
        }
        else if (type == 2)
        {
            attackPoint = this.attackPoint2;
            attackRange = this.attackRange2;
        }
        else return;

        if (this.stateScript.targetColl == null) return;
        LayerMask layerMask = 1 << stateScript.targetColl.gameObject.layer;
        Collider2D hostileColl = Physics2D.OverlapCircle((Vector2) attackPoint.position, attackRange, layerMask);
        if (hostileColl != null)
        {
            KnightHurt.Instance.GotAttack(this.statScript.damage, attackPoint, this.statScript.enduranceDecrement);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.attackPoint2.position, this.attackRange2);
        Gizmos.DrawWireSphere(this.attackPoint1.position, this.attackRange1);
    }
}
