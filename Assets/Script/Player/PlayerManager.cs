using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Singleton")]
    protected static PlayerManager instance;
    public static PlayerManager Instance { get => instance; }

    [Header("References")]
    [SerializeField] protected PlayerStats statsScript;

    [Header("Respawn Knight")]
    [SerializeField] public Transform checkPoint;

    protected void Start()
    {
        this.SetSingleton();
    }

    protected void SetSingleton()
    {
        if(PlayerManager.instance != null)
        {
            Debug.Log("Only 1 PlayerManager allowed to exist");
            Destroy(this);
        }
        instance = this;
    }

    public void SetCheckPoint(Transform pos)
    {
        //Reset old checkPoint
        this.checkPoint.TryGetComponent<CheckPoint>(out CheckPoint oldCheckPointScript);
        if (oldCheckPointScript != null) oldCheckPointScript.ResetObject();

        //Set new checkPoint
        this.checkPoint = pos;
    }

    public void RevivePlayer()
    {
        if (!this.statsScript.isDead)
            return;

        this.TeleportPlayer(this.checkPoint);
        this.statsScript.animator.SetTrigger("endState");

        StartCoroutine(CameraFollow.Instance.FocusToKnight());
    }

    public void TeleportPlayer(Transform newPlace)
    {
        transform.parent.position = newPlace.position;
    }
}
