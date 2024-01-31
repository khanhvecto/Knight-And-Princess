using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunction : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] protected PlayerManager managerScript;
    [SerializeField] protected GameObject deadScreeen;

    protected void Start()
    {
        this.CheckReferences();
    }

    protected void CheckReferences()
    {
        // Manager script
        if (this.managerScript == null)
            Debug.LogError("Can't find manager script for UIFunction of " + transform.parent.name);
        // Dead screen
        if (this.deadScreeen == null)
            Debug.LogError("Can't find dead screen for UIFunction of " + transform.parent.name);
    }

    #region Buttons

    public void ReviveButtonClicked()
    {
        this.managerScript.RevivePlayer();
    }

    #endregion

    #region Screens

    public void ShowDeadScreen(bool state)
    {
        deadScreeen.SetActive(state);
    }

    #endregion
}
