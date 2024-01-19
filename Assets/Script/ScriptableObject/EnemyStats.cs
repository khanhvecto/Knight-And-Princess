using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Scriptable Object/ New enemy stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Type")]
    public movingType moveType;

    [Header("Combat stats")]
    public float health;
    public float damage;
    public float cooldown;
    public float distance;
    public float enduranceDecrement;
    public int unharmedAttacksAmount;   // Can take this amount of attack without switch to hurt state, only lose health.
    public int choosableAttacksAmount;

    [Header("Attack time")] // Time from starting attack to sending damage
    public float maxDelayAttackTime;
    public float attackTime1;
    public float attackTime2;

    [Header("Movement stats")]
    public float range_coeff;    //Coefficient of combat sensor range and normal sensor range
    public float normalSpeed;
    public float combatSpeed;
    public float horizontalMoveRange;
    public float verticalMoveRange;
}

public enum movingType
{
    walk = 100,
    flight = 200
}