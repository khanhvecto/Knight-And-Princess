using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    //Singleton
    private static KnightAttack instance;
    public static KnightAttack Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;
        // Sound
    [SerializeField] protected string attackSoundPoolTag;
    protected ObjectPooling attackSoundPoolScript;

    //Cooldown
    private bool attackable = true;

    [Header("Attack Range")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float range = 0.49f;

    private void Awake()
    {
        //Singleton
        if (instance != null) Debug.LogError("Only 1 KnightAttack allow to exist!");
        instance = this;
    }

    void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for KnightAttack");
        if (this.attackPoint == null) Debug.LogError("Can't find attackPoint for KnightAttack");
        // attack sound pool
        this.attackSoundPoolScript = GameObject.FindGameObjectWithTag(this.attackSoundPoolTag)?.GetComponent<ObjectPooling>();
        if (this.attackSoundPoolScript == null)
            Debug.LogError("Can't find attack sound pool script for KnightAttack of " + transform.parent.parent.name);
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
        if (InputManager.Instance.GetAttackKeyDown())
        {
            animator.SetTrigger("attack");
            this.attackSoundPoolScript.Get();   // Play sound on awake
        }
    }

    public void TriggerAttack()
    {
        //Check if attack touch enemy
        Collider2D enemyHit = Physics2D.OverlapCircle(this.attackPoint.position, this.range, this.enemyLayer);
        if (enemyHit == null) return;

        SlimeDamageReceiver enemy = enemyHit.gameObject.GetComponent<SlimeDamageReceiver>();
        if (enemy == null)
        {
            enemy = enemyHit.gameObject.GetComponentInChildren<SlimeDamageReceiver>();
        }

        if (enemy == null) return;

        enemy.GotHit(KnightStats.Instance.damage, transform.parent.parent, 0);
    }

    private void OnDrawGizmosSelected()
    {
        if (this.attackPoint != null)
        {
            Gizmos.DrawWireSphere(this.attackPoint.position, this.range);
        }
    }
}
