using UnityEngine;

public class KnightBoss_Combat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SlimeState stateScript;
    [SerializeField] protected SlimeStats statScript;

    void Start()
    {
        this.LoadReferences();
    }

    protected void LoadReferences()
    {
        // state script
        if (this.stateScript == null) 
            Debug.LogError("Can' find state script for KnigthBoss_Combat of " + transform.parent.name);
        // stat script
        if (this.statScript == null) 
            Debug.LogError("Can' find stat script for KnigthBoss_Combat of " + transform.parent.name);
    }

    public void ChooseAttack()
    {
        // Choose a random attack
        int rand = Random.Range(1, this.statScript.attackTypeNumber + 1);
        this.stateScript.animator.SetTrigger("attack" + rand);
    }
}
