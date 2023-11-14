using System.Collections;
using UnityEngine;

public class Elevator : InstructionAppearObj, SignalReceiverInterface
{
    [Header("References")]
    [SerializeField] protected Animator animator;

    //State
    protected bool working = false;
    protected bool moving = false;

    //Instruction name
    protected string signalRequiredInstruction = "SignalRequired";

    [Header("Position")]
    [SerializeField] protected Vector3 firstPos;
    [SerializeField] protected Vector3 secondPos;
    protected string firstPosName = "FirstPos";
    protected string secondPosName = "SecondPos";

    //Stats
    protected float speed = 4f;

    protected override void LoadReferences()
    {
        base.LoadReferences();
        //Animator
        this.animator = GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find animator for Elevator of " + transform.name);
        //First position
        this.firstPos = transform.Find(this.firstPosName).position;
        if (this.firstPos == null) Debug.LogError("Can't find first position for Elevator of " + transform.name);
        //Second position
        this.secondPos = transform.Find(this.secondPosName).position;
        if (this.secondPos == null) Debug.LogError("Can't find second position for Elevator of " + transform.name);
    }

    protected override void Update()
    {
        base.Update();
        this.CheckMove();
    }

    protected virtual void CheckMove()
    {
        if (!this.interactable) return;
        if (!base.interacted) return;
        if (this.working) 
        {
            if (this.moving) return;
            this.moving = true;
            this.StartMove(); 
        }
        else    //Reset intereacted state
        {
            base.ResetInteract();
            base.ShowInstruction(this.signalRequiredInstruction, true);
        }
    }

    protected virtual void StartMove()   //Set up before move
    {
        //Find newPos
        Vector3 newPos;
        if (transform.position == this.firstPos)
        {
            newPos = this.secondPos;
        }
        else
        {
            newPos = this.firstPos;
        }

        //Move
        StartCoroutine(this.Move(newPos));
    }

    protected IEnumerator Move(Vector3 newPos)
    {
        while(transform.position != newPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, this.speed * Time.deltaTime);
            yield return null;
        }

        //Reset after move to new pos
        base.ResetInteract();
        if(base.interactable) base.SetPopUpShowing(true);
        this.moving = false;
    }

    //SignalReceiverInterface
    public void ReceiveSignal(bool signal)
    {
        this.working = signal;
        this.animator.SetBool("working", signal);
    }
}
