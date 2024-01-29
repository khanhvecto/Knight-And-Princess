using UnityEngine;

public class DamagableObj: MonoBehaviour
{
    [Header("------ DAMAGABLE OBJECT ------")]

    [Header("References")]
    [SerializeField] protected LayerMask targetLayerMask;
    protected int targetLayer;

    [Header("Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float enduranceDecrement;

    protected virtual void Start()
    {
        this.targetLayer = this.FindLayer(this.targetLayerMask);
    }

    protected int FindLayer(LayerMask layerMask)
    {
        int counter = 0;
        while (layerMask > 1)
        {
            counter++;
            layerMask = layerMask >> 1;
        }
        return counter;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision == null) 
            return;

        if (collision.collider.gameObject.layer == this.targetLayer)    //Knight layer
        {
            IDamageReceiver receiveDamageScript = collision.collider.GetComponentInChildren<IDamageReceiver>();
            receiveDamageScript?.GotHit(this.damage, (Vector2) transform.position + gameObject.GetComponent<Collider2D>().offset, this.enduranceDecrement);

            this.OnTouchingTarget();
        }
    }

    protected virtual void OnTouchingTarget()
    {

    }
}