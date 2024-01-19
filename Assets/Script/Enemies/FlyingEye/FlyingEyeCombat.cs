using System.Collections;
using UnityEngine;

public class FlyingEyeCombat: SlimeDamageSender
{ 
    protected override void Attack(int attackType)
    {
        base.Attack(attackType);
        this.FlightToEnemy(attackType);
    }

    protected void FlightToEnemy(int attackType)
    {
        if (base.stateScript.targetColl == null) return;

        // Flight velocity based on distance and time need to fly
        var direction = base.stateScript.targetColl.transform.position - transform.position;
        switch (attackType)
        {
            case 1:
                base.stateScript.rb2D.velocity = direction / base.statScript.attackTime1;
                break;
            case 2:
                base.stateScript.rb2D.velocity = direction / base.statScript.attackTime2;
                break;
            default:
                break;
        }
    }
}