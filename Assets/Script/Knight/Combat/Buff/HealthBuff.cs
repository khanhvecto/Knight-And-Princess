using System.Collections;
using UnityEngine;

public class HealthBuff : CombatBuff
{
    //Singleton
    private static HealthBuff instance;
    public static HealthBuff Instance { get => instance; }

    [Header("Stats")]
    [SerializeField] protected float healSpeed = 2f;

    protected override void Awake()
    {
        base.Awake();

        //Singleton
        if (instance != null) Debug.LogError("Only 1 HealthBuff allows to exist");
        instance = this;
    }

    //Buff detail
    protected override IEnumerator Buffing()
    {
        float startTime = 0f;
        while (startTime < this.effectTime)
        {
            this.hudObj.slider.value = (this.effectTime - startTime) / this.effectTime; //Cooldown UI

            if (KnightStats.Instance.health < KnightStats.Instance.maxEndurance)    //Restore health
            {
                KnightStats.Instance.Heal(this.healSpeed * Time.deltaTime);
            }

            startTime += Time.deltaTime;
            yield return null;
        }
    }
}