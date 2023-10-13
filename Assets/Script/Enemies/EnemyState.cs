using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask knightLayer;
    [SerializeField] private EnemyStats statScript;

    [Header("Control state")]
    public bool isCombating = false;
    public bool facingLeft = true;

    [Header("Targeting")]
    public Collider2D targetColl;

    [Header("Flip")]
    [SerializeField] private Rigidbody2D rb2D;

    private void Start()
    {
        this.CheckReferences();
    }

    private void CheckReferences()
    {
        //statScript
        this.statScript = transform.Find("Stats").GetComponent<EnemyStats>();
        if (statScript == null) Debug.LogError("Can't find EnemyStats for " + gameObject.name);
        //Animator
        this.animator = gameObject.GetComponent<Animator>();
        if (this.animator == null) Debug.LogError("Can't find Animator for EnemyState of" + gameObject.name);
    }

    void Update()
    {
        checkCombat();
    }

    void checkCombat()  //Switch between combat and normal state
    {
        if (!isCombating && inRangeAttack())    //If player in combat Range
        {
            setAttackState(true);
        }
        else if (isCombating && !inRangeAttack())   //If player is out of combat Range
        {
            setAttackState(false);
        }
    }
    public void setAttackState(bool state)  //Set attack state
    {
        animator.SetBool("attackState", state);
        isCombating = state;
    }

    private bool inRangeAttack()    //If someone in range of attack
    {
        if(!isCombating)
        {
            targetColl = Physics2D.OverlapBox((Vector2)transform.position + this.statScript.combatRangeOffset, this.statScript.combatRangeSize, 0f, knightLayer);
        }
        else
        {
            targetColl = Physics2D.OverlapBox((Vector2)transform.position + this.statScript.combatRangeOffset, this.statScript.combatRangeSize * this.statScript.RANGE_COEFF, 0f, knightLayer);
        }
        return targetColl;
    }

    public void flip()  //Flip slime
    {
        rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
        transform.Rotate(0f, 180f, 0f);
        facingLeft = !facingLeft;
        this.statScript.combatRangeOffset.x = -this.statScript.combatRangeOffset.x;
    }

    //Dead state
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
