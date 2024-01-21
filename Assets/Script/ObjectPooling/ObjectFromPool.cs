using UnityEngine;

public abstract class ObjectFromPool : MonoBehaviour
{
    protected void Update()
    {
        this.AutoRelease();
    }

    protected void AutoRelease()
    {
        if (!this.IsNeedToRelease()) return;

        this.ReleaseObject();
    }

    protected abstract bool IsNeedToRelease();

    protected abstract void ReleaseObject();
}
