using System.Collections;
using UnityEngine;

public class InvincibleBuff : CombatBuff
{
    //Singleton
    private static InvincibleBuff instance;
    public static InvincibleBuff Instance { get => instance; }

    protected override void Awake()
    {
        base.Awake();

        //Singleton
        if (instance != null) Debug.LogError("Only 1 HealthBuff allows to exist");
        instance = this;
    }

    protected override void PerformBuff()
    {
        base.PerformBuff();
        KnightState.Instance.invincible = true;
    }
    protected override void ResetBuff()
    {
        base.PerformBuff();
        KnightState.Instance.invincible = false;
    }
}