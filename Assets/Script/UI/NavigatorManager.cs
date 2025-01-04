using System.Collections;
using TMPro;
using UnityEngine;

public class NavigatorManager : MonoBehaviour
{
    //Singleton
    private static NavigatorManager instance;
    public static NavigatorManager Instance { get => instance; }

    [Header("References")]
        //Position
    [SerializeField] protected Transform defaultPos;
    [SerializeField] protected Transform displayPos;
        //Navigator pool
    [SerializeField] protected ObjectPooling pool;

    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 80f;
    [SerializeField] protected float displayTime = 3f;

    protected void Awake()
    {
        //Singleton
        if (instance != null) Debug.LogError("Only 1 Navigator allows to exist!");
        instance = this;

        //Check references
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        //Default position
        this.defaultPos = transform.Find("Position").Find("Default").transform;
        if (this.defaultPos == null) Debug.LogError("Can't find default pos for Navigator");
        //Display position
        this.displayPos = transform.Find("Position").Find("Display").transform;
        if (this.displayPos == null) Debug.LogError("Can't find display pos for Navigator");
        //Navigator pool
        this.pool = transform.Find("Pool").GetComponent<ObjectPooling>();
        if (this.pool == null) Debug.LogError("Can't find navigator pool for Navigator");
    }

    public void ShowNavigator(string text)
    {
        GameObject navigator = this.pool.Get();

        //Set stats
        navigator.GetComponentInChildren<TextMeshProUGUI>().text = text;  //text
        navigator.transform.position = this.defaultPos.position;    //position
        navigator.transform.SetAsLastSibling(); //Make sure new navigator is display above old navigator

        StartCoroutine(this.MoveNavigator(navigator));
    }

    protected IEnumerator MoveNavigator(GameObject navigator)
    {
        //Move to display position
        while(navigator.transform.position != this.displayPos.position)
        {
            navigator.transform.position = Vector3.MoveTowards(navigator.transform.position, this.displayPos.position, this.moveSpeed * Time.deltaTime);
            yield return null;
        }

        //Displaying
        yield return new WaitForSeconds(this.displayTime);

        //Return to pool
        this.pool.Release(navigator);
    }
}