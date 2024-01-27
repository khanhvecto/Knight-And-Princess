using System.Collections;
using UnityEngine;

public class KeyCollect : CollectableObj, SignalSourceInterface
{
    [Header("--- KEY COLLECT ---")]
    [Header("References")]
    [SerializeField] protected Transform signalReceiverObj;
    protected SignalReceiverInterface signalReceiverInterface;

    protected virtual void Awake()
    {
        this.LoadReferences();
    }

    protected virtual void LoadReferences()
    {
        this.signalReceiverInterface = this.signalReceiverObj.GetComponentInChildren<SignalReceiverInterface>();
    }

    protected override IEnumerator CollectObj()
    {
        if (this.signalReceiverInterface == null) yield break;
        NavigatorManager.Instance.ShowNavigator("Something just opened!");

        if (CameraFollow.Instance.isFollowingPlayer)
            yield return StartCoroutine(this.SendSignal());
        else
            this.signalReceiverInterface.ReceiveSignal(true);
    }

    //Signal source interface
    public IEnumerator SendSignal()
    {
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(CameraFollow.Instance.SetMoveToPos(this.signalReceiverObj.transform.position));

        this.signalReceiverInterface.ReceiveSignal(true);

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(CameraFollow.Instance.FocusToKnight());
    }
}
