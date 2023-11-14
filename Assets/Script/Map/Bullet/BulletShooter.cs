using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject spawnPoint;
    [SerializeField] protected GameObject bulletPoolObj;
    protected ObjectPooling bulletPoolScript;

    [Header("Stats")]
    [SerializeField] protected Vector2 direction;
    [SerializeField] protected float cooldown;
    protected float timer=0f;

    protected void Awake()
    {
        //spawn point
        if (this.spawnPoint == null) Debug.LogError("Can't find spawn point for bullet shooter");
        //bullet pool obj
        if (this.bulletPoolObj == null) Debug.LogError("Can't find bullet pool obj for bullet shooter");
        //bullet pool script
        this.bulletPoolScript = this.bulletPoolObj.GetComponent<ObjectPooling>();
        if (this.bulletPoolScript == null) Debug.LogError("Can't find bullet pool script for bullet shooter");
    }

    protected void FixedUpdate()
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
        GameObject bullet = this.bulletPoolScript.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        //Set bullet stats
        bulletScript.SetDirection(this.direction);
        //bulletScript.SetPool(this.objectPool);
        bulletScript.SetSpawner(gameObject);
        bullet.transform.position = this.spawnPoint.transform.position;
        //Put bullet in pool folder
    }
}