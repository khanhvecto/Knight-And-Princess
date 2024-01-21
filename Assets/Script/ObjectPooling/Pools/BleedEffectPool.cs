using UnityEngine;

public class BleedEffectPool : ObjectPooling
{
    [Header("Singleton")]
    private static BleedEffectPool instance;
    public static BleedEffectPool Instance { get => instance; }

    protected override void SetSingleton()
    {
        if(BleedEffectPool.instance != null)
        {
            Debug.LogError("Only 1 BloodEffectPool allowed to exist!");
            Destroy(gameObject);
        }
        instance = this;
    }
}
