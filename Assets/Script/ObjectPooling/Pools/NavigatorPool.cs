using UnityEngine;

public class NavigatorPool : ObjectPooling
{
    [Header("Singleton")]
    protected static NavigatorPool instance;
    public static NavigatorPool Instance { get => instance; }

    protected override void SetSingleton()
    {
        if(NavigatorPool.Instance != null)
        {
            Debug.Log("Only 1 NavigatorPool allowed to exist!");
            Destroy(gameObject);
        }
        instance = this;
    }
}
