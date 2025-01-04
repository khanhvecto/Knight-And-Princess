using UnityEngine;

public class DamageBuff : CombatBuff
{
    //Singleton
    private static DamageBuff instance;
    public static DamageBuff Instance { get => instance; }

    protected override void Awake()
    {
        base.Awake();

        //Singleton
        if (instance != null) Debug.LogError("Only 1 HealthBuff allows to exist");
        instance = this;
    }

    //Detail
    protected override void PerformBuff()
    {
        base.PerformBuff();
        KnightStats.Instance.damage *= 2;
    }
    protected override void ResetBuff()
    {
        base.PerformBuff();
        KnightStats.Instance.damage /= 2;
    }
}