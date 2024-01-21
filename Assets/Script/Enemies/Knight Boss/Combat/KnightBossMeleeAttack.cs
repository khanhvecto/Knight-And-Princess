using UnityEngine;

public class KnightBossMeleeAttack : MeleeAttack
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected Skill skillScriptableObj;

    protected override void LoadReferences()
    {
        // skill scriptable object
        if (this.skillScriptableObj == null)
            Debug.LogError("Can't find skill scriptable object for MeleeAttack of " + transform.parent.parent.parent.name);
    }

    protected override void LoadStats()
    {
        base.damage = this.skillScriptableObj.damage;
        base.enduranceDecrement = this.skillScriptableObj.enduranceDecrement;
    }
}
