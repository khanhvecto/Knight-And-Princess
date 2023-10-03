using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCombat : MonoBehaviour
{
    private static KnightCombat instance;
    public static KnightCombat Instance { get => instance; }

    [Header("Reference")]
    [SerializeField] LayerMask combatLayer;
    private Animator animator;

    [Header("Attack range")]
    [SerializeField] private Transform attackSource;
    [SerializeField] private float attackRange;
    private float timeCount = 0f;
    private bool attackable = true;
    private bool blocking = false;

    [Header("Roll")]
    [SerializeField] private float rollForce= 13f;

    private void Awake()
    {
        //Design pattern
        if (KnightCombat.instance != null) Debug.LogError("Only 1 KnightCombat allow to exist");
        instance = this;

        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(!attackable)
        {
            this.SetCooldown();
        }

        this.SetAction();
    }

    private void SetAction()
    {
        //Attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackable)
        {
            attackable = false;
            animator.SetTrigger("attacking");
        }
        //Block and roll(dash)
        else if (KnightState.Instance.controlable) 
        {
            //Block
            if (!blocking && Input.GetKeyDown(KeyCode.Mouse1))
            {
                this.startBlock();
            }
            else if (blocking && Input.GetKeyUp(KeyCode.Mouse1))
            {
                this.endBlock();
            }
            //Roll
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.SetTrigger("roll");
            }
        }
    }

    //Attack

    private void setAttack()
    {
        Collider2D enemyHit = Physics2D.OverlapCircle(attackSource.position, attackRange, combatLayer);
        if(enemyHit != null)
        {
            EnemyCombat enemy = enemyHit.gameObject.GetComponent<EnemyCombat>();
            if(enemy != null)
            {
                enemy.gotHit(KnightState.Instance.damage);
            }
        }
    }

    //Block

    public void startBlock()
    {
        animator.SetTrigger("block");
        animator.SetBool("blocking", true);
        this.blocking = true;
        KnightState.Instance.vulnerable = false;
    }

    public void endBlock()
    {
        animator.SetBool("blocking", false);
        this.blocking = false;
        KnightState.Instance.vulnerable = true;
    }

    //Roll

    public void StartRoll()
    {
        KnightState.Instance.vulnerable = false;
        Physics2D.IgnoreLayerCollision(7, 8, true);   //Ignore collide of Knight and Enemy
        //Push body toward
        KnightState.Instance.StopControlable();
        KnightState.Instance.rb2D.gravityScale = 0f;
        if(KnightState.Instance.facingRight)
        {
            KnightState.Instance.rb2D.velocity = Vector2.right * rollForce;
        }
        else
        {
            KnightState.Instance.rb2D.velocity = Vector2.left * rollForce;
        }
    }

    public void EndRoll()
    {
        KnightState.Instance.vulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, false); //Reset collide of Knight and Enemy
        KnightState.Instance.SetControlable();
        KnightState.Instance.rb2D.gravityScale = 5f;
    }

    //Hurt

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)    //Layer of enemy
        {
            gotHurt(KnightState.Instance.touchDamage, collision.transform);
        }
    }

    public void gotHurt(float damage, Transform damageSource)
    {
        if (KnightState.Instance.vulnerable)
        {
            //Check if need to flip
            if((damageSource.position.x > transform.position.x && !KnightState.Instance.facingRight) ||
                (damageSource.position.x < transform.position.x && KnightState.Instance.facingRight))
            {
                KnightState.Instance.flip();
            }

            KnightState.Instance.health -= damage;
            KnightState.Instance.setFallBack();
        }
    }

    public void gotHurt(float damage)
    {
        if (KnightState.Instance.vulnerable)
        {
            KnightState.Instance.health -= damage;
            KnightState.Instance.setFallBack();
        }
    }

    public void checkDead() //Event animation
    {
        if (KnightState.Instance.health <= 0)
        {
            KnightState.Instance.setDead();
        }
    }

    //Other

    private void SetCooldown()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= KnightState.Instance.cooldown)
        {
            timeCount = 0f;
            attackable = true;
        }
    }
    private void OnDrawGizmosSelected() //Visualize attackRange on editor
    {
        if (attackSource != null)
        {
            Gizmos.DrawWireSphere(attackSource.position, attackRange);
        }
    }
}
