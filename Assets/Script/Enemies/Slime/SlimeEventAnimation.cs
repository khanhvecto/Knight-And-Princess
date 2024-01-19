using UnityEngine;

public class SlimeEventAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlimeState stateScript;
    [SerializeField] private SlimeDamageSender combatScript;

    private void Start()
    {
        this.LoadReferences();
    }
    private void LoadReferences()
    {
        //EnemyState
        if (this.stateScript == null) Debug.LogError("Can't find EnemyState for EnemyEventAnimation of " + gameObject.name);
        //EnemyCombat
        if (this.combatScript == null) Debug.LogError("Can't find EnemyCombat for EnemyEventAnimation of " + gameObject.name);
    }

    //Attack events
    public void SendDamage1()
    {
        this.combatScript.SendDamage(1);
    }
    public void SendDamage2()
    {
        this.combatScript.SendDamage(2);
    }

    //Dead events
    public void DesTroyEnemy()
    {
        this.stateScript.DestroyEnemy();
    }
}
