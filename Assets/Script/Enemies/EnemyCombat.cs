using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyState stateScript;
    [SerializeField] private EnemyStats statScript;

    [Header("Attack setup")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;

    //Cooldown
    private float cooldownTimer;

    private void Start()
    {
        this.LoadReferences();
        //Set cooldown counter
        this.cooldownTimer = Time.time;
    }
    private void LoadReferences()   //Check if any reference not found
    {
        //EnemyStats
        this.statScript = transform.Find("Stats").GetComponent<EnemyStats>();
        if (this.statScript == null) Debug.LogError("Can't find EnemyStats for EnemyCombat of " + gameObject.name);
        //Animator
        this.animator = transform.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find Animator for EnemyCombat of " + gameObject.name);
        //EnemyState
        this.stateScript = transform.GetComponent<EnemyState>();
        if (this.stateScript == null) Debug.LogError("Can't find EnemyState for EnemyCombat of " + gameObject.name);
    }

    //Set attack
    public void tryAttack()
    {
        //If ready to attack
        if(Time.time - this.cooldownTimer >= this.statScript.cooldown)
        {
            animator.SetTrigger("attack");
            this.cooldownTimer = Time.time;  //Reset cooldown timer
        }
    }

    public void attack()
    {
        if(stateScript.targetColl == null) { return; }
        LayerMask layerMask = 1 << stateScript.targetColl.gameObject.layer;
        Collider2D hostileColl = Physics2D.OverlapCircle(this.attackPoint.position, this.attackRange, layerMask);
        if (hostileColl != null)
        {
            KnightHurt.Instance.GotAttack(this.statScript.damage, this.attackPoint, this.statScript.enduranceDecrement);
        }
    }

    //Got hurt
    public void gotHit(float damage)
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
        Gizmos.DrawWireSphere(this.attackPoint.position, this.attackRange);
    }
}
