using UnityEngine;

public class SlimeHudManager : HudManager
{
    [Header("References")]
    [SerializeField] protected KnightBossStats statsScript;

    [Header("Item list")]
    [SerializeField] private string hp = "HpBar";

    //States
    private bool displayingHp = false;

    private void Update()
    {
        this.CheckDisplayHp();
    }

    private void CheckDisplayHp()   //Check if need to show Hp bar
    {
        if(this.statsScript.isCombating && !this.displayingHp)
        {
            base.Display(this.hp, true);
            this.displayingHp = true;
        }
        else if(!this.statsScript.isCombating && this.displayingHp)
        {
            base.Display(this.hp, false);
            this.displayingHp = false;
        }
    }
}