using UnityEngine;

public class BrickPool : ObjectPooling
{
    [Header("------ BRICK POOL ------")]
    [Header("Singleton")]
    protected static BrickPool instance;
    public static BrickPool Instance { get => instance; }

    protected override void SetSingleton()
    {
        if(BrickPool.Instance != null)
        {
            Destroy(BrickPool.Instance);
        }

        instance = this;
    }
}
