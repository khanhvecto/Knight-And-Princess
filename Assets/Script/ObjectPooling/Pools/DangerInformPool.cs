using UnityEngine;

public class DangerInformPool : ObjectPooling
{
    [Header("Singleton")]
    private static DangerInformPool instance;
    public static DangerInformPool Instance { get => instance; }

    protected override void SetSingleton()
    {
        if(DangerInformPool.instance != null)
        {
            Debug.Log("Only 1 DangerInformPool allowed to exist!");
            Destroy(gameObject);
        }    
        instance = this;
    }
}
