using UnityEngine;

public class PlayerEnduranceRestoration : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected PlayerStats statsScript;

    protected void Start()
    {
        this.CheckReferences();
        this.InitStats();
    }

    protected void CheckReferences()
    {
        // stats script
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerEnduranceRestoration of " + transform.parent.name);
    }

    protected void InitStats()
    {
        this.statsScript.currentEnduranceRestoreSpeed = this.statsScript.maxEnduranceRestoreSpeed;
    }

    protected void Update()
    {
        if (this.statsScript.enduranceRestoreable)
            this.CheckRestoreEndurance();
    }

    protected void CheckRestoreEndurance()
    {
        if (this.statsScript.CurrentEndurance == this.statsScript.maxEndurance)
            return;

        var newEndurance = this.statsScript.CurrentEndurance + this.statsScript.currentEnduranceRestoreSpeed * Time.deltaTime;
        
        // Check to decrease restore speed
        if (newEndurance < this.statsScript.maxEndurance / 4 && this.statsScript.currentEnduranceRestoreSpeed == this.statsScript.maxEnduranceRestoreSpeed)
            this.statsScript.currentEnduranceRestoreSpeed = this.statsScript.maxEnduranceRestoreSpeed / 3;
        else if (newEndurance >= this.statsScript.maxEndurance / 4 && this.statsScript.currentEnduranceRestoreSpeed < this.statsScript.maxEnduranceRestoreSpeed)
            this.statsScript.currentEnduranceRestoreSpeed = this.statsScript.maxEnduranceRestoreSpeed;

        this.statsScript.SetCurrentEnduranceValue(newEndurance);
    }
}
