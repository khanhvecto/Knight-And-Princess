using UnityEngine;

public abstract class PopupAppear : MonoBehaviour
{
    protected GameObject popupObj;
    protected bool isPopupActive = true;
    
    protected virtual void Awake()
    {
        this.LoadReferences();
    }
    protected virtual void LoadReferences()
    {
        popupObj = transform.Find("Popup").gameObject;
        if (popupObj == null) Debug.LogWarning("Can't find popup for " + gameObject.name);
    }

    protected virtual void SetPopUpShowing(bool state)
    {
        this.popupObj.SetActive(state);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (this.isPopupActive) this.SetPopUpShowing(true);
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (this.isPopupActive) this.SetPopUpShowing(false);
        }
    }
}
