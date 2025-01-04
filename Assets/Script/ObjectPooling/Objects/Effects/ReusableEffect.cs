using UnityEngine;

public abstract class ReusableEffect : ObjectFromPool
{
    [Header("References")]
    [SerializeField] protected ParticleSystem effect;

    protected void Start()
    {
        this.CheckReferences();
    }

    protected void CheckReferences()
    {
        // Particle system
        if (this.effect == null)
            Debug.LogError("Can't find particle system of ReusableEffect");
    }

    protected override bool IsNeedToRelease()
    {
        if (this.effect.isStopped) 
            return true;
        return false;
    }
}
