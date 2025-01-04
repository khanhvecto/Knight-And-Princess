using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPooling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject objectPrefab;
    [SerializeField] protected List<GameObject> objectPool;

    protected void Awake()
    {
        this.SetSingleton();
        this.CheckReferences();
    }

    protected abstract void SetSingleton();

    protected virtual void CheckReferences()
    {
        //Prefab
        if (this.objectPrefab == null) Debug.LogError("Can't find prefab for ObjectPool");
    }

    public GameObject Get() //Get object from pool
    {
        //if found any inactive obj, then set it active and return
        foreach(GameObject obj1 in this.objectPool)
        {
            if(!obj1.activeSelf)
            {
                obj1.SetActive(true);
                return obj1;
            }
        }

        //if not found any, initailize one then return
        GameObject obj2 = Instantiate(this.objectPrefab);
        this.objectPool.Add(obj2);
        obj2.transform.SetParent(transform);
        return obj2;
    }

    public void Release(GameObject returnObj)
    {
        returnObj.SetActive(false);
    }
}