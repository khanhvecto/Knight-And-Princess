using UnityEngine;

public class BuffManager : MonoBehaviour
{
    //Singleton
    private static BuffManager instance;
    public static BuffManager Instance { get => instance; }

    protected void Awake()
    {
        //Singleton
        if (instance != null) Debug.LogError("Only 1 BuffHudManager allows to exist");
        instance = this;
    }

    protected void Update()
    {
        this.CheckUseBuff();
    }

    protected void CheckUseBuff()   //Check if player want to use buff
    {
        if (InputManager.Instance.GetHealthBuffKeyDown())
        {
            HealthBuff.Instance.TryUseBuff();
        }
        if (InputManager.Instance.GetShieldBuffKeyDown())
        {
            ShieldBuff.Instance.TryUseBuff();
        }
        if (InputManager.Instance.GetDamageBuffKeyDown())
        {
            DamageBuff.Instance.TryUseBuff();
        }
        if (InputManager.Instance.GetInvincibleBuffKeyDown())
        {
            InvincibleBuff.Instance.TryUseBuff();
        }
    }
}