using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : InteractableObject
{
    protected bool settedCheckPoint = false;

    protected override void Update()
    {
        base.Update();
        if (!base.interactable) return;
        if (!base.interacted) return;
        if (settedCheckPoint) return;

        this.SetCheckPoint();

    }

    protected virtual void SetCheckPoint()
    {
        //Setting
        GamePlayLogic.Instance.SetCheckPoint(gameObject.transform);
        this.settedCheckPoint = true;

        //Show in navigator
        NavigatorManager.Instance.ShowNavigator("Check point\nsetted!");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && GamePlayLogic.Instance.checkPoint != transform)
        {
            base.interactable = true;
            if (base.activeState) base.SetPopUpShowing(true);
        }
    }

    public virtual void ResetObject()
    {
        base.ResetInteract();
        this.settedCheckPoint=false;
    }
}