using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private static CameraMovement instance;
    public static CameraMovement Instance { get => instance; }

    [SerializeField] private Transform knightPosition;
    private float followSpeed;
    [SerializeField] private float heightOffset;
    [SerializeField] private float forwardRange = 4f;
    private float zAxisPosition = -10f;
    private float forwardOffset;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 CameraMovement allow to exist!");
        instance = this;

        followSpeed = KnightMovement.Instance.Speed;
    }

    private void Update()
    {
        if (KnightState.Instance.facingRight)
        {
            this.forwardOffset = Mathf.Abs(this.forwardRange);
        }
        else
        {
            this.forwardOffset = -Mathf.Abs(this.forwardRange);
        }
        Vector3 newPosition = new Vector3(knightPosition.position.x + forwardOffset, knightPosition.position.y + heightOffset, zAxisPosition);
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed*Time.deltaTime);
    }
}
