using UnityEngine;

public class SlimeStats : MonoBehaviour, HpBarInterface
{
    [Header("References")]
    [SerializeField] private EnemyStats statSO;

    [Header("Combat range")]
    public Vector2 combatRangeSize = new Vector2(2.45f, 1.68f);
    public Vector2 combatRangeOffset = new Vector2(-0.93f, 0.52f);
    public float RANGE_COEFF;

    [Header("Combat stats")]
    public float health;
    public float damage;
    public float enduranceDecrement;
    public float distance;
    public int attackTypeNumber;
    public int unharmedAttacksAmount;

    [Header("Attacks")]
    public float maxAttackDelayTime;
    public float attackTime1;
    public float attackTime2;

    [Header("Movement stats")]
    public float approachSpeed;
    public float normalSpeed;
    public movingType moveType;

    private void Start()
    {
        this.CheckReferences();
        this.LoadStats();
    }
    private void CheckReferences()  //Check if any reference not found
    {
        //CombatStats
        if (this.statSO == null) Debug.LogError("Can't find CombatStats for EnemyStats of " + transform.parent.name);
    }
    private void LoadStats()    //Load stats from SO
    {
        this.RANGE_COEFF = this.statSO.range_coeff;
        this.health = this.statSO.health;
        this.damage = this.statSO.damage;
        this.enduranceDecrement = this.statSO.enduranceDecrement;
        this.normalSpeed = this.statSO.normalSpeed;
        this.approachSpeed = this.statSO.combatSpeed;
        this.distance = this.statSO.distance;
        this.attackTime1 = this.statSO.attackTime1;
        this.attackTime2 = this.statSO.attackTime2;
        this.moveType = this.statSO.moveType;
        this.maxAttackDelayTime = this.statSO.maxDelayAttackTime;
        this.attackTypeNumber = this.statSO.choosableAttacksAmount;
        this.unharmedAttacksAmount = this.statSO.unharmedAttacksAmount;
    }

    //HpBarInterface
    public float GetHp()
    {
        return this.health;
    }

    public float GetMaxHp()
    {
        return this.statSO.health;
    }
}
