using UnityEngine;

public class Level2Manger : MonoBehaviour
{
    protected void Start()
    {
        AudioManager.Instance.PlayBgm(BgmType.Lvl2);
    }
}
