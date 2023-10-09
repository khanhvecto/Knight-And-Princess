using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightStats : MonoBehaviour
{
    //Design pattern
    private static KnightStats instance;
    public static KnightStats Instance { get => instance; }

    //Endurance
        //Stats
    public float minEndurance = 0f;
    public float maxEndurance = 100f;
    public float endurance = 100f;
        //Chaning speed
    public float enduranceLooseSpeed = 8f;
    public float enduranceRestoreSpeed = 8f;
        //Stage
    public bool restoringEndurance = true;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightStat allow to exist!");
        instance = this;
    }

    private void Update()
    {
        //Endurance
        this.SetEndurance();
    }

    //Endurance
    private void SetEndurance()
    {
        //Restoring
        if(this.restoringEndurance)
        {
            if (this.endurance < this.maxEndurance)
            {
                this.RestoringEndurance(this.enduranceRestoreSpeed);
            }
        }
        //Loosing
        else
        {
            if (this.endurance > this.minEndurance)
            {
                this.DecreasingEndurance(this.enduranceLooseSpeed);
            }
        }
    }
    private void DecreasingEndurance(float value)
    {
        this.endurance -= value * Time.deltaTime;
        if (this.endurance < this.minEndurance)  //Limit: endurance can not smaller than min value
        {
            this.endurance = this.minEndurance;
        }
    }
    private void RestoringEndurance(float value)
    {
        this.endurance += value * Time.deltaTime;
        if (this.endurance > this.maxEndurance)  //Limit: endurance can not exceed max value
        {
            this.endurance = this.maxEndurance;
        }
    }
}
