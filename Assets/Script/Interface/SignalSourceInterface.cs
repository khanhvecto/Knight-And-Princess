using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface SignalSourceInterface
{
    IEnumerator SendSignal();   //Need to move camera cinematiclly
}