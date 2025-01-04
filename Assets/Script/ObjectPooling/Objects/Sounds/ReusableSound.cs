using UnityEngine;

public abstract class ReusableSound : ObjectFromPool
{
    // References
    [SerializeField] protected AudioSource audioSource;

    protected void Start()
    {
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        // audio source
        this.audioSource = this.GetComponent<AudioSource>();
        if (this.audioSource == null)
            Debug.LogError("Can't find audio source for ReusableSound of " + this.name);
    }

    protected override bool IsNeedToRelease()
    {
        if (this.audioSource.isPlaying) 
            return false;
        
        return true;
    }
}
