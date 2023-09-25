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
    public Vector2 controlRange;
    public combatTypeList combatType;
}

public enum combatTypeList
{
    Melee, Ranged
}
