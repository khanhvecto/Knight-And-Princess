using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAppear : MonoBehaviour
{
    private GameObject popupTextObj;

    private void Awake()
    {
        popupTextObj = transform.Find("Popup text").gameObject;
        if (popupTextObj == null) Debug.LogWarning("Can't find popup for " + gameObject.name);
    }

    //Stop popup
    public void StopPopup()
    {
        this.popupTextObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)    //Knight layer
        {
            if (GamePlayLogic.Instance.checkPoint != gameObject.transform)
            {
                this.popupTextObj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)    //Knight layer
        {
            this.popupTextObj.SetActive(false);
        }
    }
}
