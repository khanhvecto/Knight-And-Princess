using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Scriptable Object/ New enemy stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Type")]
    public movingType moveType;

    [Header("Combat stats")]
    public float health;
    public float damage;    // should remove
    public float cooldown;  // should remove
    public float approachDistance;
    public float enduranceDecrement;    // should remove
    public int unharmedAttacksAmount;   // Can take this amount of attack without switch to hurt state, only lose health.
    public int choosableAttacksAmount;

    [Header("Attack time")] // Time from starting attack to sending damage
    public float maxDelayAttackTime;
    public float attackTime1;   // Should remove
    public float attackTime2;   // Should remove

    [Header("Targeting")]
    public float range_coeff;    //Coefficient of combat sensor range and normal sensor range

    [Header("Speed")]
    public float normalSpeed;
    public float combatSpeed;

    [Header("Move range")]
    public float horizontalMoveRange;
    public float verticalMoveRange;
}

public enum movingType
{
    walk = 100,
    flight = 200
}