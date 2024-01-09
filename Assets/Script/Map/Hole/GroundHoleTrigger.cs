using UnityEngine;

public class GroundHoleTrigger : MonoBehaviour
{
    private HoleTrigger holeTriggerScript;

    private void Awake()
    {
        holeTriggerScript = transform.parent.GetComponent<HoleTrigger>();
        if (holeTriggerScript == null) Debug.LogError("Can't find hole trigger for " + transform.parent.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        holeTriggerScript.respawnPlace = this.transform;
    }
}
