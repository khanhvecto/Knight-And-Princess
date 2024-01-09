using UnityEngine;

public class SlimeStats : MonoBehaviour, HpBarInterface
{
    [Header("References")]
    [SerializeField] private CombatStats statSO;
    [SerializeField] private SlimeState stateScript;

    [Header("Combat range")]
    [SerializeField] public Vector2 combatRangeSize = new Vector2(2.45f, 1.68f);
    [SerializeField] public Vector2 combatRangeOffset = new Vector2(-0.93f, 0.52f);
    public float RANGE_COEFF;

    [Header("Combat stats")]
    [SerializeField] public float health;
    [SerializeField] public float damage;
    [SerializeField] public float enduranceDecrement;

    private void Start()
    {
        this.CheckReferences();
        this.LoadStats();
    }
    private void CheckReferences()  //Check if any reference not found
    {
        //CombatStats
        if (this.statSO == null) Debug.LogError("Can't find CombatStats for EnemyStats of " + transform.parent.name);
        //StateScript
        this.stateScript = transform.parent.GetComponent<SlimeState>();
        if (this.stateScript == null) Debug.LogError("Can't find stateScript for EnemyStats of " + transform.parent.name);
    }
    private void LoadStats()    //Load stats from SO
    {
        this.RANGE_COEFF = this.statSO.range_coeff;
        this.health = this.statSO.health;
        this.damage = this.statSO.damage;
        this.enduranceDecrement = this.statSO.enduranceDecrement;
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

    private void OnDrawGizmosSelected()
    {
        //Draw combat range
        if (this.stateScript.isCombating)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + this.combatRangeOffset, this.combatRangeSize * this.RANGE_COEFF);
        }
        else
        {
            Gizmos.DrawWireCube((Vector2)transform.position + this.combatRangeOffset, this.combatRangeSize);
        }
    }
}
