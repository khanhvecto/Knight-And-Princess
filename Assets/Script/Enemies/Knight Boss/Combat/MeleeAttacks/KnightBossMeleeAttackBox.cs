using UnityEngine;

public class KnightBossMeleeAttackBox : MeleeAttackBox
{
    [Header("References")]
    [SerializeField] protected Skill skillScriptableObject;

    protected override void LoadCombatStats()
    {
        base.damage = this.skillScriptableObject.damage;
        base.enduranceDecrement = this.skillScriptableObject.enduranceDecrement;
    }
}
