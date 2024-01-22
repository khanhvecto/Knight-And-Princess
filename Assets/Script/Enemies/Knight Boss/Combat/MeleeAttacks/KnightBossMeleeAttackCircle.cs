using UnityEngine;

public class KnightBossMeleeAttackCircle : MeleeAttackCircle
{
    [Header("References")]
    [SerializeField] protected Skill skillScriptableObj;

    protected override void LoadCombatStats()
    {
        base.damage = this.skillScriptableObj.damage;
        base.enduranceDecrement = this.skillScriptableObj.enduranceDecrement;
    }
}
