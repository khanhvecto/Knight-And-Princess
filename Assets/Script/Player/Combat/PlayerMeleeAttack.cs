using UnityEngine;

public class PlayerMeleeAttack : MeleeAttack
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected PlayerStats statsScript;

    protected override void LoadReferences()
    {
        // Stats script
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerMeleeAttack");
    }

    protected override void LoadStats()
    {
        base.damage = this.statsScript.damage;
    }
}
