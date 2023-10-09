using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBlock : MonoBehaviour
{
    //Design pattern
    private static KnightBlock instance;
    public static KnightBlock Instance { get => instance; }

    [Header("References")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightBlock allow to exist!");
        instance = this;
    }

    void Start()
    {
        //Check references
        if (this.animator == null) Debug.LogError("Can't find animator for Knight Block");
    }

    void Update()
    {
        if(KnightState.Instance.controlable)
        {
            this.SetBlock();
        }
    }

    private void SetBlock()
    {
        if (!KnightState.Instance.blocking && InputManager.Instance.GetBlockKeyDown())
        {
            this.StartBlock();
        }
        else if (KnightState.Instance.blocking && InputManager.Instance.GetBlockKeyUp())
        {
            this.EndBlock();
        }
    }

    //Block
    private void StartBlock()
    {
        animator.SetTrigger("block");
        animator.SetBool("blocking", true);
        //Set blocking state
        KnightState.Instance.blocking = true;
        if(KnightState.Instance.facingRight)
        {
            KnightState.Instance.rightBlocking = true;
        }
        else
        {
            KnightState.Instance.rightBlocking = false;
        }
        //Endurance
        KnightStats.Instance.restoringEndurance = false;
    }
    private void EndBlock()
    {
        animator.SetBool("blocking", false);
        //Blocking state
        KnightState.Instance.blocking = false;
        //Endurance
        KnightStats.Instance.restoringEndurance = true;
    }
}
