using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : InteractableObject
{
    protected bool settedCheckPoint = false;

    protected override void Update()
    {
        base.Update();
        if (base.interacted && !settedCheckPoint)
        {
            this.SetCheckPoint();
            this.settedCheckPoint = true;
        }
    }

    protected virtual void SetCheckPoint()
    {
        Debug.Log("Set respawn");
        GamePlayLogic.Instance.SetCheckPoint(gameObject.transform);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && !base.interacted && GamePlayLogic.Instance.checkPoint != transform)
        {
            base.interactable = true;
            if (base.activeState) base.SetPopUpShowing(true);
        }
    }

    public virtual new void ResetObject()
    {
        base.ResetObject();
        this.settedCheckPoint=false;
    }
}
