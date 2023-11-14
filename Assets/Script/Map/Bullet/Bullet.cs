using UnityEngine;

public class Bullet : DamagableObj
{
    [Header("References")]
    [SerializeField] protected GameObject spawner;
    [SerializeField] protected GameObject bulletPoolObj;
    [SerializeField] protected string bulletPoolTag;
    protected ObjectPooling bulletPoolScript;

    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected float despawnDistance;
    protected Vector2 direction;

    protected void Start()
    {
        //Bullet pool obj
        this.bulletPoolObj = GameObject.FindWithTag(this.bulletPoolTag);
        if (this.bulletPoolObj == null) Debug.LogError("Can't find bullet pool obj for bullet");
        //bullet pool script
        this.bulletPoolScript = this.bulletPoolObj.GetComponent<ObjectPooling>();
        if (this.bulletPoolScript == null) Debug.LogError("Can't find ObjectPooling script for bullet");
    }

    protected void FixedUpdate()
    {
        this.MoveBullet();
        this.CheckDespawn();
    }

    //
    // Damage
    //
    protected override void OnTouchingKnight()
    {
        base.OnTouchingKnight();
        this.Despawn();
    }

    //
    // Moving
    //
    protected void MoveBullet()
    {
        transform.position = transform.position + (Vector3)direction * speed * Time.fixedDeltaTime;
    }
    protected override void OnTouchingGround()
    {
        base.OnTouchingGround();
        this.Despawn();
    }

    //
    // Set stats
    //
    public void SetDirection(Vector2 newDirection)
    {
        this.direction = newDirection;
    }
    public void SetSpawner(GameObject spawner)
    {
        this.spawner = spawner;
    }

    //
    //Despawn
    //
    protected void CheckDespawn()
    {
        //Despawn by length
        if(Vector2.Distance((Vector2) transform.position, (Vector2) this.spawner.transform.position) >= this.despawnDistance)
        {
            this.Despawn();
        }
    }
    protected void Despawn()
    {
        this.bulletPoolScript.Release(gameObject);
    }
}