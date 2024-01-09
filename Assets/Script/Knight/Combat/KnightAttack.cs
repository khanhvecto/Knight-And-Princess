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
                this.CheckAttack();
            }
        }
    }

    private void CheckAttack()
    {
        if (InputManager.Instance.GetAttackKeyDown())    //If player press attack key
        {
            animator.SetTrigger("attack");
        }
    }

    //Animation functions
    public void TriggerAttack()
    {
        //Check if attack touch enemy
        Collider2D enemyHit = Physics2D.OverlapCircle(this.attackPoint.position, this.range, this.enemyLayer);
        if (enemyHit != null)
        {
            SlimeCombat enemy = enemyHit.gameObject.GetComponent<SlimeCombat>();
            if (enemy != null)
            {
                enemy.GotHit(KnightStats.Instance.damage);
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
