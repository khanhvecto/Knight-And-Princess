using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    protected void Start()
    {
        AudioManager.Instance.PlayBgm(BgmType.Menu);
    }
}
