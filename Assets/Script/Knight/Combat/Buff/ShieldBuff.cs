using System.Collections;
using UnityEngine;

public class ShieldBuff : CombatBuff
{
    //Singleton
    private static ShieldBuff instance;
    public static ShieldBuff Instance { get => instance; }

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
        KnightState.Instance.indefatigable = true;
    }
    protected override IEnumerator Buffing()
    {
        float startTime = 0;
        while (startTime < base.effectTime)
        {
            this.hudObj.slider.value = (base.effectTime - startTime) / this.effectTime; //Cooldown UI

            if (KnightStats.Instance.endurance < KnightStats.Instance.maxEndurance) //Restore endurance if not full
            {
                KnightBlock.Instance.RestoringEndurance(KnightStats.Instance.enduranceRestoreSpeed);
            } 

            startTime += Time.deltaTime;
            yield return null;
        }
    }
    protected override void ResetBuff()
    {
        base.ResetBuff();
        KnightState.Instance.indefatigable = false;
    }
}