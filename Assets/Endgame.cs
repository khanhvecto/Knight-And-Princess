using System.Collections;
using UnityEngine;

public class Endgame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject dialog1;
    [SerializeField] protected GameObject dialog2;
    [SerializeField] protected GameObject dialog3;

    void Start()
    {
        StartCoroutine(this.ShowDialogs());
    }

    protected IEnumerator ShowDialogs()
    {
        this.dialog1.SetActive(true);
        yield return new WaitForSeconds(5f);

        this.dialog1.SetActive(false);
        this.dialog2.SetActive(true);
        yield return new WaitForSeconds(5f);

        this.dialog2.SetActive(false);
        this.dialog3.SetActive(true);
        yield return new WaitForSeconds(5f);

        SceneLoader.Instance.LoadScene("Menu", SceneLoader.LoadingSceneType.ChangeLevel);
    }
}
