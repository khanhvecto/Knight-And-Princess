using UnityEngine;

public abstract class DamagableObj: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float damage;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 7)    //Knight layer
        {
            KnightHurt.Instance.TakeDamage(this.damage);
            this.OnTouchingKnight();
        }
        else if(collision.collider.gameObject.layer == 6)    //Ground layer
        {
            this.OnTouchingGround();
        }
    }

    protected virtual void OnTouchingKnight()
    {

    }
    protected virtual void OnTouchingGround()
    {

    }
}