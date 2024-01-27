using UnityEngine;

public abstract class BulletShooter : MonoBehaviour
{
    [Header("------ BULLET SHOOTER ------")]

    [Header("References")]
    [SerializeField] protected GameObject spawnPoint;
    protected ObjectPooling bulletPool;

    [Header("Stats")]
    [SerializeField] protected Vector2 direction;
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float speed;
    [SerializeField] protected float despawnDistance;
    protected float timer=0f;

    protected void Start()
    {
        this.LoadBulletPool();
    }

    protected void Update()
    {
        if (this.Shootable()) this.Shoot();
    }

    protected bool Shootable()  
    {
        if (this.timer > this.cooldown)
        {
            this.timer = 0f;
            return true;
        }
        this.timer += Time.fixedDeltaTime;
        return false;
    }

    protected void Shoot()
    {
        GameObject bullet = this.bulletPool.Get();
        BulletDespawn bulletDespawn = bullet?.GetComponent<BulletDespawn>();
        BulletDamage bulletDamage = bullet?.GetComponent<BulletDamage>();

        if (bulletDespawn == null || bulletDamage == null)
            return;

        //Set bullet stats
        bulletDespawn.SetDirection(this.direction);
        bulletDespawn.SetSpawner(gameObject);
        bulletDespawn.SetSpeed(this.speed);
        bulletDespawn.SetDespawnDistance(this.despawnDistance);
        bulletDamage.SetDamage(this.damage);
        bullet.transform.position = this.spawnPoint.transform.position;
    }

    protected abstract void LoadBulletPool();
}