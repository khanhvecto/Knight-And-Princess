using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OrbCollector : CollectableObj
{
    [SerializeField] protected CombatBuff.BuffType type;

    protected override IEnumerator CollectObj()
    {
        switch(this.type)
        {
            case CombatBuff.BuffType.healthBuff:
                HealthBuff.Instance.ReceiveBuff(1);
                break;
            case CombatBuff.BuffType.shieldBuff:
                ShieldBuff.Instance.ReceiveBuff(1);
                break;
            case CombatBuff.BuffType.damageBuff:
                DamageBuff.Instance.ReceiveBuff(1);
                break;
            default:
                InvincibleBuff.Instance.ReceiveBuff(1);
                break;
        }
        yield return null;
    }
}