using UnityEngine;

public abstract class ObjectFromPool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected string poolTag;
    protected ObjectPooling poolScript;

    protected void Start()
    {
        this.LoadReferences();
    }

    protected virtual void LoadReferences()
    {
        // pool script
        this.poolScript = GameObject.FindGameObjectWithTag(this.poolTag)?.GetComponent<ObjectPooling>();
        if (this.poolScript == null)
            Debug.LogError("Can't find pool script for ObjectFromPool of " + gameObject.name);
    }

    protected void Update()
    {
        this.AutoRelease();
    }

    protected void AutoRelease()
    {
        if (!this.IsNeedToRelease()) return;

        this.poolScript?.Release(gameObject);
    }

    protected abstract bool IsNeedToRelease();
}
