using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayLogic : MonoBehaviour
{
    private static GamePlayLogic instance;
    public static GamePlayLogic Instance { get => instance;}

    [Header("Respawn Knight")]
    [SerializeField] public Transform checkPoint;
    private GameObject knightObj;

    private void Awake()
    {
        //Design pattern
        if (GamePlayLogic.instance != null) Debug.LogError("Only 1 GamePlayLogic allow to exist!");
        GamePlayLogic.instance = this;

        knightObj = GameObject.FindGameObjectWithTag("Knight");
    }

    public void SetCheckPoint(Transform pos)
    {
        this.checkPoint = pos;
    }

    public void RespawnKnight() //Need to respawn knight position to the check point and reset some basic stats
    {
        //Respawn knight body
        this.TeleportKnight(checkPoint);
        KnightState.Instance.setRespawn();
    }

    public void TeleportKnight(Transform newPlace)
    {
        knightObj.transform.position = newPlace.position;
    }
}
