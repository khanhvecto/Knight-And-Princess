using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform knightPosition;
    [SerializeField] private float followSpeed;
    [SerializeField] private float heightOffset;
    private float zAxisPosition = -10f;


    private void Update()
    {
        Vector3 newPosition = new Vector3(knightPosition.position.x, knightPosition.position.y + heightOffset, zAxisPosition);
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed*Time.deltaTime);
    }
}
