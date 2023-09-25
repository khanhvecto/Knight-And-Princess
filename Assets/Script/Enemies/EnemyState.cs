using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyState : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask knightLayer;

    [Header("Combat range")]
    [SerializeField] private Vector2 combatRangeSize;           //For manually set combat check range
    [SerializeField] private Vector2 combatRangeOffset;      //
    private float combatRangeCoefficient = 2f;  //is (combat range/ control range). After switch to combat state,
                                                //the controlRange is become bigger.
    private bool isCombating = false;
    [HideInInspector] public Collider2D targetColl;

    // Update is called once per frame
    void Update()
    {
        checkCombat();
        
    }

    void checkCombat()
    {
        if (!isCombating && Physics2D.OverlapBox((Vector2)transform.position + combatRangeOffset, combatRangeSize, 0f, knightLayer))
        {   //If player in combat Range, then set combat state
            targetColl = Physics2D.OverlapBox((Vector2)transform.position + combatRangeOffset, combatRangeSize, 0f, knightLayer);
            setAttackState(true);
        }
        else if (isCombating && !Physics2D.OverlapBox((Vector2)transform.position + combatRangeOffset, combatRangeCoefficient * combatRangeSize, 0f, knightLayer))
        {   //If player is out of combat Range, then set to normal state
            setAttackState(false);
        }
    }

    public void flip()
    {
        combatRangeOffset = -combatRangeOffset;
    }

    public void setAttackState(bool state)
    {
        animator.SetBool("attackState", state);
        isCombating = state;
    }

    private void OnDrawGizmosSelected()
    {
        if (isCombating)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + combatRangeOffset, combatRangeSize * combatRangeCoefficient);
        }
        else
        {
            Gizmos.DrawWireCube((Vector2)transform.position + combatRangeOffset, combatRangeSize);
        }
    }
}
