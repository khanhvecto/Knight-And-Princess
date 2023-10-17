using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : PopupAppear
{
    protected bool interactable = false;
    protected bool interacted = false;  //Interact state

    protected virtual void Update()
    {
        if (this.interactable && !this.interacted && InputManager.Instance.GetInteractKeyDown())
        {
            //This
            this.interacted = true;
            //Base
            base.activeState = false;
            base.SetPopUpShowing(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && !this.interacted)
        {
            this.interactable = true;
            if (base.activeState) base.SetPopUpShowing(true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && !this.interacted)
        {
            this.interactable = false;
            if (base.activeState) base.SetPopUpShowing(false);
        }
    }

    protected virtual void ResetObject()    //Make Objectable can interact again like a new one
    {
        this.interacted = false;
        base.activeState = true;
    }
}
