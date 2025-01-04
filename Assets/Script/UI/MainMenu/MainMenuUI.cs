using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void NewGameButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Level 1", SceneLoader.LoadingSceneType.ChangeLevel);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    #region Level list

    public void Level1ButtonLicked()
    {
        SceneLoader.Instance.LoadScene("Level 1", SceneLoader.LoadingSceneType.ChangeLevel);
    }
    public void Level2ButtonLicked()
    {
        SceneLoader.Instance.LoadScene("Level 2", SceneLoader.LoadingSceneType.ChangeLevel);
    }
    public void Level3ButtonLicked()
    {
        SceneLoader.Instance.LoadScene("Level 3", SceneLoader.LoadingSceneType.ChangeLevel);
    }
    public void Level4ButtonLicked()
    {
        SceneLoader.Instance.LoadScene("Level 4", SceneLoader.LoadingSceneType.ChangeLevel);
    }

    #endregion
}
