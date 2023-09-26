using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyState : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask knightLayer;
    [SerializeField] private CombatStats statSO;

    [Header("Combat range")]
    [SerializeField] private Vector2 combatRangeSize;           //For manually set combat check range
    [SerializeField] private Vector2 combatRangeOffset;         //
    private static float RANGE_COEFF;  //is (combat range/ control range). After switch to combat state,
                                                //the controlRange is become bigger.
    private bool isCombating = false;
    [HideInInspector] public Collider2D targetColl;
    public bool attacking;

    [Header("Flip")]
    [SerializeField] private Rigidbody2D rb2D;
    [HideInInspector] public bool facingLeft = false;

    private void Start()
    {
        RANGE_COEFF = statSO.range_coeff;
    }

    // Update is called once per frame
    void Update()
    {
        checkCombat();
    }

    void checkCombat()
    {
        if (!isCombating && inRangeAttack())
        {   //If player in combat Range, then set combat state
            setAttackState(true);
        }
        else if (isCombating && !inRangeAttack())
        {   //If player is out of combat Range, then set to normal state
            setAttackState(false);
        }
    }
    public void setAttackState(bool state)
    {
        animator.SetBool("attackState", state);
        isCombating = state;
    }

    private bool inRangeAttack()    //If someone in range of attack
    {
        if(!isCombating)
        {
            targetColl = Physics2D.OverlapBox((Vector2)transform.position + combatRangeOffset, combatRangeSize, 0f, knightLayer);
        }
        else
        {
            targetColl = Physics2D.OverlapBox((Vector2)transform.position + combatRangeOffset, combatRangeSize * RANGE_COEFF, 0f, knightLayer);
        }
        return targetColl;
    }

    public void flip()
    {
        rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
        transform.Rotate(0f, 180f, 0f);
        facingLeft = !facingLeft;
        combatRangeOffset.x = -combatRangeOffset.x;
    }

    private void OnDrawGizmosSelected()
    {
        if (isCombating)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + combatRangeOffset, combatRangeSize * RANGE_COEFF);
        }
        else
        {
            Gizmos.DrawWireCube((Vector2)transform.position + combatRangeOffset, combatRangeSize);
        }
    }
}
