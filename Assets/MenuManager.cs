using UnityEngine;

public class MenuManager : MonoBehaviour
{
    protected void Start()
    {
        AudioManager.Instance.PlayBgm(BgmType.Menu);
    }
}
