using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected PlayerStats statsScript;

    protected void Start()
    {
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        // Stats script
        if (this.statsScript == null)
            Debug.LogError("Can't find stats script for PlayerManager of " + transform.parent.name);
    }

    public void RevivePlayer()
    {
        if(this.statsScript.isDead)
            this.statsScript.animator.SetTrigger("endState");
    }
}
