using UnityEngine;

public class KnightBossStateChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected KnightBossStats statsScript;

    void Update()
    {
        this.CheckStateChange();
    }

    protected void CheckStateChange()
    {

        if (!this.statsScript.isCombating && this.IsInAttackRange() && !this.statsScript.isDead)    //If player in combat Range
        {
            this.statsScript.SetAttackState(true);
        }
        else if ((this.statsScript.isCombating && !this.IsInAttackRange())
            || this.statsScript.isDead)   //If player is out of combat Range
        {
            this.statsScript.SetAttackState(false);
        }
    }

    protected bool IsInAttackRange()
    {
        if (!this.statsScript.isCombating)
        {
            this.statsScript.targetColl = Physics2D.OverlapBox((Vector2)transform.position + this.statsScript.combatRangeOffset, this.statsScript.combatRangeSize, 0f, this.statsScript.knightLayer);
        }
        else
        {
            this.statsScript.targetColl = Physics2D.OverlapBox((Vector2)transform.position + this.statsScript.combatRangeOffset, this.statsScript.combatRangeSize * this.statsScript.RANGE_COEFF, 0f, this.statsScript.knightLayer);
        }
        return this.statsScript.targetColl;
    }
}
