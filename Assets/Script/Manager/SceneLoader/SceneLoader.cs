using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Singleton
    private static SceneLoader instance;
    public static SceneLoader Instance => instance;

    [Header("References")]
        //Loading Scene Obj
    [SerializeField] protected GameObject loadingSceneObj;
    [SerializeField] protected SceneLoading sceneLoadingScript;
        //Loading Icon
    [SerializeField] protected GameObject loadingIconObj;

    //Loading type
    public enum LoadingSceneType
    {
        ChangeLevel
    }

    protected virtual void Awake()
    {
        this.SetSingletonAndDontDestroyOnLoad();
        this.LoadReferences();
    }

    //
    //Init steps
    //
    protected void SetSingletonAndDontDestroyOnLoad()
    {
        if (instance != null)
        {
            Debug.LogError("Only 1 SceneLoader allows to exist");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    protected virtual void LoadReferences()
    {
        //Loading icon obj
        this.loadingIconObj = transform.Find("Canvas").Find("Loading Icon").gameObject;
        if (this.loadingIconObj == null) Debug.LogError("Can't find loading icon obj for SceneLoader");
    }
    protected void SetLoadSceneObj(LoadingSceneType type)
    {
        //Loading scene obj
        this.loadingSceneObj = transform.Find("Canvas").Find(type.ToString()).gameObject;
        if (this.loadingSceneObj == null) Debug.LogError("Can't find Loading Scene Obj for SceneLoader");
        //SceneLoading Script
        this.sceneLoadingScript = this.loadingSceneObj.GetComponent<SceneLoading>();
        if (this.sceneLoadingScript == null) Debug.LogError("Can't find SceneLoading Script for SceneLoader");
    }

    //
    //Load scene
    //
    public void LoadScene(string sceneName, LoadingSceneType type)
    {
        this.SetLoadSceneObj(type);
        StartCoroutine(this.SetLoadScene(sceneName));
    }

    protected IEnumerator SetLoadScene(string sceneName)
    {
        //Start load
        this.loadingSceneObj.SetActive(true);
        yield return StartCoroutine(this.sceneLoadingScript.StartLoad());
        this.loadingIconObj.SetActive(true);

        //Loading
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        //End load
        this.loadingIconObj.SetActive(false);
        yield return StartCoroutine(this.sceneLoadingScript.EndLoad());
        this.loadingSceneObj.SetActive(false);
    }
}