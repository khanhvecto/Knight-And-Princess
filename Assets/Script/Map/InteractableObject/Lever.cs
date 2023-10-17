using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    [Header("References")]
    [SerializeField] protected Animator animator;

    //State
    protected bool turnedOn = false;

    protected override void Start()
    {
        base.Start();

        this.LoadReferences();
    }
    protected virtual void LoadReferences()
    {
        //Animator
        this.animator = transform.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find animtor for Lever of " + transform.name);
    }
    protected override void Update()
    {
        base.Update();

        if (base.interacted)
        {
            this.turnedOn = !this.turnedOn;
            this.animator.SetBool("turnOn", this.turnedOn);

            //ResetObject after that to make lever can be push again
            base.ResetObject();
            base.SetPopUpShowing(true);
        }
    }
}
