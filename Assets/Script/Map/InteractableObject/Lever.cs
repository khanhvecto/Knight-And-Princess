using System.Collections;
using UnityEngine;

public class Lever : InteractableObject, SignalSourceInterface
{
    [Header("References")]
    [SerializeField] protected Animator animator;
        //Signal receiver
    [SerializeField] protected Transform signalReceiverObj;
    protected SignalReceiverInterface signalReceiverInterface;

    //State
    protected bool turnedOn = false;
    protected bool sendingSignal = false;

    protected override void LoadReferences()
    {
        base.LoadReferences();
        //Animator
        this.animator = transform.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find animtor for Lever of " + transform.name);
        //SignalReceiver
        this.signalReceiverInterface = this.signalReceiverObj.GetComponent<SignalReceiverInterface>();
        if (this.signalReceiverInterface == null) Debug.LogError("Can't find signal receiver interface for lever");
    }

    protected override void Update()
    {
        base.Update();
        if (!base.interactable) return;
        if (!base.interacted) return;

        if (!this.sendingSignal)
        {
            this.sendingSignal = true;
            this.turnedOn = !this.turnedOn; //Change state
            StartCoroutine(SendSignal());   //Send signal
        }
    }

    //SignalSourceInterface
    public IEnumerator SendSignal()
    {
        //Lever animation
        this.animator.SetBool("turnOn", this.turnedOn);
        float waitTime = this.animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(waitTime);

        if(CameraFollow.Instance.isFollowingPlayer)
        {
            //Focus camera to signal receiver obj
            yield return StartCoroutine(CameraFollow.Instance.SetMoveToPos(this.signalReceiverObj.transform.position));
            //Send signal and back to knight
            this.signalReceiverInterface.ReceiveSignal(this.turnedOn);
                //Wait 1 secs
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(CameraFollow.Instance.FocusToKnight());
        }
        else
        {
            this.signalReceiverInterface.ReceiveSignal(this.turnedOn);
        }

        //Reset state
        base.ResetInteract();
        if(base.interactable) base.SetPopUpShowing(true);
        this.sendingSignal = false;
    }
}
