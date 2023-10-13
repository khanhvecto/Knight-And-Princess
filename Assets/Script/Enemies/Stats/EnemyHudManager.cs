using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHudManager : HudManager
{
    [Header("References")]
    [SerializeField] private EnemyState stateScript;

    [Header("Item list")]
    [SerializeField] private string hp = "HpBar";

    //States
    private bool displayingHp = false;

    protected override void Start()
    {
        this.LoadReferences();
    }
    private void LoadReferences()
    {
        //HudObj for base
        if (base.hudObj == null) Debug.LogError("Can't find HUD Object for EnemyHudManager of " + transform.parent.name);
        //stateScript
        this.stateScript = transform.parent.GetComponent<EnemyState>();
        if (this.stateScript == null) Debug.LogError("Can't find stateScript for EnemyHudManager of " + transform.parent.name);
    }

    private void Update()
    {
        this.CheckDisplayHp();
    }

    private void CheckDisplayHp()   //Check if need to show Hp bar
    {
        if(this.stateScript.isCombating && !this.displayingHp)
        {
            base.Display(this.hp, true);
            this.displayingHp = true;
        }
        else if(!this.stateScript.isCombating && this.displayingHp)
        {
            base.Display(this.hp, false);
            this.displayingHp = false;
        }
    }
}