using UnityEngine;

public class SlimeCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SlimeState stateScript;
    [SerializeField] private SlimeStats statScript;

    [Header("Attack setup")]
    [SerializeField] private Transform attackPoint1;
    [SerializeField] private Transform attackPoint2;
    [SerializeField] private float attackRange;

    private void Start()
    {
        this.LoadReferences();
    }
    private void LoadReferences()   //Check if any reference not found
    {
        //EnemyStats
        this.statScript = transform.Find("Stats").GetComponent<SlimeStats>();
        if (this.statScript == null) Debug.LogError("Can't find EnemyStats for EnemyCombat of " + gameObject.name);
        //Animator
        this.animator = transform.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find Animator for EnemyCombat of " + gameObject.name);
        //EnemyState
        this.stateScript = transform.GetComponent<SlimeState>();
        if (this.stateScript == null) Debug.LogError("Can't find EnemyState for EnemyCombat of " + gameObject.name);
    }

    //Set attack
    public void TryAttack() 
    {
        //Chose a random attack
        int randNum = Random.Range(1, 3);
        if (randNum == 1) this.Attack1();
        else this.Attack2();
    }
    protected void Attack1()
    {
        animator.SetTrigger("attack1");
    }
    protected void Attack2()
    {
        animator.SetTrigger("attack2");
    }

    public void SendDamage(int type)
    {
        if(stateScript.targetColl == null) { return; }

        // Define attack type
        Transform attackPoint;
        if (type == 1) attackPoint = this.attackPoint1;
        else if (type == 2) attackPoint = this.attackPoint2;
        else return;

        LayerMask layerMask = 1 << stateScript.targetColl.gameObject.layer;
        Collider2D hostileColl = Physics2D.OverlapCircle((Vector2) attackPoint.position, this.attackRange, layerMask);
        if (hostileColl != null)
        {
            KnightHurt.Instance.GotAttack(this.statScript.damage, attackPoint, this.statScript.enduranceDecrement);
        }
    }

    //Got hurt
    public void GotHit(float damage)
    {
        animator.SetTrigger("gotHit");
        stateScript.setAttackState(true);
        this.statScript.health -= damage;
        if (this.statScript.health <= 0)
        {
            gameObject.layer = 9; //Dead layer
            animator.SetBool("isDead", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.attackPoint2.position, this.attackRange);
        Gizmos.DrawWireSphere(this.attackPoint1.position, this.attackRange);
    }
}
