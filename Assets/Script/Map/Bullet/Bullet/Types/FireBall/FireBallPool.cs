using UnityEngine;

public class FireBallPool : ObjectPooling
{
    [Header("------ FIRE BALL POOL ------")]

    [Header("Singleton")]
    protected static FireBallPool instance;
    public static FireBallPool Instance { get => instance; }

    protected override void SetSingleton()
    {
        if(FireBallPool.Instance != null)
        {
            Debug.Log("Only 1 fire ball pool is allowed to exist!");
            Destroy(FireBallPool.Instance);
        }

        instance = this;
    }
}
