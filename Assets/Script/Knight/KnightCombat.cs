using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCombat : MonoBehaviour
{
    [SerializeField] private KnightMovement knightMovement;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask combatLayer;
    [SerializeField] Rigidbody2D rb2D;

    [Header("Combat stat")]
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float touchDamage;

    [Header("Attack range")]
    [SerializeField] private Transform attackSource;
    [SerializeField] private float attackRange;
    private float timeCount = 0f;
    private bool attackable = true;

    private void Update()
    {
        if(!attackable)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= cooldown)
            {
                timeCount = 0f;
                attackable = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackable)
        {
            attackable = false;
            animator.SetTrigger("attacking");
        }
    }

    private void setAttack()
    {
        Collider2D enemyHit = Physics2D.OverlapCircle(attackSource.position, attackRange, combatLayer);
        if(enemyHit != null)
        {
            EnemyCombat enemy = enemyHit.gameObject.GetComponent<EnemyCombat>();
            if(enemy != null)
            {
                enemy.gotHit(damage);
            }
        }
    }

    public void touchEnemy()
    {
        gotHurt(touchDamage);
    }

    public void gotHurt(float damage)
    {
        health -= damage;
        animator.SetTrigger("gotHurt");
    }

    private void OnDrawGizmosSelected() //Visualize attackRange on editor
    {
        if (attackSource != null)
        {
            Gizmos.DrawWireSphere(attackSource.position, attackRange);
        }
    }
}
