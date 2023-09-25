using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CombatStats statSO;
    [SerializeField] EnemyState stateScript;
    private float health;
    private float damage;
    private float cooldown;
    // Start is called before the first frame update
    private void Start()
    {
        //Get stats
        health = statSO.health;
        damage = statSO.damage;
        cooldown = statSO.cooldown;
    }

    public void gotHit(float damage)
    {
        animator.SetTrigger("gotHit");
        stateScript.setAttackState(true);
        health -= damage;
        if (health <= 0)
        {
            gameObject.layer = 9; //Dead layer
            animator.SetBool("isDead", true);
        }
    }
    private void destroyObject()
    {
        Destroy(gameObject);
    }
}
