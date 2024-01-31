using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    protected void Start()
    {
        AudioManager.Instance.PlayBgm(BgmType.Lvl3);
    }
}
