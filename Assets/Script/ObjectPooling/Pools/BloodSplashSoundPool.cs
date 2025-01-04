using UnityEngine;

public class BloodSplashSoundPool : ObjectPooling
{
    [Header("Singleton")]
    protected static BloodSplashSoundPool instance;
    public static BloodSplashSoundPool Instance { get => instance; }

    protected override void SetSingleton()
    {
        if(BloodSplashSoundPool.Instance != null)
        {
            Debug.LogError("Only 1 BloodSplashSoundPool allowed to exist");
            Destroy(gameObject);
        }
        instance = this;
    }
}
