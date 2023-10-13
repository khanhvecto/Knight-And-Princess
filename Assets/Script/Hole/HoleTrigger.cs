using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    [HideInInspector] public Transform respawnPlace;
    [SerializeField] private float fallDamage;
    private void Awake()
    {
        respawnPlace = transform.Find("Left Ground").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            GamePlayLogic.Instance.TeleportKnight(respawnPlace);
            //Player take fall damage;
            KnightHurt.Instance.TakeDamage(fallDamage);
        }
    }
}
