using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyState stateScript;
    [SerializeField] private EnemyCombat combatScript;

    private void Start()
    {
        this.LoadReferences();
    }
    private void LoadReferences()
    {
        //EnemyState
        this.stateScript = gameObject.GetComponent<EnemyState>();
        if (this.stateScript == null) Debug.LogError("Can't find EnemyState for EnemyEventAnimation of " + gameObject.name);
        //EnemyCombat
        this.combatScript = gameObject.GetComponent<EnemyCombat>();
        if (this.combatScript == null) Debug.LogError("Can't find EnemyCombat for EnemyEventAnimation of " + gameObject.name);
    }

    //Attack events
    public void SendDamage()
    {
        this.combatScript.attack();
    }

    //Dead events
    public void DesTroyEnemy()
    {
        this.stateScript.DestroyEnemy();
    }
}
