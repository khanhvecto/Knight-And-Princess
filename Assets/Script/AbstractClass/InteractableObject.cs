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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            this.interactable = true;
            if (base.activeState) base.SetPopUpShowing(true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            this.interactable = false;
            if (base.activeState) base.SetPopUpShowing(false);
        }
    }

    protected virtual void ResetInteract()    //Make Objectable can interact again like a new one
    {
        this.interacted = false;
        base.activeState = true;
    }
}