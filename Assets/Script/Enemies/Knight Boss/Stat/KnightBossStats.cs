using UnityEngine;

public class KnightBossStats : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected EnemyStats statSO;
    [SerializeField] protected KnightBossHUDBar _HUDBarScript;
    public Rigidbody2D rb2D;
    public Animator animator;
    public LayerMask knightLayer;

    [Header("--- STATS ---")]
    [Header("Targeting")]
    public Vector2 combatRangeSize;
    public Vector2 combatRangeOffset;
    [Header("Normal range")]
    public float defaultHorizontalMoveRange;
    public float defaultVerticalMoveRange;
    [Header("Combat range")]
    public float approachDistance;
    public float RANGE_COEFF;
    [Header("Movement")]
    public MovingType moveType;
    public float normalSpeed;
    public float approachSpeed;
    [Header("Combat stats")]
    protected float health;
    public float Health { get => health; }
    public float maxHealth;
    [Header("Attack")]
    public int attackTypeNumber;
    public int unharmedAttacksAmount;
    public float maxAttackDelayTime;
    [Header("Target")]
    public Collider2D targetColl;

    [Header("--- STATES ---")]
    [Header("Dead")]
    public bool isDead = false;
    [Header("Combat")]
    public bool isCombating = false;
    [Header("Movement")]
    public bool facingLeft;

    private void Start()
    {
        this.LoadStats();
        this.InitHealthBar();
    }

    protected virtual void LoadStats()
    {
        this.RANGE_COEFF = this.statSO.range_coeff;
        this.health = this.statSO.health;
        this.maxHealth = this.statSO.health;
        this.approachSpeed = this.statSO.combatSpeed;
        this.approachDistance = this.statSO.approachDistance;
        this.maxAttackDelayTime = this.statSO.maxDelayAttackTime;
        this.attackTypeNumber = this.statSO.choosableAttacksAmount;
        this.unharmedAttacksAmount = this.statSO.unharmedAttacksAmount;
        this.normalSpeed = this.statSO.normalSpeed;
        this.defaultHorizontalMoveRange = this.statSO.horizontalMoveRange;
        this.defaultVerticalMoveRange = this.statSO.verticalMoveRange;
        this.moveType = this.statSO.moveType;
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
