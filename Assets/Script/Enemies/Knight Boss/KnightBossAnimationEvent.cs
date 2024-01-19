using UnityEngine;

public class KnightBossAnimationEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlimeState stateScript;

    private void Start()
    {
        this.LoadReferences();
    }
    private void LoadReferences()
    {
        //EnemyState
        if (this.stateScript == null) Debug.LogError("Can't find EnemyState for KnightBossAnimationEvent of " + gameObject.name);
    }

    //Dead events
    public void DesTroyEnemy()
    {
        this.stateScript.DestroyEnemy();
    }
}
