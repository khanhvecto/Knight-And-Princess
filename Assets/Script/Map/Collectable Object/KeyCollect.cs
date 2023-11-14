using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : CollectableObj, SignalSourceInterface
{
    [Header("References")]
    [SerializeField] protected Transform signalReceiver;
    protected SignalReceiverInterface signalReceiverInterface;

    protected virtual void Awake()
    {
        this.LoadReferences();
    }
    protected virtual void LoadReferences()
    {
        //Signal receiver
        if (this.signalReceiver == null) Debug.LogError("Need signal Receiver for key");
        //SignalReceiverInterface
        this.signalReceiverInterface = this.signalReceiver.GetComponentInChildren<SignalReceiverInterface>();
    }

    protected override IEnumerator CollectObj()
    {
        if (this.signalReceiverInterface == null) yield break;
        yield return StartCoroutine(this.SendSignal());
    }

    //Signal source interface
    public IEnumerator SendSignal()
    {
        //Focus camera to signal receiver obj
        yield return StartCoroutine(CameraMovement.Instance.FocusToObject(this.signalReceiver.transform.position));
        //Send signal and back to knight
        this.signalReceiverInterface.ReceiveSignal(true);
        yield return StartCoroutine(CameraMovement.Instance.FocusToKnight());
    }
}
