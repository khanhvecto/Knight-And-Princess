using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    //Reference
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyState stateScript;
    [SerializeField] private CombatStats statSO;

    [Header("Attack setup")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;

    //Basic stats
    private float health;
    private float damage;
    private float cooldown;

    //Other stats
    private float cooldownTimer;

    // Start is called before the first frame update
    private void Start()
    {
        //Get stats
        health = statSO.health;
        damage = statSO.damage;
        cooldown = statSO.cooldown;
        //Set cooldown counter
        cooldownTimer = Time.time;
    }

    //Set attack

    public void tryAttack()
    {
        //If ready to attack
        if(Time.time - cooldownTimer >= cooldown)
        {
            animator.SetTrigger("attack");
            cooldownTimer = Time.time;  //Reset cooldown timer
        }
    }

    public void attack()
    {
        if(stateScript.targetColl == null) { return; }
        LayerMask layerMask = 1 << stateScript.targetColl.gameObject.layer;
        Collider2D hostileColl = Physics2D.OverlapCircle(transform.position, attackRange, layerMask);
        if (hostileColl != null)
        {
            KnightHurt.Instance.GotAttack(damage, attackPoint);
        }
    }

    //...

    public void gotHit(float damage)
    {
        animator.SetTrigger("gotHit");
        stateScript.setAttackState(true);
        health -= damage;
        if (health <= 0)
        {
            gameObject.layer = 9; //Dead layer
            animator.SetBool("isDead", true);
        }
    }
    private void destroyObject()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
