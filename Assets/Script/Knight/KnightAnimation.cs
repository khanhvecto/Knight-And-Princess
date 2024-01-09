using UnityEngine;

public class KnightAnimation : MonoBehaviour
{
    //Desing pattern
    private static KnightAnimation instance;
    public static KnightAnimation Instance { get => instance; }

    private void Awake()
    {
        //Design pattern
        if (instance != null) Debug.LogError("Only 1 KnightAnimation allow to exist");
        instance = this;
    }

    //Attack
    public void TriggerAttack()
    {
        KnightAttack.Instance.TriggerAttack();
    }

    //Roll
    public void StartRoll()
    {
        KnightRoll.Instance.StartRoll();
    }
    public void EndRoll()
    {
        KnightRoll.Instance.EndRoll();
    }

    //Hurt
    public void ResetControl()
    {
        KnightState.Instance.controlable = true;
    }
}
