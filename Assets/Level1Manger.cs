using UnityEngine;

public class Level1Manger : MonoBehaviour
{
    protected void Start()
    {
        AudioManager.Instance.PlayBgm(BgmType.Lvl1);
    }
}
