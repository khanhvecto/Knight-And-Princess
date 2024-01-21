using UnityEngine;

public class KnightBossStats : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [SerializeField] private EnemyStats statSO;
    [SerializeField] protected KnightBossHUDBar _HUDBarScript;
    public Rigidbody2D rb2D;
    public Animator animator;
    public LayerMask knightLayer;

    [Header("--- STATS ---")]
    [Header("Combat range")]
    public Vector2 combatRangeSize;
    public Vector2 combatRangeOffset;
    public float RANGE_COEFF;
    public float distance;
    [Header("Combat stats")]
    protected float health;
    public float Health { get => health; }
    public float maxHealth;
    [Header("Damage")]
    public float damage;
    public float enduranceDecrement;
    [Header("Attack")]
    public int attackTypeNumber;
    public int unharmedAttacksAmount;
    public float maxAttackDelayTime;
    [Header("Movement")]
    public float approachSpeed;
    [Header("Target")]
    public Collider2D targetColl;

    [Header("--- STATES ---")]
    [Header("Combat")]
    public bool isCombating = false;
    [Header("Movement")]
    public bool facingLeft;

    private void Start()
    {
        this.LoadStats();
        this.InitHealthBar();
    }

    private void LoadStats()
    {
        this.RANGE_COEFF = this.statSO.range_coeff;
        this.health = this.statSO.health;
        this.maxHealth = this.statSO.health;
        this.damage = this.statSO.damage;
        this.enduranceDecrement = this.statSO.enduranceDecrement;
        this.approachSpeed = this.statSO.combatSpeed;
        this.distance = this.statSO.distance;
        this.maxAttackDelayTime = this.statSO.maxDelayAttackTime;
        this.attackTypeNumber = this.statSO.choosableAttacksAmount;
        this.unharmedAttacksAmount = this.statSO.unharmedAttacksAmount;
    }

    protected void InitHealthBar()
    {
        this._HUDBarScript.SetHealthBarMaxValue(this.maxHealth);
        this._HUDBarScript.SetHealthBarMinValue(0);
        this._HUDBarScript.SetHealthBarValue(this.Health);
    }

    public void SetHealthValue(float value)
    {
        if (value <= 0)
            this.health = 0;
        else if (value >= this.maxHealth)
            this.health = this.maxHealth;
        else
            this.health = value;

        this._HUDBarScript.SetHealthBarValue(this.Health);
    }

    public void SetAttackState(bool state)
    {
        this.animator.SetBool("attackState", state);
        this.isCombating = state;
    }

    private void OnDrawGizmos()
    {
        //Draw combat range
        if (this.isCombating)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + this.combatRangeOffset, this.combatRangeSize * this.RANGE_COEFF);
        }
        else
        {
            Gizmos.DrawWireCube((Vector2)transform.position + this.combatRangeOffset, this.combatRangeSize);
        }
    }
}
