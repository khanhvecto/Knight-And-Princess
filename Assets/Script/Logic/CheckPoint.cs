using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool interactable = false;
    private PopupAppear popupScript;

    private void Awake()
    {
        popupScript = GetComponent<PopupAppear>();
        if(popupScript == null) Debug.LogError("Can't find PopupAppear for " + gameObject.name);
    }

    private void Update()
    {
        if(this.interactable)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                this.SetCheckPoint();
                interactable = false;
                popupScript.StopPopup();
            }
        }
    }

    private void SetCheckPoint()
    {
        Debug.Log("Set respawn");
        GamePlayLogic.Instance.SetCheckPoint(gameObject.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7 && GamePlayLogic.Instance.checkPoint != transform)
        {
            this.interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            this.interactable = false;
        }
    }
}
