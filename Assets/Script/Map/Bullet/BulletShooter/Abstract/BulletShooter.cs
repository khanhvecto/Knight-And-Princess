using System.Collections;
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
    [SerializeField] protected float enduranceDecrement;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float speed;
    [SerializeField] protected float despawnDistance;

    [Header("States")]
    protected bool shootable = true;

    protected void Start()
    {
        this.LoadBulletPool();
    }

    protected void Update()
    {
        if(this.shootable)
        {
            this.Shoot();
            StartCoroutine(this.Cooldowning());
        }
    }

    protected IEnumerator Cooldowning()
    {
        this.shootable = false;
        yield return new WaitForSeconds(this.cooldown);
        this.shootable = true;
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
        bulletDamage.SetEnduranceDecrement(this.enduranceDecrement);
        bullet.transform.position = this.spawnPoint.transform.position;
    }

    protected abstract void LoadBulletPool();
}