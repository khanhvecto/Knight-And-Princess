using UnityEngine;

public abstract class BulletDespawn : ObjectFromPool
    // Despawn by distance
{
    [Header("------ BULLET DESPAWN ------")]

    [Header("References")]
    protected GameObject spawner;

    [Header("Stats")]
    [SerializeField] protected Vector2 defaultDirection;
    protected float speed;
    protected float despawnDistance;
    protected Vector2 direction;

    protected override bool IsNeedToRelease()
    {
        if (this.spawner == null)
            return false;

        if (Vector2.Distance((Vector2)transform.position, (Vector2)this.spawner.transform.position) >= this.despawnDistance)
            return true;
        return false;
    }

    protected void FixedUpdate()
    {
        this.MoveBullet();
    }

    protected void MoveBullet()
    {
        transform.position = transform.position + (Vector3)direction * speed * Time.fixedDeltaTime;
    }

    #region Set Stats

    public void SetDirection(Vector2 newDirection)
    {
        this.direction = newDirection;

        // Rotate bullet
        this.RotateBullet(newDirection);
    }

    protected virtual void RotateBullet(Vector2 newDirection)
        // If the bullet has a default head, this function make the bullet head to the right direction
    {
        var angle = Vector2.SignedAngle(this.defaultDirection, newDirection);
        transform.Rotate(0f, 0f, angle);

        this.defaultDirection = newDirection;
    }

    public void SetSpawner(GameObject spawner)
    {
        this.spawner = spawner;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetDespawnDistance(float distance)
    {
        this.despawnDistance = distance;
    }
    #endregion
}
