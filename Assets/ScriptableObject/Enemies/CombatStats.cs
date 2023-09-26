using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "ScriptableObject/CombatStat")]
public class CombatStats : ScriptableObject
{
    [Header("Combat stats")]
    public float health;
    public float damage;
    public float cooldown;
    public combatTypeList combatType;
    public float distance;

    [Header("Movement stats")]
    public float range_coeff;    //Coefficient of combat sensor range and normal sensor range
    public float normalSpeed;
    public float combatSpeed;
    public float jumpForce;
}

public enum combatTypeList
{
    Melee = 1, 
    Ranged = 2
}
