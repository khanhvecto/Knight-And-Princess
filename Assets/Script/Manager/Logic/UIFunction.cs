using UnityEngine;

public class UIFunction : MonoBehaviour
{
    private static UIFunction instance;

    [Header("Screens")]
    [SerializeField] private GameObject deadScreeen;

    public static UIFunction Instance { get => instance;}

    private void Awake()
    {
        //Design pattern
        if (UIFunction.instance != null) Debug.LogError("Only 1 UIFunction allow to exist!");
        UIFunction.instance = this;
    }

    public void RestartButtonClicked()
    {
        GamePlayLogic.Instance.RespawnKnight();
    }

    public void ShowDeadScreen(bool option)
    {
        deadScreeen.SetActive(option);
    }
}
