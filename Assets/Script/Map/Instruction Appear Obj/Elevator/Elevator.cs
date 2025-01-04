using System.Collections;
using UnityEngine;

public class Elevator : InstructionAppearObj, SignalReceiverInterface
{
    [Header("References")]
    [SerializeField] protected Animator animator;

    //State
    public bool working = false;
    public bool moving = false;

    //Instruction name
    protected string signalRequiredInstruction = "SignalRequired";

    [Header("Position")]
    [SerializeField] protected Transform firstPos;
    [SerializeField] protected Transform secondPos;
    public Vector3 newPos;

    //Stats
    protected float speed = 4f;

    protected override void LoadReferences()
    {
        base.LoadReferences();
        //Animator
        this.animator = GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find animator for Elevator of " + transform.name);
        //First position
        this.firstPos = transform.parent.Find("Position").Find("FirstPos");
        if (this.firstPos == null) Debug.LogError("Can't find first position for Elevator of " + transform.name);
        //Second position
        this.secondPos = transform.parent.Find("Position").Find("SecondPos");
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
            this.StartMove(); 
        }
        else    //Reset intereacted state
        {
            base.ResetInteract();
            base.ShowInstruction(this.signalRequiredInstruction, true);
        }
    }

    public virtual void StartMove()   //Set up before move
    {
        this.moving = true;

        //Find newPos
        if (transform.position == this.firstPos.position)
        {
            this.newPos = this.secondPos.position;
        }
        else
        {
            this.newPos = this.firstPos.position;
        }

        //Move
        StartCoroutine(this.Move());
    }

    protected IEnumerator Move()
    {
        while(transform.position != this.newPos)
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
