using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject pauseScreen;

    protected void Update()
    {
        // Check for pause game
        if (InputManager.Instance.GetPauseGameKeyDown())
        {
            bool isPauseScreenShowing = this.pauseScreen.activeSelf;
            this.pauseScreen.SetActive(!isPauseScreenShowing);
        }
    }

    #region Pause screen
    public void MainMenuButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Menu", SceneLoader.LoadingSceneType.ChangeLevel);
    }

    public void RestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
