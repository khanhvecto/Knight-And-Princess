using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class KnightHurt : MonoBehaviour
{
    //Design pattern
    private static KnightHurt instance;
    public static KnightHurt Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightHurt allow to exist!");
        instance = this;
    }

    private void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for KnightHurt");
    }

    public void GotAttack(float damage, Transform attackPos)
    {
        //If not blocking
        if (!KnightState.Instance.blocking)
        {
            TakeDamage(damage, attackPos);
        }
        //If blocking
        else
        {
            //If bloking right and attacked left
            if (KnightState.Instance.rightBlocking && this.CheckLeftAttack(attackPos))
            {
                KnightState.Instance.flip();
                this.TakeDamage(damage);
            }
            //If blocking left and attacked right
            else if(!KnightState.Instance.rightBlocking && !this.CheckLeftAttack(attackPos))
            {
                KnightState.Instance.flip();
                this.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
        //Dont flip
    {
        //Animator
        this.animator.SetTrigger("gotHurt");
        KnightState.Instance.setFallBack();
        //Decrease health
        KnightState.Instance.health -= damage;
        //Set state
        KnightState.Instance.controlable = false;
    }
    public void TakeDamage(float damage, Transform attackPos)
        //Can flip if not facing attackPos
    {
        //Check flip
        if(KnightState.Instance.facingRight && this.CheckLeftAttack(attackPos))
        {
            KnightState.Instance.flip();
        }
        else if(!KnightState.Instance.facingRight && !this.CheckLeftAttack(attackPos))
        {
            KnightState.Instance.flip();
        }
        //Animator
        this.animator.SetTrigger("gotHurt");
        KnightState.Instance.setFallBack();
        //Decrease health
        KnightState.Instance.health -= damage;
        //Set state
        KnightState.Instance.controlable = false;
    }

    public void CheckDead()
    {
        if (KnightState.Instance.health == 0)
        {
            KnightState.Instance.setDead();
        }
    }

    private bool CheckLeftAttack(Transform attackPos)
    {
        if (attackPos.position.x < transform.parent.parent.position.x) return true;
        return false;
    }
}
