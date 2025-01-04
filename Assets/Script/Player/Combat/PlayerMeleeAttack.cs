using UnityEngine;

public class PlayerMeleeAttack : MeleeAttackCircle
{
    [Header("References")]
    [SerializeField] protected PlayerStats statsScript;

    protected override void LoadCombatStats()
    {
        base.damage = this.statsScript.damage;
    }
}