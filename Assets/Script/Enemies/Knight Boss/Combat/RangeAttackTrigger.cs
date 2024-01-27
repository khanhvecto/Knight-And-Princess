using System.Collections;
using UnityEngine;

public class RangeAttackTrigger : MonoBehaviour
{
    [Header("--- REFERENCES ---")]
    [SerializeField] protected Skill skillScriptableObj;

    [Header("--- STATS ---")]
    [Header("Collider")]
    [SerializeField] protected LayerMask targetLayerMask;
    protected int targetLayer;
    protected float scaleTime = 0.1f;
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
            Debug.LogError("Can't find skill scriptable object for RangeAttackTrigger of " + transform.parent.parent.parent.name);
    }

    protected void LoadStats()
    {
        this.damage = this.skillScriptableObj.damage;
        this.enduranceDecrement = this.skillScriptableObj.enduranceDecrement;

        // Find layer
        int tmp = this.targetLayerMask.value;
        this.targetLayer = 0;
        while (tmp > 1)
        {
            this.targetLayer++;
            tmp = tmp >> 1;
        }
    }

    protected void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(this.ScaleTrigger());
    }

    protected IEnumerator ScaleTrigger()
    {
        float scaleTimer = 0f;
        while (scaleTimer <= this.scaleTime)
        {
            // Scale up over time
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scaleTimer / this.scaleTime);
            scaleTimer += Time.deltaTime;
            yield return null;
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.layer != this.targetLayer) return;

        IDamageReceiver receiveDamageScript = collision.GetComponentInChildren<IDamageReceiver>();
        receiveDamageScript?.GotHit(this.damage * Time.deltaTime, transform.parent.parent.parent.position, this.enduranceDecrement);
    }
}