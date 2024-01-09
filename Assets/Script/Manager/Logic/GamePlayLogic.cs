using UnityEngine;

public class GamePlayLogic : MonoBehaviour
{
    //Singleton
    private static GamePlayLogic instance;
    public static GamePlayLogic Instance { get => instance;}

    [Header("Respawn Knight")]
    [SerializeField] public Transform checkPoint;
    private GameObject knightObj;

    private void Awake()
    {
        //Singleton
        if (GamePlayLogic.instance != null) Debug.LogError("Only 1 GamePlayLogic allow to exist!");
        GamePlayLogic.instance = this;

        knightObj = GameObject.FindGameObjectWithTag("Knight");
    }

    public void SetCheckPoint(Transform pos)
    {
        //Reset old checkPoint
        this.checkPoint.TryGetComponent<CheckPoint>(out CheckPoint oldCheckPointScript);
        if (oldCheckPointScript != null) oldCheckPointScript.ResetObject();

        //Set new checkPoint
        this.checkPoint = pos;
    }

    public void RespawnKnight() //Need to respawn knight position to the check point and reset some basic stats
    {
        //Respawn knight body
        this.TeleportKnight(checkPoint);
        KnightState.Instance.setRespawn();
        //Reset camera
        CameraMovement.Instance.ResetToKnightPos();
    }

    public void TeleportKnight(Transform newPlace)
    {
        knightObj.transform.position = newPlace.position;
    }
}
