using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatHud : HudManager
{
    [Header("Item list")]
    [SerializeField] private string hp = "HpBar";
    [SerializeField] private string endurance = "EnduranceBar";

    //Display state
    private bool displayingHp = true;
    private bool displayingEndurance = false;

    private void Update()
    {
        //Set display Bars
        this.CheckDisplayHp();
        this.CheckDisplayEndurance();
    }

    private void CheckDisplayHp()
    {
        if(KnightState.Instance.alive && !this.displayingHp) //If need to display
        {
            base.Display(this.hp, true);
            this.displayingHp = true;
        } 
        else if(!KnightState.Instance.alive && this.displayingHp)    //If need to stop display
        {
            base.Display(this.hp, false);
            this.displayingHp= false;
        }
    }

    private void CheckDisplayEndurance()
    {
        if(KnightStats.Instance.endurance < KnightStats.Instance.maxEndurance)
        {
            if(!this.displayingEndurance) //If need to display
            {
                base.Display(this.endurance, true);
                this.displayingEndurance = true;
            }
        }
        else
        {
            if(this.displayingEndurance)  //If need to stop display
            {
                base.Display(this.endurance, false);
                this.displayingEndurance = false;
            }
        }
    }
}