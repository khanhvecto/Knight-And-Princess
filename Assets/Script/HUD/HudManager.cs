using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HudManager: MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform hudObj;

    protected abstract void Start();    //TODO: check references

    protected virtual void Display(string nameItem, bool state)
    {
        Transform item = hudObj.Find(nameItem);
        if (item == null)
        {
            Debug.LogError("Can't find " + nameItem + " for Knight's HudManager");
            return;
        }

        item.gameObject.SetActive(state);
    }
}
