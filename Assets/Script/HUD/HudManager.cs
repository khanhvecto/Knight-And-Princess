using UnityEngine;

public abstract class HudManager: MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform hudObj;

    protected virtual void Start()
    {
        if (this.hudObj == null) Debug.LogError("Can't find HUD Object for Knight's HudManager");
    }

    protected virtual void Display(string nameItem, bool state)
    {
        Transform item = hudObj.Find(nameItem);
        if (item == null)
        {
            Debug.LogError("Can't find " + nameItem + " for HudManager");
            return;
        }

        item.gameObject.SetActive(state);
    }
}