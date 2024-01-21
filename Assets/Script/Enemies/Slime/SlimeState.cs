using Unity.VisualScripting;
using UnityEngine;

public class SlimeState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Animator animator;
    [SerializeField] public LayerMask knightLayer;
    [SerializeField] private SlimeStats statScript;
    [SerializeField] protected GameObject hudObj;

    [Header("Control state")]
    public bool isCombating = false;
    public bool facingLeft = true;

    [Header("Targeting")]
    public Collider2D targetColl;

    [Header("Flip")]
    [SerializeField] public Rigidbody2D rb2D;

    protected virtual void Start()
    {
        this.CheckReferences();
    }

    private void CheckReferences()
    {
        //statScript
        if (statScript == null) Debug.LogError("Can't find EnemyStats for " + gameObject.name);
        //Animator
        if (this.animator == null) Debug.LogError("Can't find Animator for EnemyState of" + gameObject.name);
        // HudObj
        if (this.hudObj == null) Debug.LogError("Can't find HUD Object for Slime State");
    }

    public void SetAttackState(bool state)  //Set attack state
    {
        animator.SetBool("attackState", state);
        isCombating = state;
    }

    public void Flip()  //Flip slime
    {
        rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
        transform.parent.Rotate(0f, 180f, 0f);
        this.hudObj.transform.Rotate(0f, 180f, 0f);
        facingLeft = !facingLeft;
        this.statScript.combatRangeOffset.x = -this.statScript.combatRangeOffset.x;
    }

    //Dead state
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        //Draw combat range
        if (this.isCombating)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + this.statScript.combatRangeOffset, this.statScript.combatRangeSize * this.statScript.RANGE_COEFF);
        }
        else
        {
            Gizmos.DrawWireCube((Vector2)transform.position + this.statScript.combatRangeOffset, this.statScript.combatRangeSize);
        }
    }
}
