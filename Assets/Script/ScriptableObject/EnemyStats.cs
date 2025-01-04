using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Scriptable Object/ New enemy stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Type")]
    public MovingType moveType;

    [Header("Combat stats")]
    public float health;
    public float approachDistance;
    public int unharmedAttacksAmount;   // Can take this amount of attack without switch to hurt state, only lose health.
    public int choosableAttacksAmount;

    [Header("Attack time")] // Time from starting attack to sending damage
    public float maxDelayAttackTime;

    [Header("Targeting")]
    public float range_coeff;    //Coefficient of combat sensor range and normal sensor range

    [Header("Speed")]
    public float normalSpeed;
    public float combatSpeed;

    [Header("Move range")]
    public float horizontalMoveRange;
    public float verticalMoveRange;
}

public enum MovingType
{
    walk = 100,
    flight = 200
}