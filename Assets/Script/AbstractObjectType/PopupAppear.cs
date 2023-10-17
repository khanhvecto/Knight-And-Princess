using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupAppear : MonoBehaviour
{
    protected GameObject popupTextObj;
    protected bool activeState = true;

    protected virtual void Start()
    {
        popupTextObj = transform.Find("Popup").gameObject;
        if (popupTextObj == null) Debug.LogWarning("Can't find popup for " + gameObject.name);
    }

    protected virtual void SetPopUpShowing(bool state)
    {
        this.popupTextObj.SetActive(state);
    }
}
