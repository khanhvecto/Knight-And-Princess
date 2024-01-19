using UnityEngine;

public class KnightBossStateChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SlimeState stateScript;
    [SerializeField] protected SlimeStats statsScript;

    void Start()
    {
        this.CheckReferences();
    }

    protected void CheckReferences()
    {
        if (this.stateScript == null) Debug.LogError("Can't find state script for KnightBossStateChanger of " + transform.parent.name);
        if (this.statsScript == null) Debug.LogError("Can't find stats script for KnightBossStateChanger of " + transform.parent.name);
    }

    void Update()
    {
        this.CheckStateChange();
    }

    protected void CheckStateChange()
    {
        if (!this.stateScript.isCombating && this.IsInAttackRange())    //If player in combat Range
        {
            this.stateScript.SetAttackState(true);
        }
        else if (this.stateScript.isCombating && !this.IsInAttackRange())   //If player is out of combat Range
        {
            this.stateScript.SetAttackState(false);
        }
    }

    protected bool IsInAttackRange()
    {
        if (!this.stateScript.isCombating)
        {
            this.stateScript.targetColl = Physics2D.OverlapBox((Vector2)transform.position + this.statsScript.combatRangeOffset, this.statsScript.combatRangeSize, 0f, this.stateScript.knightLayer);
        }
        else
        {
            this.stateScript.targetColl = Physics2D.OverlapBox((Vector2)transform.position + this.statsScript.combatRangeOffset, this.statsScript.combatRangeSize * this.statsScript.RANGE_COEFF, 0f, this.stateScript.knightLayer);
        }
        return this.stateScript.targetColl;
    }
}
