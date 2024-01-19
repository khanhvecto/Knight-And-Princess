using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected Skill skillScriptableObj;

    [Header("--- STATS ---")]
    [Header("Collider")]
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected float range;
    [Header("Combat")]
    protected float damage;
    protected float enduranceDecrement;

    protected void Start()
    {
        this.CheckReferences();
        this.LoadStats();
    }

    protected void CheckReferences()
    {
        // skill scriptable object
        if (this.skillScriptableObj == null)
            Debug.LogError("Can't find skill scriptable object for MeleeAttack of " + transform.parent.parent.parent.name);
    }

    protected void LoadStats()
    {
        this.damage = this.skillScriptableObj.damage;
        this.enduranceDecrement = this.skillScriptableObj.enduranceDecrement;
    }

    public void Attack()
    {
        Collider2D hostileColl = Physics2D.OverlapCircle((Vector2) transform.position, this.range, this.targetLayer);
        if (hostileColl == null)
            return;

        IDamageReceiver receiveDamageScript = hostileColl.GetComponentInChildren<IDamageReceiver>();
        receiveDamageScript?.GotHit(this.damage, transform.parent.parent.parent, this.enduranceDecrement);
    }

    //protected void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, this.range);
    //}
}
