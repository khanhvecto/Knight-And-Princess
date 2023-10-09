using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    //Design pattern
    private static KnightAttack instance;
    public static KnightAttack Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;

    //Cooldown
    private float cooldownTimer = 0f;
    private bool attackable = true;

    [Header("Attack Range")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float range = 0.49f;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightAttack allow to exist!");
        instance = this;
    }

    void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for KnightAttack");
        if (this.attackPoint == null) Debug.LogError("Can't find attackPoint for KnightAttack");
    }

    void Update()
    {
        if (KnightState.Instance.controlable)
        {
            if (this.attackable)
            {
                this.SetAttack();
            }
            else
            {
                this.SetCooldown();
            }
        }
    }

    private void SetAttack()
    {
        if (InputManager.Instance.GetAttackKeyDown())    //If player press attack key
        {
            animator.SetTrigger("attacking");
            this.attackable = false;
        }
    }

    private void SetCooldown()
    {
        this.cooldownTimer += Time.deltaTime;
        if (this.cooldownTimer >= KnightState.Instance.cooldown)
        {
            this.cooldownTimer = 0f;
            this.attackable = true;
        }
    }

    //Animation functions
    public void TriggerAttack()
    {
        //Check if attack touch enemy
        Collider2D enemyHit = Physics2D.OverlapCircle(this.attackPoint.position, this.range, this.enemyLayer);
        if (enemyHit != null)
        {
            EnemyCombat enemy = enemyHit.gameObject.GetComponent<EnemyCombat>();
            if (enemy != null)
            {
                enemy.gotHit(KnightState.Instance.damage);
            }
        }
    }

    //Visualize attack range
    private void OnDrawGizmosSelected()
    {
        if (this.attackPoint != null)
        {
            Gizmos.DrawWireSphere(this.attackPoint.position, this.range);
        }
    }
}
